using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.OpenAL
{
    public class WAVData
    {
        public byte[] Data { get; set; }
        public int Channels { get; set; }
        public int Bits { get; set; }
        public int Rate { get; set; }

        public ALFormat SoundFormat
        {
            get
            {
                switch (this.Channels)
                {
                    case 1: return this.Bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                    case 2: return this.Bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                    default: throw new NotSupportedException("The specified sound format is not supported.");
                }
            }
        }
    }
}
