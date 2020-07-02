using MapEditor.Core;
using System.IO;

namespace MapEditor.ViewModels
{
    public class BGMViewModel : StringRepresentation
    {
        private string file;
        private string name;
        private int loopStart;
        private int loopEnd;

        public string File
        {
            get
            {
                return this.file;
            }

            set
            {
                this.SetValue(ref this.file, value);
                this.OnPropertyChanged(nameof(this.Label));
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.SetValue(ref this.name, value);
                this.OnPropertyChanged(nameof(this.Label));
            }
        }

        public int LoopStart
        {
            get { return this.loopStart; }
            set { this.SetValue(ref this.loopStart, value); }
        }

        public int LoopEnd
        {
            get { return this.loopEnd; }
            set { this.SetValue(ref this.loopEnd, value); }
        }

        public string Label => $"{this.File}.ogg ({this.Name})";

        protected override string GetStringValue()
        {
            return $"{this.LoopStart},{this.LoopEnd},{this.Name},{this.File}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(',');
            if (!this.Validate(entries, "Invalid number of parameters.", e => e.Length == 4))
            {
                return;
            }

            var newLoopStart = this.ParseIntOrAddError(entries[0], i => i >= 0, (i) => "Loop start must be >= 0");
            var newLoopEnd = this.ParseIntOrAddError(entries[1], i => i >= 0, (i) => "Loop end must be >= 0");
            var newName = entries[2];
            var newFile = entries[3];

            this.Validate(newFile, $"Missing bgm file {newFile}", f => System.IO.File.Exists(Path.Combine("music", newFile + ".ogg")));

            //if (!this.HasErrors)
            //{
                this.LoopStart = newLoopStart;
                this.LoopEnd = newLoopEnd;
                this.Name = newName;
                this.File = newFile;
            //}
        }
    }
}
