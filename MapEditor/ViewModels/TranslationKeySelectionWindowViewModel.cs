using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class TranslationKeySelectionWindowViewModel : ViewModelBase
    {
        private string filterText;
        private FileNameFilter filterFile;
        private string filterLocale;
        private IEnumerable<KeyValuePair<string, TranslationEntry>> filteredEntries;
        private int page;
        private int pages;
        private KeyValuePair<string, TranslationEntry> currentEntry;
        private KeyValuePair<string, TranslationEntry> selectedEntry;

        public string FilterText
        {
            get
            {
                return this.filterText;
            }

            set
            {
                this.SetValue(ref this.filterText, value);
                this.Page = 1;
                this.FilterEntries();
            }
        }

        public FileNameFilter FilterFile
        {
            get
            {
                return this.filterFile;
            }

            set
            {
                var nullValue = FileNameFilter.AllFilesFilter;
                this.SetValue(ref this.filterFile, value ?? nullValue);

                var previousSelection = this.CurrentEntry;
                this.FilterEntries();

                if (this.FilterFile.IsAllFiles || (previousSelection.Value != null && previousSelection.Value.FilePath == this.FilterFile.Filter))
                {
                    this.SelectKeyEntry(previousSelection.Key, this.FilterFile.IsAllFiles);
                }
            }
        }

        public string FilterLocale
        {
            get
            {
                return this.filterLocale;
            }

            set
            {
                var previousFilterFile = this.FilterFile;
                this.SetValue(ref this.filterLocale, value);
                this.SelectKeyEntry(this.CurrentEntry.Key, previousFilterFile?.IsAllFiles ?? true);
                this.FilterEntries();
                if (previousFilterFile == null || previousFilterFile.IsAllFiles)
                {
                    this.FilterFile = previousFilterFile;
                }
            }
        }

        public IEnumerable<KeyValuePair<string, TranslationEntry>> FilteredEntries
        {
            get
            {
                return this.filteredEntries;
            }

            set
            {
                this.SetValue(ref this.filteredEntries, value);
                this.OnPropertyChanged(nameof(this.PageEntries));
            }
        }

        public IEnumerable<KeyValuePair<string, TranslationEntry>> PageEntries
            => this.FilteredEntries.Skip(300 * (this.Page - 1)).Take(300);

        public int Page
        {
            get
            {
                return this.page;
            }

            set
            {
                this.SetValue(ref this.page, value);
                this.OnPropertyChanged(nameof(this.PageEntries));
            }
        }

        public int Pages
        {
            get
            {
                return this.pages;
            }

            set
            {
                this.SetValue(ref this.pages, value);
            }
        }

        public Action<string> KeySetterAction { get; set; }

        public KeyValuePair<string, TranslationEntry> CurrentEntry
        {
            get
            {
                return this.currentEntry;
            }

            set
            {
                this.SetValue(ref this.currentEntry, value);

                TranslationKeySelectionWindow.ScrollToSelectedEntry();
                this.OnPropertyChanged(nameof(this.CurrentEntry));
            }
        }

        public KeyValuePair<string, TranslationEntry> SelectedEntry
        {
            get
            {
                return this.selectedEntry;
            }

            set
            {
                this.SetValue(ref this.selectedEntry, value);
                this.CurrentEntry = value;
                this.KeySetterAction?.Invoke(value.Key);
            }
        }

        public ICommand PageUpCommand => new RelayCommand((s) => this.Page < this.Pages, (s) => this.Page = Math.Min(this.Page + 1, this.Pages));
        public ICommand PageDownCommand => new RelayCommand((s) => this.Page > 1, (s) => this.Page = Math.Max(this.Page - 1, 1));

        public ICommand NewTranslationCommand => new RelayCommand(
            this.AddNewTranslation    
        );

        public ICommand EditTranslationCommand => new RelayCommand(
            this.CanEditTranslation,
            this.EditTranslation
        );

        public ICommand DeleteTranslationCommand => new RelayCommand(
            this.CanDeleteTranslation,
            this.DeleteTranslation
        );

        public void FilterEntries()
        {
            var previousSelection = this.CurrentEntry;
            var fileFiltered = (this.FilterFile == null || this.FilterFile.IsAllFiles || this.FilterFile.Filter == null
                ? Constants.TranslationService.LanguageEntries.Where(kvp => kvp.Key.Item2 == this.FilterLocale)
                : Constants.TranslationService.LanguageEntries.Where(kvp => kvp.Value.FilePath.ToLowerInvariant().Contains(this.FilterFile.Filter.ToLowerInvariant()) && kvp.Key.Item2 == this.FilterLocale))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            var filtered = (this.FilterText == null
                ? fileFiltered
                : fileFiltered.Where(kvp => kvp.Key.Item1.ToLowerInvariant().Contains(this.FilterText.ToLowerInvariant()) || kvp.Value.Dialogue.Text.ToLowerInvariant().Contains(this.FilterText.ToLowerInvariant())))
                .Select(kvp => new KeyValuePair<string, TranslationEntry>(kvp.Key.Item1, kvp.Value)).ToDictionary(kv => kv.Key, kv => kv.Value);
            this.Pages = (int)Math.Ceiling(filtered.Count() / 300.0);

            this.FilteredEntries = filtered;

            if (previousSelection.Key != null && filtered.ContainsKey(previousSelection.Key))
            {
                this.CurrentEntry = new KeyValuePair<string, TranslationEntry>(previousSelection.Key, filtered[previousSelection.Key]);
            }
            else
            {
                this.Page = 1;
            }
        }

        public void SelectKeyEntry(string initialKey, bool isAllFiles)
        {
            var matchingKeys = Constants.TranslationService.LanguageEntries.Where(kvp => kvp.Key.Item1 == initialKey && kvp.Key.Item2 == this.FilterLocale)
                .Select(kvp => new KeyValuePair<string, TranslationEntry>(kvp.Key.Item1, kvp.Value));
            var initialKeyFile = matchingKeys.Any() ? matchingKeys.First().Value.FilePathShort : null;

            if (initialKeyFile == null && this.FilteredEntries != null && this.FilteredEntries.Any())
            {
                this.Page = 1;
                this.CurrentEntry = this.PageEntries.FirstOrDefault();
                return;
            }
            else if (isAllFiles || initialKey == null)
            {
                if (this.FilterFile == null || !this.FilterFile.IsAllFiles)
                {
                    this.FilterFile = FileNameFilter.AllFilesFilter;
                }
            }
            else
            {
                if (this.FilterFile == null || this.FilterFile.Filter != initialKeyFile)
                {
                    this.FilterFile = new FileNameFilter { IsAllFiles = false, Filter = initialKeyFile };
                }
            }

            var entry = matchingKeys.FirstOrDefault();
            this.Page = 1 + (Array.IndexOf(this.FilteredEntries.ToArray(), entry) / 300);
            this.CurrentEntry = entry;
        }

        private void AddNewTranslation(object obj)
        {
            AddEditTranslationWindow.ShowWindow(null, this.FilterLocale, this.FilterFile.IsAllFiles ? this.PageEntries?.FirstOrDefault().Value?.FilePath : this.FilterFile.Filter);
        }

        private void EditTranslation(object obj)
        {
            AddEditTranslationWindow.ShowWindow(this.CurrentEntry.Key, this.FilterLocale, this.FilterFile.IsAllFiles ? null : this.FilterFile.Filter);
        }

        private bool CanEditTranslation(object obj)
        {
            return this.CurrentEntry.Key != null;
        }

        private void DeleteTranslation(object obj)
        {

            var skipConfirm = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            var deleteConfirm = skipConfirm ? MessageBoxResult.Yes : MessageBox.Show($"Are you sure you want to delete this item?{Environment.NewLine}All uses of this string will become invalid!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (deleteConfirm == MessageBoxResult.Yes)
            {
                var keyToDelete = this.CurrentEntry.Key;
                // TODO: Fix datagrid not updating selection even when CurrentEntry updated
                //var nextEntry = this.FilteredEntries.SkipWhile(e => e.Key != keyToDelete).Skip(1).FirstOrDefault();
                //var previousEntry = this.FilteredEntries.TakeWhile(e => e.Key != keyToDelete).LastOrDefault();

                Constants.TranslationService.DeleteAllTranslations(keyToDelete);

                //this.CurrentEntry = nextEntry.Key != null ? nextEntry : previousEntry;
            }
        }

        private bool CanDeleteTranslation(object obj)
        {
            return this.CurrentEntry.Key != null;
        }
    }
}
