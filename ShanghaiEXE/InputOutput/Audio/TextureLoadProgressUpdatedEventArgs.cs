using System;

namespace NSShanghaiEXE.InputOutput.Audio
{
    public class AudioLoadProgressUpdatedEventArgs : EventArgs
    {
        public string UpdateLabel { get; set; }
        public double UpdateProgress { get; set; }

        public AudioLoadProgressUpdatedEventArgs(string label, double progress)
        {
            this.UpdateLabel = label;
            this.UpdateProgress = progress;
        }
    }
}
