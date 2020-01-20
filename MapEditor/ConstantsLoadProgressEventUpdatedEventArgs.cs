namespace MapEditor
{
    public class ConstantsLoadProgressEventUpdatedEventArgs
    {
        public string UpdateLabel { get; set; }
        public double UpdateProgress { get; set; }

        public ConstantsLoadProgressEventUpdatedEventArgs(string label, double progress)
        {
            this.UpdateLabel = label;
            this.UpdateProgress = progress;
        }
    }
}