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
        private const double OggBufferSize = 0.1;

        public static AudioEngine Instance;

        // AudioContext.Dispose causes popping sound
        private readonly AudioContext sharedContext;

        private readonly ConcurrentDictionary<int, ALSourceState> playStates;
        private readonly List<int> currentSources;
        private readonly object oggQueueLock = new object();

        private float volume = 1.0f;

        private int oggSource;
        private string oggFilePath;
        private long oggLoopStart;
        private long oggLoopEnd;
        private long oggPausePosition;

        private long oggTotalSamples;
        private long oggProgress;
        private int oggSampleRate;
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
            this.currentSources = new List<int>();
        }
        #endregion

        #region Events

        public event EventHandler<OggPlaybackEventArgs> OggPlayback;

        #endregion

        #region Properties

        public void WavPlay(byte[] wavBytes)
        {
            using (var memoryStream = new MemoryStream(wavBytes))
            {
                this.BeginWav(memoryStream);
            }
        }

        public void WavPlay(string fileName)
        {
            using (var fileStream = File.Open(fileName, FileMode.Open))
            {
                this.BeginWav(fileStream);
            }
        }

        public void WavPlay(WAVData wavData)
        {
            this.BeginWav(wavData);
        }

        public void OggPlay(string filePath, bool isLooping, long? sampleStart = null, long? sampleEnd = null)
        {
            if (this.IsInProgress)
            {
                return;
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

            this.BeginOgg(isLooping);
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

        public void WavStop()
        {
            var sources = this.playStates.Keys;
            foreach (var k in sources)
            {
                if (k == this.oggSource)
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
            AL.SourceStop(this.oggSource);

            this.OggSeek(0);

            this.UpdateOggProgress(0);
        }

        public void OggPause()
        {
            this.OggSeek(this.oggProgress);
            this.playStates[this.oggSource] = ALSourceState.Stopped;
            AL.SourceStop(this.oggSource);
        }

        public void OggSeek(long sample)
        {
            this.oggPausePosition = sample;

            this.UpdateOggProgress(sample);
        }

        public float Volume
        {
            get
            {
                return this.volume;
            }

            set
            {
                this.volume = value;

                foreach (var source in this.currentSources)
                {
                    AL.Source((uint)source, ALSourcef.Gain, value);
                }
            }
        }

        private bool IsInProgress => this.playStates.Any(kvp => kvp.Value == ALSourceState.Playing);
        #endregion

        #region Methods
        private void BeginWav(Stream byteStream)
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

            this.BeginWav(wavData);
        }

        private void BeginWav(WAVData wavData)
        {
            var buffer = AL.GenBuffer();
            var currentSource = GenSourceWithVolume();

            var soundFormat = wavData.SoundFormat;
            AL.BufferData(buffer, soundFormat, wavData.Data, wavData.Data.Length, wavData.Rate);

            AL.Source(currentSource, ALSourcei.Buffer, buffer);

            this.Play(currentSource, () => { AL.DeleteBuffer(buffer); });
        }

        private void BeginOgg(bool isLooping)
        {
            if (string.IsNullOrEmpty(this.oggFilePath) || this.IsInProgress)
            {
                return;
            }

            lock (this.oggQueueLock) { }

            this.oggSource = GenSourceWithVolume();
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
            var queueThread = new Thread(() =>
            {
                lock (this.oggQueueLock)
                {
                    for (int i = 0; i < OggBufferCount; i++)
                    {
                        var currentBuffer = AL.GenBuffer();
                        QueueBuffer(currentSource, currentBuffer, vorbis, soundFormat, oggSampleRate, valuesPerBuffer, sampleEnd);
                        buffers.Add(currentBuffer);
                    }

                    var allBuffersProcessed = false;
                    while (!buffersInitialized || !playbackStopDetected)
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
                    }
                }

                vorbis.Dispose();
            });
            queueThread.Start();

            while (!buffersInitialized)
            {
                Thread.Sleep(10);
            }

            this.OggPlayback?.Invoke(this, new OggPlaybackEventArgs { StateChange = ALSourceState.Playing });
            this.Play(currentSource, () =>
            {
                playbackStopDetected = true;
                lock (this.oggQueueLock)
                {
                    foreach (var buf in buffers)
                    {
                        AL.DeleteBuffer(buf);
                    }
                    this.OggPlayback?.Invoke(this, new OggPlaybackEventArgs
                    {
                        StateChange = ALSourceState.Stopped
                    });
                }
                this.oggSource = -1;
            });

        }

        private void Play(int source, Action bufferCleanup, Action callback = null)
        {
            var waitThread = new Thread(() =>
            {
                AL.SourcePlay(source);
                AL.GetSource(source, ALGetSourcei.SourceState, out int state);
                this.playStates[source] = (ALSourceState)state;

                // Query the source to find out when it stops playing.
                do
                {
                    Thread.Sleep(250);
                    if (this.playStates[source] == ALSourceState.Playing)
                    {
                        AL.GetSource(source, ALGetSourcei.SourceState, out state);
                        this.playStates[source] = (ALSourceState)state;
                        this.playStates[source] = this.playStates[source];
                    }
                    callback?.Invoke();
                }
                while (this.playStates[source] != ALSourceState.Stopped);
                
                AL.SourceStop(source);
                AL.DeleteSource(source);
                this.currentSources.Remove(source);
                this.playStates.TryRemove(source, out _);
                bufferCleanup();
            });

            waitThread.Start();
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

        private int GenSourceWithVolume()
        {
            var source = AL.GenSource();
            AL.Source(source, ALSourcef.Gain, this.volume);
            this.currentSources.Add(source);
            return source;
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

                if (!t.Wait(10) || t.Result <= 0)
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
