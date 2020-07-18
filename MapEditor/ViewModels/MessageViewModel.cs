using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class MessageViewModel : StringRepresentation
    {
        private int index;
        private Wrapper<string> selectedDialogue;

        private string initialStringValue;

        public MessageViewModel()
        {
            this.Index = 0;
            this.DialogueKeys = new ObservableCollection<Wrapper<string>>();
            this.DialogueKeys.CollectionChanged += CollectionChanged;
            this.RegisterDialogueKeys();

            this.initialStringValue = string.Empty;
        }

        public MessageViewModel(int index, IList<Wrapper<string>> dialogueKeys)
        {
            this.Index = index;
            this.DialogueKeys = new ObservableCollection<Wrapper<string>>(dialogueKeys ?? new List<Wrapper<string>>());
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

        public ObservableCollection<Wrapper<string>> DialogueKeys { get; }

        public Wrapper<string> SelectedDialogue
        {
            get { return this.selectedDialogue; }
            set { this.SetValue(ref this.selectedDialogue, value); }
        }

        public string IndexLabel => $"{this.Index}{(this.IsDirty ? "*" : string.Empty)}";

        public string Summary => string.Join(Environment.NewLine, this.DialogueKeys.Select(k =>
        {
            if (!Constants.TranslationService.CanTranslate(k.Value))
            {
                return "INVALID KEY";
            }
            var dialogue = Constants.TranslationService.Translate(k.Value);
            return $"{dialogue.Face.ToString()}: {dialogue.Text}";
        }));

        public bool IsDirty => this.initialStringValue != this.StringValue;

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand UndoCommand => new RelayCommand(this.Undo);

        protected override string GetStringValue()
        {
            var stringValueBuilder = new StringBuilder();
            stringValueBuilder.AppendLine($"<Message Index=\"{this.Index}\">");
            foreach (var dialogueKey in this.DialogueKeys)
            {
                stringValueBuilder.AppendLine($"  <Dialogue Key=\"{dialogueKey.Value}\" />");
            }
            stringValueBuilder.AppendLine("</Message>");

            return stringValueBuilder.ToString();
        }

        protected override void SetStringValue(string value)
        {
            var lines = value.Trim().Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            var newIndex = 0;
            var newDialogueKeys = new List<string>();

            var indexMatch = default(Match);
            this.Validate(lines, "Empty message", l => l.Length > 0);
            if (this.HasErrors) return;
            this.Validate(lines[0], "Malformed message open", l => (indexMatch = Regex.Match(l, @"<Message Index=""(\d+)"">")).Success);
            newIndex = int.Parse(indexMatch.Groups[1].Value);
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
                    this.Validate(line, "Malformed message close", l => Regex.IsMatch(l, @"</Message>"));
                    if (this.HasErrors) return;
                    continue;
                }

                var dialogueMatch = default(Match);
                this.Validate(line, "Malformed dialogue tag", l => (dialogueMatch = Regex.Match(l, @"  <Dialogue Key=""([^""]+)"" />")).Success);
                if (this.HasErrors) return;
                newDialogueKeys.Add(dialogueMatch.Groups[1].Value);
            }

            this.Index = newIndex;
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

        private void Save()
        {
            this.initialStringValue = this.StringValue;
            this.Refresh();
        }

        private void Undo()
        {
            this.UnregisterDialogueKeys();
            this.DialogueKeys.Clear();
            var realIndex = this.Index;
            this.StringValue = this.initialStringValue;
            this.Index = realIndex;
            this.RegisterDialogueKeys();
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
            this.OnPropertyChanged(nameof(this.Summary));
        }
    }
}
