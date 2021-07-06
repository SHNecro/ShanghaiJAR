using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.OpenAL
{
    public class OggData
    {
        public string File { get; set; }
        public string Name { get; set; }
        public long LoopStart { get; set; }
        public long LoopEnd { get; set; }
        public string Label => $"{this.File}.ogg ({this.Name})";
    }
}
