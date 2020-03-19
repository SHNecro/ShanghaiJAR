using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSShanghaiEXE.InputOutput.Audio.MIDI
{
    public class PianoNotePlayer : IDisposable
    {
        private OutputDevice outputDevice;
        // Note: <StartTime, Duration>
        private Dictionary<int, Tuple<int, int>> playingNotes;

        private int currentTick;

        private bool disposedValue = false; // To detect redundant calls

        public PianoNotePlayer()
        {
            this.outputDevice = new OutputDevice(0);
            this.playingNotes = new Dictionary<int, Tuple<int, int>>();
        }

        public void PlayNote(Note note, int volume, int tickDuration)
        {
            if (playingNotes.ContainsKey(note.NoteIndex) && tickDuration == -1 && playingNotes[note.NoteIndex].Item2 == -1)
            {
                return;
            }
            if (tickDuration == 0)
            {
                outputDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, note.NoteIndex, 0));
                if (playingNotes.ContainsKey(note.NoteIndex))
                {
                    playingNotes.Remove(note.NoteIndex);
                }
            }
            else
            {
                outputDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, note.NoteIndex, volume));
                playingNotes[note.NoteIndex] = Tuple.Create(this.currentTick, tickDuration);
            }
        }

        public void UpdateNoteTick()
        {
            var expiredNotes = new List<int>();

            foreach (var kvp in this.playingNotes)
            {
                var isSustained = kvp.Value.Item2 == -1;

                var noteIndex = kvp.Key;
                var startTick = kvp.Value.Item1;
                var duration = kvp.Value.Item2;

                if (!isSustained && this.currentTick >= startTick + duration)
                {
                    outputDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, noteIndex, 0));
                    expiredNotes.Add(noteIndex);
                }
            }

            foreach (var note in expiredNotes)
            {
                this.playingNotes.Remove(note);
            }

            if (this.IsPlayingNote)
            {
                this.currentTick++;
            }
            else
            {
                this.currentTick = 0;
            }
        }

        public bool IsPlayingNote => this.playingNotes.Any(n => n.Value.Item2 != -1);

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.outputDevice.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
