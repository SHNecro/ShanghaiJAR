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
    public class MailItemViewModel : StringRepresentation
    {
        private int index;
        private string senderKey;
        private string subjectKey;
        private Wrapper<string> selectedDialogue;

        private string initialStringValue;

        public MailItemViewModel()
        {
            this.index = 0;
            this.senderKey = "Debug.UnimplementedText";
            this.subjectKey = "Debug.UnimplementedText";
            this.DialogueKeys = new ObservableCollection<Wrapper<string>>();
            this.DialogueKeys.CollectionChanged += CollectionChanged;
            this.RegisterDialogueKeys();

            this.initialStringValue = string.Empty;
        }

        public MailItemViewModel(int index, MailDefinition mailDefinition)
        {
            this.index = index;
            this.senderKey = mailDefinition.SenderKey;
            this.subjectKey = mailDefinition.SubjectKey;
            this.DialogueKeys = new ObservableCollection<Wrapper<string>>(mailDefinition.DialogueKeys.Select(k => k.Wrap()) ?? new List<Wrapper<string>>());
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

        public string SenderKey
        {
            get
            {
                return this.senderKey;
            }

            set
            {
                this.SetValue(ref this.senderKey, value);
                this.Refresh();
            }
        }

        public string SubjectKey
        {
            get
            {
                return this.subjectKey;
            }

            set
            {
                this.SetValue(ref this.subjectKey, value);
                this.Refresh();
            }
        }

        public ObservableCollection<Wrapper<string>> DialogueKeys { get; }

        public Wrapper<string> SelectedDialogue
        {
            get { return this.selectedDialogue; }
            set { this.SetValue(ref this.selectedDialogue, value); }
        }

        public string Name => Constants.TranslationService.Translate(this.SenderKey).Text + ": " + Constants.TranslationService.Translate(this.SubjectKey).Text;

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
            stringValueBuilder.AppendLine($"<Mail Index=\"{this.Index}\" Subject=\"{this.SenderKey}\" Sender=\"{this.SubjectKey}\">");
            foreach (var dialogueKey in this.DialogueKeys)
            {
                stringValueBuilder.AppendLine($"  <Dialogue Key=\"{dialogueKey.Value}\" />");
            }
            stringValueBuilder.AppendLine("</Mail>");

            return stringValueBuilder.ToString();
        }

        protected override void SetStringValue(string value)
        {
            var lines = value.Trim().Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            var newIndex = 0;
            var newSenderKey = string.Empty;
            var newSubjectKey = string.Empty;
            var newDialogueKeys = new List<string>();

            var openTagMatch = default(Match);
            this.Validate(lines, "Empty mail item", l => l.Length > 0);
            if (this.HasErrors) return;
            this.Validate(lines[0], "Malformed mail item open", l => (openTagMatch = Regex.Match(l, @"<Mail Index=""(\d+)"" Subject=""([^""]+)"" Sender=""([^""]+)"">")).Success);
            newIndex = int.Parse(openTagMatch.Groups[1].Value);
            newSenderKey = openTagMatch.Groups[2].Value;
            newSubjectKey = openTagMatch.Groups[3].Value;
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
                    this.Validate(line, "Malformed mail item close", l => Regex.IsMatch(l, @"</Mail>"));
                    if (this.HasErrors) return;
                    continue;
                }

                var dialogueMatch = default(Match);
                this.Validate(line, "Malformed dialogue tag", l => (dialogueMatch = Regex.Match(l, @"  <Dialogue Key=""([^""]+)"" />")).Success);
                if (this.HasErrors) return;
                newDialogueKeys.Add(dialogueMatch.Groups[1].Value);
            }

            this.Index = newIndex;
            this.SenderKey = newSenderKey;
            this.SubjectKey = newSubjectKey;
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
            var index = s?.IndexOf("Sender=\"", StringComparison.InvariantCulture) ?? -1;

            return index == -1 ? s : s.Substring(index);
        }
    }
}
