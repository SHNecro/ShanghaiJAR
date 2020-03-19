using System;

namespace NSShanghaiEXE.InputOutput.Audio
{
    public class Note
    {
        public static readonly string[] OctaveNotes = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        public Note(string noteString)
        {
            var note = noteString[0].ToString();
            var isSharp = noteString[1] == '#';
            var octave = int.Parse(noteString.Substring(isSharp ? 2 : 1)) + 2;

            var noteOffset = "C D EF G A B".IndexOf(note, StringComparison.InvariantCulture) + (isSharp ? 1 : 0);
            var noteNumber = 12 * octave + noteOffset;
            if (noteNumber < 0 || noteNumber > 127)
            {
                throw new InvalidOperationException("Invalid note");
            }

            this.NoteIndex = noteNumber;
            this.NoteLabel = noteString;
        }

        public int NoteIndex { get; }

        public string NoteLabel { get; }
    }
}
