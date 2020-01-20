using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Views
{
    /// <summary>
    /// Interaction logic for AddEditTranslationWindowView.xaml
    /// </summary>
    public partial class AddEditTranslationWindowView : Window
    {
        public AddEditTranslationWindowView()
        {
            InitializeComponent();
        }
        
        private void SetFilePath(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var assemblyPath = Assembly.GetEntryAssembly().Location;
            var binding = button.GetBindingExpression(Button.TagProperty);
            var boundProperty = binding.DataItem.GetType().GetProperty(binding.ResolvedSourcePropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);


            var saveFileDialog = new CommonSaveFileDialog
            {
                RestoreDirectory = false,
                DefaultExtension = "xml",
                OverwritePrompt = false,
                NavigateToShortcut = true,
                InitialDirectory = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(assemblyPath), (string)(button.Tag))),
                Title = "Set String Location"
            };
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("Language File", "*.xml"));
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

            var dialogSuccess = saveFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
                var relativePath = AddEditTranslationWindowView.MakeRelativePath(assemblyPath, saveFileDialog.FileName);
                boundProperty.SetValue(binding.DataItem, relativePath);
                binding.UpdateSource();
            }
        }

        private static string MakeRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException(nameof(fromPath));
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException(nameof(toPath));

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            /*
            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }
            */
            return relativePath;
        }
    }
}
