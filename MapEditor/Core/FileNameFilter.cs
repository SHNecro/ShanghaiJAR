namespace MapEditor.Core
{
    public class FileNameFilter : ViewModelBase
    {
        private bool isAllFiles;
        private string filter;

        public static FileNameFilter AllFilesFilter => new FileNameFilter { IsAllFiles = true };

        public bool IsAllFiles
        {
            get { return this.isAllFiles; }
            set { this.SetValue(ref this.isAllFiles, value); }
        }

        public string Filter
        {
            get { return this.IsAllFiles ? "All Files" : this.filter; }
            set { this.SetValue(ref this.filter, value); }
        }
    }
}
