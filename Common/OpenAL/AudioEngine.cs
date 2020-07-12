using NVorbis;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Common.OpenAL
{
    public class AudioEngine : IDisposable
    {
        #region Fields
        private const int OggBufferCount = 10;
        private const double OggBufferSize = 0.1;

        // AudioContext.Dispose causes popping sound
        private readonly AudioContext sharedContext;

        private ConcurrentDictionary<int, ALSourceState> playStates;
        private List<int> currentSources;

        private float volume = 1.0f;

        private string wavFilePath;
        private string oggFilePath;
        private long oggLoopStart;
        private long oggLoopEnd;
        private long oggPausePosition;
        private object oggQueueLock = new object();

        private long oggTotalSamples;
        private long oggProgress;
        private int oggSampleRate;
        #endregion

        #region Constructor
        public AudioEngine()
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

        public void PlayWaveCommand(string filePath)
        {
            this.wavFilePath = filePath;
            this.PlayWave();
        }

        public void PlayOggCommand(string filePath, bool isLooping, long? sampleStart = null, long? sampleEnd = null)
        {
            this.oggFilePath = filePath;

            this.LoadOggInfo(filePath, sampleStart, sampleEnd);

            this.PlayOgg(isLooping);
        }

        public void LoadOggInfo(string filePath, long? sampleStart, long? sampleEnd)
        {
            if (string.IsNullOrEmpty(filePath) || this.IsInProgress)
            {
                return;
            }

            try
            {
                using (var vorbis = new VorbisReader(filePath))
                {
                    this.oggLoopStart = sampleStart ?? 0;
                    this.oggLoopEnd = sampleEnd ?? vorbis.TotalSamples;

                    var sampleRate = vorbis.SampleRate;
                    var totalSamples = vorbis.TotalSamples;

                    this.OggPlayback?.Invoke(this, new OggPlaybackEventArgs
                    {
                        StateChange = ALSourceState.Initial,
                        SampleRate = sampleRate,
                        TotalSamples = totalSamples
                    });
                }
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException("Invalid .ogg file");
            }
        }

        public void Stop()
        {
            var sources = this.playStates.Keys;
            foreach (var k in sources)
            {
                this.playStates[k] = ALSourceState.Stopped;
            }
        }

        public void OggStop()
        {
            this.Stop();
            this.OggSeek(0);
        }

        public void OggPause()
        {
            this.OggSeek(this.oggProgress);
            this.Stop();
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
        private void PlayWave()
        {
            if (string.IsNullOrEmpty(this.wavFilePath))
            {
                return;
            }

            var buffer = AL.GenBuffer();
            var currentSource = GenSourceWithVolume();

            int channels, bits_per_sample, sample_rate;
            byte[] sound_data;
            try
            {
                sound_data = LoadBytes(File.Open(this.wavFilePath, FileMode.Open), out channels, out bits_per_sample, out sample_rate);
            }
            catch (NotSupportedException)
            {
                throw new InvalidOperationException("Invalid .wav file");
            }
            var soundFormat = GetSoundFormat(channels, bits_per_sample);
            AL.BufferData(buffer, soundFormat, sound_data, sound_data.Length, sample_rate);

            AL.Source(currentSource, ALSourcei.Buffer, buffer);

            this.Play(currentSource, () => { AL.DeleteBuffer(buffer); });
        }

        private void PlayOgg(bool isLooping)
        {
            if (string.IsNullOrEmpty(this.oggFilePath) || this.IsInProgress)
            {
                return;
            }

            lock (this.oggQueueLock) { }
            
            var currentSource = GenSourceWithVolume();

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

            var playbackEnded = false;
            var queueThread = new Thread(() =>
            {
                lock (this.oggQueueLock)
                {
                    for (int i = 0; i < OggBufferCount; i++)
                    {
                        var currentBuffer = AL.GenBuffer();
                        this.QueueBuffer(currentSource, currentBuffer, vorbis, soundFormat, oggSampleRate, valuesPerBuffer, sampleEnd);
                        buffers.Add(currentBuffer);
                    }

                    var allBuffersProcessed = false;
                    while (!buffersInitialized || !playbackEnded)
                    {
                        buffersInitialized = true;

                        int buffersQueued, buffersProcessed;
                        AL.GetSource(currentSource, ALGetSourcei.BuffersQueued, out buffersQueued);
                        AL.GetSource(currentSource, ALGetSourcei.BuffersProcessed, out buffersProcessed);

                        var freedBuffers = buffersProcessed;

                        var newUnqueuedSize = 0;
                        for (int i = 0; i < freedBuffers; i++)
                        {
                            var currentBuffer = buffers.First();
                            buffers.Remove(currentBuffer);

                            int currentBufferSize;
                            AL.GetBuffer(currentBuffer, ALGetBufferi.Size, out currentBufferSize);
                            newUnqueuedSize += currentBufferSize;

                            AL.SourceUnqueueBuffers(currentSource, 1, new[] { currentBuffer });
                            this.QueueBuffer(currentSource, currentBuffer, vorbis, soundFormat, oggSampleRate, valuesPerBuffer, sampleEnd);
                            buffers.Add(currentBuffer);
                        }

                        var bufferProgress = freedBuffers;
                        if (allBuffersProcessed && buffersProcessed != 0)
                        {
                            bufferProgress = buffersProcessed;

                            int currentBufferSize;
                            var unqueuedBuffers = new int[buffersProcessed];
                            AL.SourceUnqueueBuffers(currentSource, buffersProcessed, unqueuedBuffers);
                            foreach (var currentBuffer in unqueuedBuffers)
                            {
                                AL.GetBuffer(currentBuffer, ALGetBufferi.Size, out currentBufferSize);
                                newUnqueuedSize += currentBufferSize;
                            }
                        }

                        var newProgress = newUnqueuedSize / (channels * (bits_per_sample / 8));
                        var adjustedProgress = this.oggProgress + newProgress;
                        this.UpdateOggProgress(adjustedProgress);

                        Thread.Sleep(10);
                        allBuffersProcessed = vorbis.SamplePosition >= this.oggTotalSamples && (!isLooping);
                        if (isLooping)
                        {
                            if (vorbis.SamplePosition >= this.oggLoopEnd)
                            {
                                vorbis.SeekTo(this.oggLoopStart);
                            }
                            if (this.oggProgress >= this.oggLoopEnd)
                            {
                                this.UpdateOggProgress(this.oggLoopStart + (this.oggProgress % this.oggLoopEnd));
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
                playbackEnded = true;
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
            });

        }

        private void QueueBuffer(int source, int buffer, VorbisReader vorbis, ALFormat format, int sampleRate, int targetSamples, long endSample)
        {
            var samples = Math.Min(targetSamples, (int)(endSample - vorbis.SamplePosition) * sizeof(short));
            if (samples <= 0)
            {
                return;
            }

            var floatBuffer = new float[samples];

            if (vorbis.ReadSamples(floatBuffer, 0, samples) <= 0)
            {
                return;
            }

            var shortBuffer = new short[samples];
            for (int i = 0; i < samples; i++)
            {
                var temp = (int)(short.MaxValue * floatBuffer[i]);
                if (temp > short.MaxValue) temp = short.MaxValue;
                else if (temp < short.MinValue) temp = short.MinValue;
                shortBuffer[i] = (short)temp;
            }

            AL.BufferData(buffer, format, shortBuffer, shortBuffer.Length * sizeof(short), sampleRate);

            AL.SourceQueueBuffer(source, buffer);
        }

        private void Play(int source, Action bufferCleanup, Action callback = null)
        {
            var waitThread = new Thread(() =>
            {
                int state;
                AL.SourcePlay(source);
                AL.GetSource(source, ALGetSourcei.SourceState, out state);
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
        private static byte[] LoadBytes(Stream stream, out int channels, out int bits, out int rate)
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
                var optionalSectionType = new string(reader.ReadChars(4));
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
                        reader.ReadBytes(sectionSize);
                        break;
                    case "data":
                        break;
                    default:
                        throw new NotSupportedException("Specified wave file is not supported (nonstandard data header).");
                }

                var dataSize = reader.ReadInt32();

                var bytes = reader.ReadBytes(dataSize);

                channels = numChannels;
                bits = bitsPerSample;
                rate = sampleRate;

                return bytes;
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
        private bool disposedValue = false; // To detect redundant calls

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
