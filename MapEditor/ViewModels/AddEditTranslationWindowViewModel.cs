using Common;
using ExtensionMethods;
using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class AddEditTranslationWindowViewModel : ViewModelBase
    {
        private string initialKey;
        private string key;
        private bool isEditingKey;
        private bool isDialogue;
        private bool isFormattingText;
        private IEnumerable<TranslationEntryViewModel> translationEntries;

        private bool entriesUpdating;
        private Func<string, string> defaultLocationCreator;

        public AddEditTranslationWindowViewModel()
        {
            this.isFormattingText = true;
        }

        public string InitialKey
        {
            get
            {
                return this.initialKey;
            }
            set
            {
                this.SetValue(ref this.initialKey, value);
                this.OnPropertyChanged(nameof(this.IsAddingNewKey));
                this.OnPropertyChanged(nameof(this.Title));
                if (value == null)
                {
                    this.TranslationEntries = Constants.TranslationService.Locales.Select((l, i) => 
                    {
                    return new TranslationEntryViewModel
					{
						Key = "Debug.UnimplementedText",
						Locale = l,
                        TranslationEntry = new TranslationEntry
                        {
                            FilePath = this.defaultLocationCreator(l),
                            Dialogue = new Common.Dialogue
                            {
                                Face = Common.FACE.Sprite.ToFaceId(false),
                                Text = i == 0 ? Constants.TranslationService.LanguageEntries[Tuple.Create("Debug.UnimplementedText", l)].Dialogue.Text : null
                            }
                        },
                        BaseGetterAction = () => this.FirstEnteredTranslation
                    };
                    }).ToList();
                }
                else
                {
                    this.TranslationEntries = Constants.TranslationService.LanguageEntries
                       .Where(kvp => kvp.Key.Item1 == this.InitialKey)
                       .Select(kvp =>
                       {
                           return new TranslationEntryViewModel
                           {
                               Key = kvp.Key.Item1,
                               Locale = kvp.Key.Item2,
                               TranslationEntry = new TranslationEntry
                               {
                                   FilePath = kvp.Value.FilePath,
                                   Dialogue = new Common.Dialogue
                                   {
                                       Face = kvp.Value.Dialogue.Face,
                                       Text = kvp.Value.Dialogue.Text
                                   }
                               },
                               BaseGetterAction = () => this.FirstEnteredTranslation
                           };
                       }).ToList();
                }
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.SetValue(ref this.key, value);
            }
        }

        public bool IsEditingKey
        {
            get
            {
                return this.isEditingKey;
            }
            set
            {
                this.SetValue(ref this.isEditingKey, value);
            }
        }

        public bool IsDialogue
        {
            get
            {
                return this.isDialogue;
            }
            set
            {
                this.SetValue(ref this.isDialogue, value);
            }
        }

        public bool IsFormattingText
        {
            get
            {
                return this.isFormattingText;
            }
            set
            {
                this.SetValue(ref this.isFormattingText, value);
            }
        }

        public IEnumerable<TranslationEntryViewModel> TranslationEntries
        {
            get
            {
                return this.translationEntries;
            }
            set
            {
                if (this.translationEntries != null)
                {
                    foreach (var te in value)
                    {
                        te.PropertyChanged -= this.UpdateEntry;
                    }
                }

                this.SetValue(ref this.translationEntries, value);
                foreach (var te in value)
                {
                    te.PropertyChanged += this.UpdateEntry;
                }

                this.OnPropertyChanged(nameof(this.FirstEnteredTranslation));
                this.CurrentEntry = this.TranslationEntries.Select(tevm => tevm.TranslationEntry).FirstOrDefault();
            }
        }

        public TranslationEntry CurrentEntry
        {
            get
            {
                return AddEditTranslationRenderer.CurrentEntry;
            }

            set
            {
                AddEditTranslationRenderer.CurrentEntry = value;
                this.OnPropertyChanged(nameof(this.CurrentEntry));
            }
        }

        private void UpdateEntry(object sender, EventArgs args)
        {
            if (!this.entriesUpdating)
            {
                this.entriesUpdating = true;
                foreach (var te in this.translationEntries)
                {
                    te.UpdateProperties();
                }
                this.entriesUpdating = false;
                this.OnPropertyChanged(nameof(this.FirstEnteredTranslation));
            }
        }

        public TranslationEntryViewModel FirstEnteredTranslation => this.TranslationEntries.FirstOrDefault(te => te.TranslationEntry.Dialogue.Text != null);

        public bool IsAddingNewKey => this.InitialKey == null;

        public string Title => this.IsAddingNewKey ? "Add String" : "Edit String";

        public ICommand ApplyChangesCommand => new RelayCommand(this.ApplyChanges);

        public void Initialize(string newInitialKey, string initialLocale, string initialFile)
        {
            if (initialLocale == null || initialFile == null)
            {
                this.defaultLocationCreator = (locale) => $"language/{locale}/Misc.xml";
            }
            else
            {
                this.defaultLocationCreator = (locale) => initialFile.ReplaceFirst(initialLocale, locale);
            }

            this.InitialKey = newInitialKey;
            this.Key = newInitialKey;
            this.IsDialogue = newInitialKey == null ? true : Constants.TranslationService.IsKeyDialogue(newInitialKey);
            this.IsEditingKey = true;
        }

        private void ApplyChanges()
        {
            foreach (var tevm in this.TranslationEntries)
            {
                var newDialogue = tevm.Text != null
                    ? tevm.Dialogue
                    : this.FirstEnteredTranslation != null
                        ? new Dialogue { Text = $"[{this.FirstEnteredTranslation.Dialogue.Text}]", Face = this.FirstEnteredTranslation.FaceId }
                        : new Dialogue { Text = "", Face = FACE.Sprite.ToFaceId(false) };
                tevm.Dialogue = newDialogue;

                if (!this.IsAddingNewKey && this.IsEditingKey && this.InitialKey != this.Key)
                {
                    Constants.TranslationService.DeleteTranslation(this.InitialKey, tevm.Locale, false);
                }

                if (Constants.TranslationService.LanguageEntries.ContainsKey(Tuple.Create(this.Key, tevm.Locale)))
                {
                    Constants.TranslationService.EditTranslation(this.Key, tevm.Locale, newDialogue, this.IsDialogue, false);
                }
                else
                {
                    Constants.TranslationService.AddTranslation(this.Key, tevm.TranslationEntry, this.IsDialogue, false);
                }
            }

            Constants.TranslationService.ReloadTranslationKeys();
            this.InitialKey = this.Key;
        }
    }
}
