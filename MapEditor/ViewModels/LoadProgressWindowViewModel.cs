using MapEditor.Core;

namespace MapEditor.ViewModels
{
    public class LoadProgressWindowViewModel : ViewModelBase
    {
        private double progress;
        private string progressLabel;

        public double Progress
        {
            get { return this.progress; }
            set { this.SetValue(ref this.progress, value); }
        }

        public string ProgressLabel
        {
            get { return this.progressLabel; }
            set { this.SetValue(ref this.progressLabel, value); }
        }
    }
}
