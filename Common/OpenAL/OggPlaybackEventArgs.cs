using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.OpenAL
{
    public class OggPlaybackEventArgs : EventArgs
    {
        private const string TimeStringFormat = @"mm\:ss\.ff";

        public ALSourceState? StateChange { get; set; }

        public long SampleRate { get; set; }
        public long TotalSamples { get; set; }
        public long ProgressSamples { get; set; }

        public double Percent => (double)this.ProgressSamples / this.TotalSamples;

        public TimeSpan TotalTime => SamplesToTime(this.TotalSamples, this.SampleRate);
        public TimeSpan ProgressTime => SamplesToTime(this.ProgressSamples, this.SampleRate);

        public string TotalTimeString => this.TotalTime.ToString(TimeStringFormat);
        public string ProgressTimeString => this.ProgressTime.ToString(TimeStringFormat);

        private static TimeSpan SamplesToTime(long samples, long sampleRate) => TimeSpan.FromSeconds((double)samples / sampleRate);
    }
}
