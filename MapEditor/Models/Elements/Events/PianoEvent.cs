using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public class PianoEvent : EventBase
    {
        private string noteKey;
        private int octave;
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

        public override string Name => $"Piano {this.NoteKey}{this.Octave}: {this.FrameDuration} frames";

        protected override string GetStringValue() => $"piano:{this.NoteKey}{this.Octave}:{this.FrameDuration}";

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed piano event \"{value}\".", e => e.Length == 3 && e[0] == "piano"))
            {
                return;
            }

            var newNoteKey = entries[1].TrimEnd("0123456789".ToCharArray());
            this.Validate(newNoteKey, "Invalid note.", k => (k.Length == 1 || (k.Length == 2 && k[1] == '#')) && "ABCDEFG".Contains(k[0].ToString()));

            var newOctave = this.ParseIntOrAddError(entries[1].TrimStart("ABCDEFG#".ToCharArray()));
            this.Validate(newOctave, "Octave out of range.", o => o >= -2 && o <= 8);

            var newFrameDuration = this.ParseIntOrAddError(entries[2]);

			if (!this.HasErrors)
            {
                this.NoteKey = newNoteKey;
                this.Octave = newOctave;
                this.FrameDuration = newFrameDuration;
            }
        }
    }
}
