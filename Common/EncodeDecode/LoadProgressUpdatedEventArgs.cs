using System;

namespace Common.EncodeDecode
{
    public class LoadProgressUpdatedEventArgs : EventArgs
    {
        public string UpdateLabel { get; set; }
        public double UpdateProgress { get; set; }

        public LoadProgressUpdatedEventArgs(string label, double progress)
        {
            this.UpdateLabel = label;
            this.UpdateProgress = progress;
        }
    }
}
