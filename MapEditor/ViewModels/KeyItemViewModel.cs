using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Models.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class KeyItemViewModel : StringRepresentation
    {
        private int index;
        private string nameKey;
        private Wrapper<string> selectedDialogue;

        private string initialStringValue;

        public KeyItemViewModel()
        {
            this.index = 0;
            this.nameKey = "Debug.UnimplementedText";
            this.DialogueKeys = new ObservableCollection<Wrapper<string>>();
            this.DialogueKeys.CollectionChanged += CollectionChanged;
            this.RegisterDialogueKeys();

            this.initialStringValue = string.Empty;
        }

        public KeyItemViewModel(int index, KeyItemDefinition keyItemDefinition)
        {
            this.index = index;
            this.nameKey = keyItemDefinition.NameKey;
            this.DialogueKeys = new ObservableCollection<Wrapper<string>>(keyItemDefinition.DialogueKeys.Select(k => k.Wrap()) ?? new List<Wrapper<string>>());
            this.DialogueKeys.CollectionChanged += CollectionChanged;
            this.RegisterDialogueKeys();

            this.initialStringValue = this.StringValue;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Refresh();
        }

        public int Index
        {
            get
            {
                return this.index;
            }

            set
            {
                this.SetValue(ref this.index, value);
                this.Refresh();
            }
        }

        public string NameKey
        {
            get
            {
                return this.nameKey;
            }

            set
            {
                this.SetValue(ref this.nameKey, value);
                this.Refresh();
            }
        }

        public ObservableCollection<Wrapper<string>> DialogueKeys { get; }

        public Wrapper<string> SelectedDialogue
        {
            get { return this.selectedDialogue; }
            set { this.SetValue(ref this.selectedDialogue, value); }
        }

        public string Name => Constants.TranslationService.Translate(this.NameKey).Text;

        public bool IsDirty => TrimIndex(this.initialStringValue) != TrimIndex(this.StringValue);

        public string IndexLabel => $"{this.Index}{(this.IsDirty ? "*" : string.Empty)}";

        public ICommand UndoCommand => new RelayCommand(this.Undo);

        public void Save()
        {
            this.initialStringValue = this.StringValue;
            this.Refresh();
        }

        public void Undo()
        {
            this.UnregisterDialogueKeys();
            var realIndex = this.Index;
            this.StringValue = this.initialStringValue;
            this.Index = realIndex;
            this.RegisterDialogueKeys();
            this.Refresh();
        }

        protected override string GetStringValue()
        {
            var stringValueBuilder = new StringBuilder();
            stringValueBuilder.AppendLine($"<KeyItem Index=\"{this.Index}\" Name=\"{this.NameKey}\">");
            foreach (var dialogueKey in this.DialogueKeys)
            {
                stringValueBuilder.AppendLine($"  <Dialogue Key=\"{dialogueKey.Value}\" />");
            }
            stringValueBuilder.AppendLine("</KeyItem>");

            return stringValueBuilder.ToString();
        }

        protected override void SetStringValue(string value)
        {
            var lines = value.Trim().Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            var newIndex = 0;
            var newNameKey = string.Empty;
            var newDialogueKeys = new List<string>();

            var openTagMatch = default(Match);
            this.Validate(lines, "Empty key item", l => l.Length > 0);
            if (this.HasErrors) return;
            this.Validate(lines[0], "Malformed key item open", l => (openTagMatch = Regex.Match(l, @"<KeyItem Index=""(\d+)"" Name=""([^""]+)"">")).Success);
            newIndex = int.Parse(openTagMatch.Groups[1].Value);
            newNameKey = openTagMatch.Groups[2].Value;
            if (this.HasErrors) return;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (i == 0)
                {
                    continue;
                }

                if (i == lines.Length - 1)
                {
                    this.Validate(line, "Malformed key item close", l => Regex.IsMatch(l, @"</KeyItem>"));
                    if (this.HasErrors) return;
                    continue;
                }

                var dialogueMatch = default(Match);
                this.Validate(line, "Malformed dialogue tag", l => (dialogueMatch = Regex.Match(l, @"  <Dialogue Key=""([^""]+)"" />")).Success);
                if (this.HasErrors) return;
                newDialogueKeys.Add(dialogueMatch.Groups[1].Value);
            }

            this.Index = newIndex;
            this.NameKey = newNameKey;
            this.DialogueKeys.Clear();
            foreach (var k in newDialogueKeys)
            {
                this.DialogueKeys.Add(k.Wrap());
            }

            if (string.IsNullOrEmpty(this.initialStringValue))
            {
                this.initialStringValue = this.StringValue;
            }

            this.Refresh();
        }

        private void RegisterDialogueKeys()
        {
            foreach (var k in this.DialogueKeys)
            {
                k.PropertyChanged += this.DialogueKeyChanged;
            }
        }

        private void UnregisterDialogueKeys()
        {
            foreach (var k in this.DialogueKeys)
            {
                k.PropertyChanged -= this.DialogueKeyChanged;
            }
        }

        private void DialogueKeyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Refresh();
        }

        private void Refresh()
        {
            this.OnPropertyChanged(nameof(this.IsDirty));
            this.OnPropertyChanged(nameof(this.IndexLabel));
            this.OnPropertyChanged(nameof(this.Name));
        }

        private static string TrimIndex(string s)
        {
            var index = s?.IndexOf("Name=\"", StringComparison.InvariantCulture) ?? -1;

            return index == -1 ? s : s.Substring(index);
        }
    }
}
