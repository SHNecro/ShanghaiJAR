using Data;
using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml;

namespace MapEditor.ViewModels
{
    public class MessageTypeViewModel : StringRepresentation
    {
        private readonly MessageType messageType;
        private readonly string filePath;

        private ObservableCollection<MessageViewModel> messages;
        private MessageViewModel selectedMessage;

        private string originalStringValue;

        public MessageTypeViewModel(MessageType messageType, string file)
        {
            this.messageType = messageType;
            this.Messages = new ObservableCollection<MessageViewModel>();
            this.Messages.CollectionChanged += this.UpdateMessageIndices;

            this.filePath = $"data/data/Messages/{file}";
            this.StringValue = File.ReadAllText(this.filePath);

            this.selectedMessage = this.Messages.FirstOrDefault();

            this.originalStringValue = this.StringValue;
        }

        public ObservableCollection<MessageViewModel> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                if (this.Messages != null)
                {
                    this.Messages.CollectionChanged -= this.UpdateMessageIndices;
                    foreach (var message in this.Messages)
                    {
                        message.PropertyChanged -= this.MessageIsDirtyChanged;
                    }
                }

                this.SetValue(ref this.messages, value);

                this.Messages.CollectionChanged += this.UpdateMessageIndices;
                foreach (var message in this.Messages)
                {
                    message.PropertyChanged += this.MessageIsDirtyChanged;
                }

                this.SelectedMessage = this.Messages.FirstOrDefault();
            }
        }

        public MessageViewModel SelectedMessage
        {
            get { return this.selectedMessage; }
            set { this.SetValue(ref this.selectedMessage, value); }
        }

        public string Label => this.messageType.ToString() + (this.IsDirty ? "*" : string.Empty);

        public bool IsDirty => this.StringValue != this.originalStringValue;

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand UndoCommand => new RelayCommand(this.Undo);

        protected override string GetStringValue()
        {
            return this.GetXmlDocument().OuterXml;
        }

        protected override void SetStringValue(string value)
        {
            this.Messages.Clear();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(value);
            var text = xmlDoc.SelectNodes("data/Message");
            var messagesDict = new Dictionary<int, MessageViewModel>();
            foreach (XmlNode message in text)
            {
                var index = int.Parse(message.Attributes["Index"].Value);
                var dialogues = message.ChildNodes;
                var dialogueKeyText = Enumerable.Repeat(default(Wrapper<string>), dialogues.Count).ToList();
                var nodeNumber = 0;
                foreach (XmlNode dialogue in message)
                {
                    dialogueKeyText[nodeNumber++] = dialogue.Attributes["Key"].Value.Wrap();
                }

                messagesDict[index] = new MessageViewModel(index, dialogueKeyText);
            }

            var fullMessages = new List<MessageViewModel>();

            if (messagesDict.Any())
            {
                var messageCount = messagesDict.Keys.Max();
                for (var i = 0; i <= messageCount; i++)
                {
                    var messageViewModel = default(MessageViewModel);
                    if (!messagesDict.TryGetValue(i, out messageViewModel))
                    {
                        messageViewModel = new MessageViewModel(i, new List<Wrapper<string>>());
                    }

                    messageViewModel.PropertyChanged += this.MessageIsDirtyChanged;

                    fullMessages.Add(messageViewModel);
                }
            }

            this.Messages = new ObservableCollection<MessageViewModel>(fullMessages);
        }

        private void UpdateMessageIndices(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (var i = 0; i < this.Messages.Count; i++)
            {
                this.Messages[i].Index = i;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newObject in e.NewItems)
                {
                    var newMessage = newObject as MessageViewModel;
                    if (newMessage != null)
                    {
                        newMessage.PropertyChanged += this.MessageIsDirtyChanged;
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
                        oldMessage.PropertyChanged -= this.MessageIsDirtyChanged;
                    }
                }
            }

            this.OnPropertyChanged(nameof(this.Label));
            this.OnPropertyChanged(nameof(this.IsDirty));
        }

        private void MessageIsDirtyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MessageViewModel.IsDirty))
            {
                this.OnPropertyChanged(nameof(this.IsDirty));
                this.OnPropertyChanged(nameof(this.Label));
            }
        }

        private void Save()
        {
            var dirtyMessages = this.Messages.Where(m => m.IsDirty);

            foreach (var message in dirtyMessages)
            {
                message.SaveCommand.Execute(null);
            }

            var xmlDoc = this.GetXmlDocument();

            using (var fs = new FileStream(this.filePath, FileMode.Create))
            {
                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    xmlDoc.WriteTo(xw);
                }
            }

            this.originalStringValue = this.StringValue;

            this.OnPropertyChanged(nameof(this.IsDirty));
        }

        private void Undo()
        {
            this.StringValue = this.originalStringValue;
        }

        private XmlDocument GetXmlDocument()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateElement("data"));
            foreach (var message in this.Messages)
            {
                var messageNode = xmlDoc.CreateElement("Message");

                var indexAttribute = xmlDoc.CreateAttribute("Index");
                indexAttribute.Value = message.Index.ToString();
                messageNode.Attributes.Append(indexAttribute);

                foreach (var dialogue in message.DialogueKeys)
                {
                    var dialogueNode = xmlDoc.CreateElement("Dialogue");
                    var keyAttribute = xmlDoc.CreateAttribute("Key");
                    keyAttribute.Value = dialogue.Value;
                    dialogueNode.Attributes.Append(keyAttribute);

                    messageNode.AppendChild(dialogueNode);
                }

                xmlDoc.SelectSingleNode("data").AppendChild(messageNode);
            }

            return xmlDoc;
        }
    }
}
