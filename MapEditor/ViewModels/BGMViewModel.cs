using Common.OpenAL;
using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MapEditor.ViewModels
{
    public class BGMViewModel : StringRepresentation
    {
        private string file;
        private string name;
        private long loopStart;
        private long loopEnd;

        private long totalSamples;

        public BGMViewModel()
        {
            this.BgmErrors = new Dictionary<string, string>();
        }

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

        // re-implementation of errors so loopstart/end errors live-update
        // avoiding redoing base class, bad original decisions
        public Dictionary<string, string> BgmErrors { get; }

        public long LoopStart
        {
            get { return this.loopStart; }
            set
            {
                this.SetValueValidate(
                ref this.loopStart,
                value,
                nameof(this.LoopStart),
                $"Loop start out of range (0 - {Math.Min(this.LoopEnd, this.totalSamples)})",
                l => l >= 0 && l <= Math.Min(this.LoopEnd, this.totalSamples));
            }
        }

        public long LoopEnd
        {
            get { return this.loopEnd; }
            set
            {
                this.SetValueValidate(
                ref this.loopEnd,
                value,
                nameof(this.LoopEnd),
                $"Loop end out of range ({Math.Max(0, this.LoopStart)} - {this.totalSamples})",
                l => l >= Math.Max(0, this.LoopStart) && l <= this.totalSamples);
            }
        }

        public string Label => $"{this.File}.ogg ({this.Name})";

        public bool CanSave => !(this.HasErrors || this.BgmErrors.Any(kvp => !string.IsNullOrEmpty(kvp.Value)));

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
            var filePath = Path.Combine("music", newFile + ".ogg");

            this.Validate(newFile, $"Missing bgm file {newFile}", f => System.IO.File.Exists(filePath));

            try
            {
                AudioEngine.LoadOggInfo(filePath, out _, out this.totalSamples);
                this.Validate(newLoopStart, $"Loop start past loop end or .ogg sample range {this.totalSamples}", l => l <= newLoopEnd && l <= this.totalSamples);
                this.Validate(newLoopEnd, $"Loop end before loop start or past .ogg sample range {this.totalSamples}", l => l >= newLoopStart && l <= this.totalSamples);
            }
            catch (InvalidOperationException)
            {
                this.Validate(false, $"Invalid .ogg file \"{filePath}\"", b => b);
            }

            // Still error design issues, ideally would have "properties-to-validate-on", etc. but too much work
            this.LoopStart = newLoopStart;
            this.LoopEnd = newLoopEnd;
            this.LoopStart = newLoopStart;
            this.Name = newName;
            this.File = newFile;

            this.OnPropertyChanged(nameof(this.HasErrors));
        }

        private void SetValueValidate<T>(ref T valueReference, T value, string propertyName, string error, Func<T, bool> validationFunc)
        {
            if (validationFunc(value))
            {
                this.BgmErrors[propertyName] = string.Empty;
            }
            else
            {
                this.BgmErrors[propertyName] = error;
            }

            this.SetValue(ref valueReference, value, propertyName);
            this.OnPropertyChanged(nameof(this.BgmErrors));
            this.OnPropertyChanged(nameof(this.CanSave));
        }
    }
}
