using MapEditor.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml;

namespace MapEditor.ViewModels
{
    public class MailDataViewModel : ViewModelBase
    {
        private MailItemViewModel selectedMail;

        public MailDataViewModel()
        {
            this.Mail = new ObservableCollection<MailItemViewModel>(Constants.MailDefinitions.Select(kvp => new MailItemViewModel(kvp.Key, kvp.Value)));
            this.Mail.CollectionChanged += this.MailCollectionChanged;

            this.SelectedMail = this.Mail.FirstOrDefault();

            foreach (var keyItem in this.Mail)
            {
                keyItem.PropertyChanged += this.MailIsDirtyChanged;
            }
        }

        public MailItemViewModel SelectedMail
        {
            get { return this.selectedMail; }
            set { this.SetValue(ref this.selectedMail, value); }
        }

        public bool IsDirty => this.Mail.Any(k => k.IsDirty);

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ObservableCollection<MailItemViewModel> Mail { get; }

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

            var xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateElement("data"));
            using (var fs = new FileStream("data/data/Mail.xml", FileMode.Create))
            {
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

                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    xmlDoc.WriteTo(xw);
                }
            }
        }
    }
}
