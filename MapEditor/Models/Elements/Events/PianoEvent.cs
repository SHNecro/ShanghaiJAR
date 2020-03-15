using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public class PianoEvent : EventBase
    {
        private Note note;
        private int frameDuration;

        public string NoteString
        {
            get
            {
                return this.note?.NoteLabel;
            }

            set
            {
                this.Note = new Note(value);
            }
        }

        public Note Note
		{
			get
			{
				return this.note;
			}

			set
			{
				this.SetValue(ref this.note, value);
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

        public override string Name => $"Piano {this.Note.NoteLabel}: {this.FrameDuration} frames";

        protected override string GetStringValue() => $"piano:{this.Note.NoteLabel}:{this.FrameDuration}";

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed piano event \"{value}\".", e => e.Length == 3 && e[0] == "piano"))
            {
                return;
            }

			var newNote = new Note(entries[1]);
            this.Validate(newNote, "Piano key does not exist.", n => !string.IsNullOrEmpty(newNote.NoteLabel));
			var newFrameDuration = this.ParseIntOrAddError(entries[2]);

			if (!this.HasErrors)
            {
                this.Note = newNote;
                this.FrameDuration = newFrameDuration;
            }
        }
    }
}
