using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for FileSelector.xaml
    /// </summary>
    public partial class FileSelector : UserControl
    {
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(FileSelector), new PropertyMetadata(null));

        public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register("InitialDirectory", typeof(string), typeof(FileSelector), new PropertyMetadata(null));

        public FileSelector()
        {
            this.Filters = new ObservableCollection<FileSelectorFilter>();

            InitializeComponent();
        }

        public string FilePath
        {
            get { return (string)this.GetValue(FilePathProperty); }
            set { this.SetValue(FilePathProperty, value); }
        }

        public string Title { get; set; }

        public string InitialDirectory
        {
            get { return (string)this.GetValue(InitialDirectoryProperty); }
            set { this.SetValue(InitialDirectoryProperty, value); }
        }

        public ObservableCollection<FileSelectorFilter> Filters { get; set; }

        public bool IsFolderPicker { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectFileDialog = new CommonOpenFileDialog
            {
                EnsureFileExists = true,
                Multiselect = false,
                DefaultDirectory = this.InitialDirectory ?? Directory.GetCurrentDirectory(),
                Title = this.Title,
                InitialDirectory = this.InitialDirectory ?? Directory.GetCurrentDirectory(),
                IsFolderPicker = this.IsFolderPicker
            };

            for (var i = 0; i < this.Filters.Count; i++)
            {
                selectFileDialog.Filters.Add(new CommonFileDialogFilter(this.Filters[i].DisplayName, this.Filters[i].Extensions));
            }

            var dialogSuccess = selectFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                var baseDirectory = new DirectoryInfo(this.InitialDirectory ?? Directory.GetCurrentDirectory()).FullName + @"\";
                var sharedPath = new string(selectFileDialog.FileName.ToCharArray().TakeWhile((c, i) => i < baseDirectory.Length && c == baseDirectory[i]).ToArray<char>());
                var backtrackDirectories = baseDirectory.Length > sharedPath.Length ? baseDirectory.Substring(sharedPath.Length).Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Length : 0;
                var backtrack = (string.Concat(Enumerable.Repeat(@"..\", backtrackDirectories)));

                this.FilePath = backtrack + selectFileDialog.FileName.Substring(sharedPath.Length);
            }
            this.FilePathBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}
