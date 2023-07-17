using MapEditor.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var binding = button.GetBindingExpression(Button.TagProperty);
            var boundProperty = binding.DataItem.GetType().GetProperty(binding.ResolvedSourcePropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);


            var saveFileDialog = new CommonSaveFileDialog
            {
                RestoreDirectory = false,
                DefaultExtension = "xml",
                OverwritePrompt = false,
                NavigateToShortcut = true,
                InitialDirectory = Path.GetDirectoryName(Path.Combine(assemblyPath, (string)(button.Tag))),
                Title = "Set String Location"
            };
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("Language File", "*.xml"));
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

            var dialogSuccess = saveFileDialog.ShowDialog();
            if (dialogSuccess == CommonFileDialogResult.Ok)
            {
				var dataItem = ((TranslationEntryViewModel)binding.DataItem);
				var origPath = Path.Combine(assemblyPath, dataItem.FilePathShort);
				dataItem.SetFilePath(saveFileDialog.FileName);
                Constants.TranslationService.LanguageEntries[Tuple.Create(dataItem.Locale, dataItem.Key)].FilePath = saveFileDialog.FileName;

				binding.UpdateSource();

                var origContents = File.ReadAllLines(origPath);
                var removedLines = origContents.Where(l => l.Contains($"Key=\"{dataItem.Key}\"")).ToArray();
                File.WriteAllLines(origPath, origContents.Except(removedLines));

				var newContents = File.ReadAllLines(saveFileDialog.FileName);
                var newUpToData = new List<string>();
                var linesAdded = false;
                foreach (var line in newContents)
                {
                    if (!linesAdded && line.Contains("</data>"))
                    {
                        newUpToData.AddRange(removedLines);
                        linesAdded = true;
					}
					newUpToData.Add(line);
				}
				File.WriteAllLines(saveFileDialog.FileName, newUpToData);
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
