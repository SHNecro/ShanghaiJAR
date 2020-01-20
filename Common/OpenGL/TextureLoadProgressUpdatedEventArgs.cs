using System;

namespace Common.OpenGL
{
    public class TextureLoadProgressUpdatedEventArgs : EventArgs
    {
        public string UpdateLabel { get; set; }
        public double UpdateProgress { get; set; }

        public TextureLoadProgressUpdatedEventArgs(string label, double progress)
        {
            this.UpdateLabel = label;
            this.UpdateProgress = progress;
        }
    }
}
