using System;

namespace MapEditor.Core
{
    public class Note
    {
        public Note(string noteString)
        {
            try
            {
                var note = noteString[0].ToString();
                var isSharp = noteString[1] == '#';
                var octave = int.Parse(noteString.Substring(isSharp ? 2 : 1));

                var noteOffset = "ABCDEFG".IndexOf(note, StringComparison.InvariantCulture) - 2;
                var noteNumber = 12 * octave + noteOffset;
                if (noteOffset == -3 || noteNumber < 0 || noteNumber > 127)
                {
                    throw new InvalidOperationException("Invalid note");
                }

                this.NoteIndex = noteNumber;
                this.NoteLabel = noteString;
            }
            catch
            {
                this.NoteLabel = null;
            }
        }

        public int NoteIndex { get; }

        public string NoteLabel { get; }
    }
}
