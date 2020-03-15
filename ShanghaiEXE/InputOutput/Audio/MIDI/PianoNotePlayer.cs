using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;

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
            outputDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, note.NoteIndex, volume));
            playingNotes[note.NoteIndex] = Tuple.Create(this.currentTick, tickDuration);
        }

        public void UpdateNoteTick()
        {
            var expiredNotes = new List<int>();

            foreach (var kvp in this.playingNotes)
            {
                var noteIndex = kvp.Key;
                var startTick = kvp.Value.Item1;
                var duration = kvp.Value.Item2;

                if (this.currentTick >= startTick + duration)
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

        public bool IsPlayingNote => this.playingNotes.Count != 0;

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
