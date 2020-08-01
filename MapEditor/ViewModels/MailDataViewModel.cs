using MapEditor.Core;
using MapEditor.Models.Elements;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml;

namespace MapEditor.ViewModels
{
    public class MailDataViewModel : StringRepresentation
    {
        private ObservableCollection<MailItemViewModel> mail;
        private MailItemViewModel selectedMail;

        private int lastSelectedIndex;
        private string originalStringValue;

        public MailDataViewModel()
        {
            this.Mail = new ObservableCollection<MailItemViewModel>(Constants.MailDefinitions.Select(kvp => new MailItemViewModel(kvp.Key, kvp.Value)));
            this.Mail.CollectionChanged += this.MailCollectionChanged;

            this.SelectedMail = this.Mail.FirstOrDefault();

            foreach (var keyItem in this.Mail)
            {
                keyItem.PropertyChanged += this.MailIsDirtyChanged;
            }

            this.originalStringValue = this.StringValue;
        }

        public ObservableCollection<MailItemViewModel> Mail
        {
            get
            {
                return this.mail;
            }
            set
            {
                if (this.Mail != null)
                {
                    this.Mail.CollectionChanged -= this.MailCollectionChanged;
                    foreach (var mailItem in this.Mail)
                    {
                        mailItem.PropertyChanged -= this.MailIsDirtyChanged;
                    }
                }

                this.SetValue(ref this.mail, value);

                this.Mail.CollectionChanged += this.MailCollectionChanged;
                foreach (var mailItem in this.Mail)
                {
                    mailItem.PropertyChanged += this.MailIsDirtyChanged;
                }

                this.SelectedMail = this.Mail.FirstOrDefault();
            }
        }

        public MailItemViewModel SelectedMail
        {
            get
            {
                return this.selectedMail;
            }
            set
            {
                if (value != null || this.Mail.Count == 0)
                {
                    this.SetValue(ref this.selectedMail, value);
                    this.lastSelectedIndex = this.Mail.IndexOf(this.SelectedMail);
                }
            }
        }

        public bool IsDirty => this.originalStringValue != this.StringValue;

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand UndoCommand => new RelayCommand(this.Undo);

        protected override void SetStringValue(string value)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(value);

            var globalMail = Constants.LoadMail(xmlDoc);
            this.Mail = new ObservableCollection<MailItemViewModel>(globalMail.Select(kvp => new MailItemViewModel(kvp.Key, kvp.Value)));

            this.SelectedMail = this.Mail.FirstOrDefault();
        }

        protected override string GetStringValue()
        {
            return this.GetXmlDocument().OuterXml;
        }

        private void MailCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (var i = 0; i < this.Mail.Count; i++)
            {
                this.Mail[i].Index = i;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newObject in e.NewItems)
                {
                    var newMessage = newObject as MessageViewModel;
                    if (newMessage != null)
                    {
                        newMessage.PropertyChanged += this.MailIsDirtyChanged;
                    }
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldObject in e.OldItems)
                {
                    var oldMessage = oldObject as MessageViewModel;
                    if (oldMessage != null)
                    {
                        oldMessage.PropertyChanged -= this.MailIsDirtyChanged;
                    }
                }
            }

            if (!this.Mail.Contains(this.SelectedMail))
            {
                this.SelectedMail = this.lastSelectedIndex < this.Mail.Count && this.lastSelectedIndex >= 0
                    ? this.Mail[this.lastSelectedIndex] : this.Mail.LastOrDefault();
            }

            this.OnPropertyChanged(nameof(this.Mail));
            this.OnPropertyChanged(nameof(this.IsDirty));
        }

        private void MailIsDirtyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MessageViewModel.IsDirty))
            {
                this.OnPropertyChanged(nameof(this.IsDirty));
            }
        }

        private void Save()
        {
            var dirtyMail = this.Mail.Where(k => k.IsDirty);

            foreach (var mailItem in dirtyMail)
            {
                mailItem.Save();
            }

            var xmlDoc = this.GetXmlDocument();

            using (var fs = new FileStream("data/data/Mail.xml", FileMode.Create))
            {
                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    xmlDoc.WriteTo(xw);
                }
            }

            var globalMailItemKeys = Constants.MailDefinitions.Keys.ToList();
            foreach (var globalDefinitionKey in globalMailItemKeys)
            {
                Constants.MailDefinitions.Remove(globalDefinitionKey);
            }

            foreach (var mailItem in this.Mail)
            {
                Constants.MailDefinitions.Add(mailItem.Index, new MailDefinition
                {
                    SenderKey = mailItem.SenderKey,
                    SubjectKey = mailItem.SubjectKey,
                    DialogueKeys = mailItem.DialogueKeys.Select(ws => ws.Value).ToList()
                });
            }

            this.OnPropertyChanged(nameof(this.IsDirty));
        }

        private XmlDocument GetXmlDocument()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateElement("data"));

            foreach (var mailItem in this.Mail)
            {
                var mailItemNode = xmlDoc.CreateElement("Mail");

                var indexAttribute = xmlDoc.CreateAttribute("Index");
                indexAttribute.Value = mailItem.Index.ToString();
                mailItemNode.Attributes.Append(indexAttribute);

                var senderAttribute = xmlDoc.CreateAttribute("Sender");
                senderAttribute.Value = mailItem.SenderKey;
                mailItemNode.Attributes.Append(senderAttribute);

                var subjectAttribute = xmlDoc.CreateAttribute("Subject");
                subjectAttribute.Value = mailItem.SubjectKey;
                mailItemNode.Attributes.Append(subjectAttribute);

                foreach (var dialogue in mailItem.DialogueKeys)
                {
                    var dialogueNode = xmlDoc.CreateElement("Dialogue");
                    var keyAttribute = xmlDoc.CreateAttribute("Key");
                    keyAttribute.Value = dialogue.Value;
                    dialogueNode.Attributes.Append(keyAttribute);

                    mailItemNode.AppendChild(dialogueNode);
                }

                xmlDoc.SelectSingleNode("data").AppendChild(mailItemNode);
            }

            return xmlDoc;
        }

        private void Undo()
        {
            this.StringValue = this.originalStringValue;
        }
    }
}
