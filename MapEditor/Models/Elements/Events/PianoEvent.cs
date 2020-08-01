using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public class PianoEvent : EventBase
    {
        private string noteKey;
        private int octave;
        private int volume;
        private int frameDuration;

        public string NoteKey
        {
            get
            {
                return this.noteKey;
            }

            set
            {
                this.SetValue(ref this.noteKey, value);
            }
        }

        public int Octave
        {
            get
            {
                return this.octave;
            }

            set
            {
                this.SetValue(ref this.octave, value);
            }
        }

        public int Volume
        {
            get
            {
                return this.volume;
            }

            set
            {
                this.SetValue(ref this.volume, value);
            }
        }

        public int FrameDuration
        {
			get
			{
				return this.frameDuration;
			}

			set
			{
				this.SetValue(ref this.frameDuration, value);
			}
		}

		public override string Info => "Plays a piano key for a specified amount of frames.";

        public override string Name => $"Piano {this.NoteKey}{this.Octave}: {this.FrameDuration} frames ({this.Volume} / 127%)";

        protected override string GetStringValue() => $"piano:{this.NoteKey}{this.Octave}:{this.Volume}:{this.FrameDuration}";

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed piano event \"{value}\".", e => e.Length == 4 && e[0] == "piano"))
            {
                return;
            }

            var newNoteKey = entries[1].TrimEnd("0123456789".ToCharArray());
            this.Validate(newNoteKey, () => this.NoteKey, s => $"Invalid note key \"{s}\".", k => (k.Length == 1 || (k.Length == 2 && k[1] == '#')) && "ABCDEFG".Contains(k[0].ToString()));

            var newOctave = this.ParseIntOrAddError(entries[1].TrimStart("ABCDEFG#".ToCharArray()));
            this.Validate(newOctave, () => this.Octave, i => $"Octave {i} out of range (-2 - 8).", o => o >= -2 && o <= 8);

            var newVolume = this.ParseIntOrAddError(entries[2]);
            this.Validate(newVolume, () => this.Volume, i => $"Volume {i} out of range (-1 - 127).", o => o >= -1 && o <= 127);

            var newFrameDuration = this.ParseIntOrAddError(entries[3]);

            this.NoteKey = newNoteKey;
            this.Octave = newOctave;
            this.volume = newVolume;
            this.FrameDuration = newFrameDuration;
        }
    }
}
