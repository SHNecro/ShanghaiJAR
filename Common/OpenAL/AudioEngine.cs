using NVorbis;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.OpenAL
{
    public class AudioEngine : IDisposable
    {
        #region Fields
        private const int OggBufferCount = 10;
        private const double OggBufferSize = 0.05;

        public const string DefaultStartGroup = "default";
        public const string DefaultVolumeGroup = "default";

        public static AudioEngine Instance;

        // AudioContext.Dispose causes popping sound
        private readonly AudioContext sharedContext;

        private readonly ConcurrentDictionary<int, ALSourceState> playStates;
        private readonly ConcurrentDictionary<int, string> playStateGroups;
        private readonly Dictionary<string, ConcurrentDictionary<int, int>> currentSources;
        private readonly object oggQueueLock = new object();
        private readonly ConcurrentDictionary<int, Thread> runningThreads;

        private IDictionary<string, float> volumes = new Dictionary<string, float> { { AudioEngine.DefaultVolumeGroup, 0.5f } };

        private int oggSource;
        private string oggFilePath;
        private long oggLoopStart;
        private long oggLoopEnd;
        private long oggPausePosition;

        private long oggTotalSamples;
        private long oggProgress;
        private int oggSampleRate;

        private bool oggPlaybackEnded;
        #endregion

        #region Constructor

        static AudioEngine()
        {
            AudioEngine.Instance = new AudioEngine();
        }

        private AudioEngine()
        {
            this.sharedContext = new AudioContext();
            this.playStates = new ConcurrentDictionary<int, ALSourceState>();
            this.playStateGroups = new ConcurrentDictionary<int, string>();
            this.currentSources = new Dictionary<string, ConcurrentDictionary<int, int>>();
            this.runningThreads = new ConcurrentDictionary<int, Thread>();
        }
        #endregion

        #region Events

        public event EventHandler<OggPlaybackEventArgs> OggPlayback;

        #endregion

        #region Properties

        public void WavPlay(byte[] wavBytes, string volumeGroup = AudioEngine.DefaultVolumeGroup, string startGroup = DefaultStartGroup)
        {
            using (var memoryStream = new MemoryStream(wavBytes))
            {
                this.BeginWav(memoryStream, volumeGroup, startGroup);
            }
        }

        public void WavPlay(string fileName, string volumeGroup = AudioEngine.DefaultVolumeGroup, string startGroup = DefaultStartGroup)
        {
            using (var fileStream = File.Open(fileName, FileMode.Open))
            {
                this.BeginWav(fileStream, volumeGroup, startGroup);
            }
        }

        public void WavPlay(WAVData wavData, string volumeGroup = AudioEngine.DefaultVolumeGroup, string startGroup = DefaultStartGroup)
        {
            this.BeginWav(wavData, volumeGroup, startGroup);
        }

        public void OggPlay(string filePath, bool isLooping, long? sampleStart = null, long? sampleEnd = null, string volumeGroup = AudioEngine.DefaultVolumeGroup)
        {
            if (this.IsInProgress)
            {
                this.OggStop();
            }

            this.oggFilePath = filePath;

            LoadOggInfo(filePath, out long sampleRate, out long totalSamples);
            this.oggLoopStart = sampleStart ?? 0;
            this.oggLoopEnd = sampleEnd ?? totalSamples;

            this.OggPlayback?.Invoke(this, new OggPlaybackEventArgs
            {
                StateChange = ALSourceState.Initial,
                SampleRate = sampleRate,
                TotalSamples = totalSamples
            });

            this.BeginOgg(isLooping, volumeGroup);
        }

        public void OggInitialize(string filePath)
        {
            LoadOggInfo(filePath, out long sampleRate, out long totalSamples);
            this.OggPlayback?.Invoke(this, new OggPlaybackEventArgs
            {
                StateChange = ALSourceState.Initial,
                SampleRate = sampleRate,
                TotalSamples = totalSamples
            });
        }

        public void WavStop(string startGroup = null)
        {
            var sources = this.playStates.Keys;
            foreach (var k in sources)
            {
                if (k == this.oggSource)
                {
                    continue;
                }

                var isStoppedGroup = startGroup == null || (this.playStateGroups.ContainsKey(k) && this.playStateGroups[k] == startGroup);
                if (!isStoppedGroup)
                {
                    continue;
                }

                this.playStates[k] = ALSourceState.Stopped;
                AL.SourceStop(k);
            }
        }

        public void OggStop()
        {
            this.playStates[this.oggSource] = ALSourceState.Stopped;
            AL.SourcePause(this.oggSource);
            AL.SourceStop(this.oggSource);
            this.StopPlayback();

            this.WaitOnLock(this.oggQueueLock, () =>
            {
                this.OggSeek(0);
                this.UpdateOggProgress(0);
            });
        }

        public void OggPause()
        {
            this.OggSeek(this.oggProgress);
            this.playStates[this.oggSource] = ALSourceState.Stopped;
            AL.SourcePause(this.oggSource);
            this.StopPlayback();
        }

        public void OggSeek(long sample)
        {
            this.oggPausePosition = sample;

            this.UpdateOggProgress(sample);
        }

        public void SetVolume(float volume, string volumeGroup = AudioEngine.DefaultVolumeGroup)
        {
            this.volumes[volumeGroup] = volume;

            if (this.currentSources.TryGetValue(volumeGroup, out var groupSources))
            {
                foreach (var source in groupSources.Values)
                {
                    AL.Source((uint)source, ALSourcef.Gain, volume);
                }
            }
        }

        public float GetVolume(string volumeGroup = AudioEngine.DefaultVolumeGroup)
        {
            return this.volumes[volumeGroup];
        }

        private bool IsInProgress => this.playStates.Any(kvp => kvp.Value == ALSourceState.Playing);
        #endregion

        #region Methods
        private void BeginWav(Stream byteStream, string volumeGroup, string startGroup)
        {
            WAVData wavData;
            try
            {
                wavData = LoadBytes(byteStream);
            }
            catch (NotSupportedException)
            {
                throw new InvalidOperationException("Invalid .wav file");
            }

            this.BeginWav(wavData, volumeGroup, startGroup);
        }

        private void BeginWav(WAVData wavData, string volumeGroup, string startGroup)
        {
            var buffer = AL.GenBuffer();
            var currentSource = GenSourceWithVolume(volumeGroup);

            var soundFormat = wavData.SoundFormat;
            AL.BufferData(buffer, soundFormat, wavData.Data, wavData.Data.Length, wavData.Rate);

            AL.Source(currentSource, ALSourcei.Buffer, buffer);

            this.Play(currentSource, () => { AL.DeleteBuffer(buffer); }, false, null, startGroup);
        }

        private void BeginOgg(bool isLooping, string volumeGroup)
        {
            if (string.IsNullOrEmpty(this.oggFilePath))
            {
                return;
            }

            lock (this.oggQueueLock) { }
            this.oggPlaybackEnded = false;

            this.oggSource = GenSourceWithVolume(volumeGroup);
            var currentSource = this.oggSource;

            int channels, bits_per_sample;
            VorbisReader vorbis;
            try
            {
                vorbis = new VorbisReader(this.oggFilePath);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException("Invalid .ogg file");
            }

            // get the channels & sample rate
            channels = vorbis.Channels;
            bits_per_sample = 16;
            this.oggSampleRate = vorbis.SampleRate;

            var soundFormat = GetSoundFormat(channels, bits_per_sample);

            // OPTIONALLY: get a TimeSpan indicating the total length of the Vorbis stream
            var totalTime = vorbis.TotalTime;

            // create a buffer for reading samples
            var valuesPerBuffer = (int)(channels * oggSampleRate * OggBufferSize); // 1s/5 = 200ms
            var readBuffer = new float[valuesPerBuffer];

            // get the initial position (obviously the start)
            var position = TimeSpan.Zero;
            this.oggTotalSamples = vorbis.TotalSamples;

            var sampleEnd = isLooping ? this.oggLoopEnd : this.oggTotalSamples;
            if (this.oggProgress < 0 || this.oggProgress >= sampleEnd)
            {
                this.OggSeek(0);
            }

            this.UpdateOggProgress(this.oggPausePosition);
            vorbis.SeekTo(this.oggPausePosition);

            var buffersInitialized = false;

            var buffers = new List<int>();

            var playbackStopDetected = false;
            this.WaitOnLock(this.oggQueueLock, () =>
            {
                for (int i = 0; i < OggBufferCount; i++)
                {
                    var currentBuffer = AL.GenBuffer();
                    QueueBuffer(currentSource, currentBuffer, vorbis, soundFormat, oggSampleRate, valuesPerBuffer, sampleEnd);
                    buffers.Add(currentBuffer);
                }

                var allBuffersProcessed = false;
                while (!buffersInitialized || (!playbackStopDetected && !this.oggPlaybackEnded))
                {
                    buffersInitialized = true;

                    AL.GetSource(currentSource, ALGetSourcei.BuffersQueued, out int buffersQueued);
                    AL.GetSource(currentSource, ALGetSourcei.BuffersProcessed, out int buffersProcessed);

                    var newUnqueuedSize = 0;
                    for (int i = 0; i < buffersProcessed; i++)
                    {
                        var currentBuffer = buffers.First();
                        buffers.Remove(currentBuffer);

                        AL.GetBuffer(currentBuffer, ALGetBufferi.Size, out int currentBufferSize);
                        newUnqueuedSize += currentBufferSize;

                        AL.SourceUnqueueBuffers(currentSource, 1, new[] { currentBuffer });
                        QueueBuffer(currentSource, currentBuffer, vorbis, soundFormat, oggSampleRate, valuesPerBuffer, sampleEnd);
                        buffers.Add(currentBuffer);
                    }

                    allBuffersProcessed = vorbis.SamplePosition >= this.oggTotalSamples && (!isLooping);
                    if (allBuffersProcessed && buffersProcessed != 0)
                    {
                        var unqueuedBuffers = new int[buffersProcessed];
                        AL.SourceUnqueueBuffers(currentSource, buffersProcessed, unqueuedBuffers);
                        foreach (var currentBuffer in unqueuedBuffers)
                        {
                            AL.GetBuffer(currentBuffer, ALGetBufferi.Size, out int currentBufferSize);
                            newUnqueuedSize += currentBufferSize;
                        }
                    }

                    var newProgress = newUnqueuedSize / (channels * (bits_per_sample / 8));
                    var adjustedProgress = this.oggProgress + newProgress;
                    if (AL.GetSourceState(currentSource) == ALSourceState.Playing)
                    {
                        this.UpdateOggProgress(adjustedProgress);
                    }
                    else
                    {
                        AL.SourcePlay(currentSource);
                    }

                    if (isLooping)
                    {
                        var realLoopEnd = Math.Min(this.oggLoopEnd, this.oggTotalSamples);
                        if (vorbis.SamplePosition >= realLoopEnd)
                        {
                            vorbis.SeekTo(this.oggLoopStart);
                        }
                        if (this.oggProgress >= realLoopEnd)
                        {
                            this.UpdateOggProgress(this.oggLoopStart + (this.oggProgress % realLoopEnd));
                        }
                    }
                    else
                    {
                        var realLoopEnd = Math.Min(this.oggLoopEnd, this.oggTotalSamples);
                        if (vorbis.SamplePosition >= realLoopEnd)
                        {
                            this.oggPlaybackEnded = true;
                        }
                    }
                }
                vorbis.Dispose();
            });

            while (!buffersInitialized)
            {
                Thread.Sleep(10);
            }

            this.OggPlayback?.Invoke(this, new OggPlaybackEventArgs { StateChange = ALSourceState.Playing });
            this.Play(currentSource, () =>
            {
                playbackStopDetected = true;
                this.WaitOnLock(this.oggQueueLock, () =>
                {
                    foreach (var buf in buffers)
                    {
                        AL.DeleteBuffer(buf);
                    }
                    this.OggPlayback?.Invoke(this, new OggPlaybackEventArgs
                    {
                        StateChange = ALSourceState.Stopped
                    });
                    this.oggSource = -1;
                });
            },
            true);

        }

        private void Play(int source, Action bufferCleanup, bool isOggPlayback, Action callback = null, string startGroup = DefaultStartGroup)
        {
            var waitThread = this.CreateAndRegisterThread(() =>
            {
                AL.SourcePlay(source);
                AL.GetSource(source, ALGetSourcei.SourceState, out int state);
                this.playStates[source] = (ALSourceState)state;
                this.playStateGroups[source] = startGroup;

                // Query the source to find out when it stops playing.
                do
                {
                    Thread.Sleep(10);
                    if (this.playStates[source] == ALSourceState.Playing)
                    {
                        AL.GetSource(source, ALGetSourcei.SourceState, out state);
                        this.playStates[source] = (ALSourceState)state;
                        this.playStates[source] = this.playStates[source];
                    }
                    callback?.Invoke();
                }
                while (this.playStates[source] != ALSourceState.Stopped || (isOggPlayback && !this.oggPlaybackEnded));
                
                AL.SourceStop(source);
                AL.DeleteSource(source);

                foreach (var volumeGroup in this.currentSources.Values)
                {
                    volumeGroup.TryRemove(source, out _);
                }
                this.playStates.TryRemove(source, out _);
                this.playStateGroups.TryRemove(source, out _);
                bufferCleanup();
            });

            waitThread.Start();
        }

        private void StopPlayback()
        {
            this.oggPlaybackEnded = true;
            // lock (this.oggQueueLock) { }
        }

        private void UpdateOggProgress(long sample)
        {
            if (this.oggSampleRate == 0)
            {
                return;
            }

            this.oggProgress = sample;

            this.OggPlayback?.Invoke(this, new OggPlaybackEventArgs
            {
                SampleRate = this.oggSampleRate,
                TotalSamples = this.oggTotalSamples,
                ProgressSamples = this.oggProgress
            });
        }

        private int GenSourceWithVolume(string volumeGroup)
        {
            var source = AL.GenSource();
            AL.Source(source, ALSourcef.Gain, this.volumes[volumeGroup]);

            if (!this.currentSources.ContainsKey(volumeGroup))
            {
                this.currentSources[volumeGroup] = new ConcurrentDictionary<int, int>();
            }

            this.currentSources[volumeGroup].TryAdd(source, source);
            return source;
        }

        private void WaitOnLock(object lockObject, Action action)
        {
            var waitingThread = this.CreateAndRegisterThread(() =>
            {
                lock (lockObject)
                {
                    action.Invoke();
                }
            });

            waitingThread.Start();
        }

        private Thread CreateAndRegisterThread(Action action)
        {
            var idx = this.runningThreads.Any() ? this.runningThreads.Keys.Max() + 1 : 0;
            Thread newThread = null;
            newThread = new Thread(() =>
            {
                action.Invoke();
                this.runningThreads.TryRemove(idx, out _);
            });

            this.runningThreads.TryAdd(idx, newThread);
            return newThread;
        }

        #endregion

        #region Static Methods
        // Loads a wave/riff audio file.
        public static WAVData LoadBytes(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                var riffSignature = new string(reader.ReadChars(4));
                if (riffSignature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file (RIFF container).");

                var riffSize = reader.ReadInt32();

                var format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                var formatSignature = new string(reader.ReadChars(4));
                if (formatSignature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported (malformed wave format).");

                var waveHeaderSize = reader.ReadInt32();

                var audioFormat = reader.ReadInt16();
                var numChannels = reader.ReadInt16();
                var sampleRate = reader.ReadInt32();
                var byteRate = reader.ReadInt32();
                var blockAlign = reader.ReadInt16();
                var bitsPerSample = reader.ReadInt16();

                // "malformed" wave header in some files (dragonvoice.wav has 18 byte, unknown header)
                var leftoverBytes = waveHeaderSize - 16;
                if (leftoverBytes > 0)
                {
                    reader.ReadBytes(leftoverBytes);
                }

                // Handle other sections (misnamed BWF file)
                var optionalSectionType = string.Empty;
                do
                {
                    optionalSectionType = new string(reader.ReadChars(4));
                    switch (optionalSectionType)
                    {
                        case "JUNK":
                        case "bext":
                        case "iXML":
                        case "qlty":
                        case "mext":
                        case "levl":
                        case "link":
                        case "axml":
                            var sectionSize = reader.ReadInt32();
                            stream.Seek(sectionSize, SeekOrigin.Current);
                            break;
                        case "data":
                            break;
                        default:
                            throw new NotSupportedException("Specified wave file is not supported (nonstandard data header).");
                    }
                } while (optionalSectionType != "data");

                var dataSize = reader.ReadInt32();

                var bytes = reader.ReadBytes(dataSize);

                return new WAVData
                {
                    Data = bytes,
                    Channels = numChannels,
                    Bits = bitsPerSample,
                    Rate = sampleRate
                };
            }
        }

        public static void LoadOggInfo(string filePath, out long sampleRate, out long totalSamples)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                sampleRate = 0;
                totalSamples = 0;
                return;
            }

            try
            {
                using (var vorbis = new VorbisReader(filePath))
                {
                    sampleRate = vorbis.SampleRate;
                    totalSamples = vorbis.TotalSamples;
                }
            }
            catch (FileNotFoundException)
            {
                throw new InvalidOperationException("Invalid .ogg file");
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException("Invalid .ogg file");
            }
        }

        private static void QueueBuffer(int source, int buffer, VorbisReader vorbis, ALFormat format, int sampleRate, int targetSamples, long endSample)
        {
            var shortBuffer = new short[targetSamples];
            try
            {
                var samples = Math.Min(targetSamples, (int)(endSample - vorbis.SamplePosition) * sizeof(short));
                if (samples <= 0)
                {
                    return;
                }

                var floatBuffer = new float[samples];

                var thread = default(Thread);
                var t = Task.Factory.StartNew(() =>
                {
                    thread = Thread.CurrentThread;
                    return vorbis.ReadSamples(floatBuffer, 0, samples);
                });

                if (!t.Wait((int)(OggBufferSize * 100)) || t.Result <= 0)
                {
                    thread?.Abort();
                    return;
                }

                shortBuffer = new short[samples];

                for (int i = 0; i < samples; i++)
                {
                    var temp = (int)(short.MaxValue * floatBuffer[i]);
                    if (temp > short.MaxValue) temp = short.MaxValue;
                    else if (temp < short.MinValue) temp = short.MinValue;
                    shortBuffer[i] = (short)temp;
                }
            }
            finally
            {
                AL.BufferData(buffer, format, shortBuffer, shortBuffer.Length * sizeof(short), sampleRate);
                AL.SourceQueueBuffer(source, buffer);
            }
        }

        private static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var thread in this.runningThreads.Values)
                    {
                        thread.Abort();
                    }
                    this.sharedContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
