using Data;
using MapEditor.Core;
using MapEditor.ExtensionMethods;
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
    public class MessagesDataViewModel : ViewModelBase
    {
        private Dictionary<MessageType, MessageTypeViewModel> messages;
        private MessageTypeViewModel selectedMessageType;

        public MessagesDataViewModel()
        {
            this.messages = new Dictionary<MessageType, MessageTypeViewModel>();
            this.messages[MessageType.ShanghaiInfo] = new MessageTypeViewModel(MessageType.ShanghaiInfo, "ShanghaiInfo.xml");
            this.messages[MessageType.AliceInfo] = new MessageTypeViewModel(MessageType.AliceInfo, "AliceInfo.xml");
            this.messages[MessageType.ShanghaiRequestInfo] = new MessageTypeViewModel(MessageType.ShanghaiRequestInfo, "ShanghaiRequestInfo.xml");
            this.messages[MessageType.AliceRequestInfo] = new MessageTypeViewModel(MessageType.AliceRequestInfo, "AliceRequestInfo.xml");
            this.messages[MessageType.HumorInfo] = new MessageTypeViewModel(MessageType.HumorInfo, "HumorInfo.xml");
            this.messages[MessageType.UNKNOWN1] = new MessageTypeViewModel(MessageType.UNKNOWN1, "UNKNOWN1.xml");
            this.messages[MessageType.UNKNOWN2] = new MessageTypeViewModel(MessageType.UNKNOWN2, "UNKNOWN2.xml");
            this.messages[MessageType.EirinCallInfo] = new MessageTypeViewModel(MessageType.EirinCallInfo, "EirinCallInfo.xml");
            this.messages[MessageType.RequestBoard] = new MessageTypeViewModel(MessageType.RequestBoard, "RequestBoard.xml");
            this.messages[MessageType.RequestBoardComplete] = new MessageTypeViewModel(MessageType.RequestBoardComplete, "RequestBoardComplete.xml");
            this.messages[MessageType.GenBoard] = new MessageTypeViewModel(MessageType.GenBoard, "GenBoard.xml");
            this.messages[MessageType.UniversityBoard] = new MessageTypeViewModel(MessageType.UniversityBoard, "UniversityBoard.xml");
            this.messages[MessageType.EienBoard] = new MessageTypeViewModel(MessageType.EienBoard, "EienBoard.xml");
            this.messages[MessageType.UnderBoard] = new MessageTypeViewModel(MessageType.UnderBoard, "UnderBoard.xml");
            this.messages[MessageType.UnderBattleBoard] = new MessageTypeViewModel(MessageType.UnderBattleBoard, "UnderBattleBoard.xml");
            this.messages[MessageType.UNKNOWN3] = new MessageTypeViewModel(MessageType.UNKNOWN3, "UNKNOWN3.xml");

            this.selectedMessageType = this.messages.First().Value;
        }

        public List<MessageTypeViewModel> MessageTypes => this.messages.Values.ToList();

        public MessageTypeViewModel SelectedMessageTypeModel
        {
            get { return this.selectedMessageType; }
            set { this.SetValue(ref this.selectedMessageType, value); }
        }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        private void Save()
        {
            this.SelectedMessageTypeModel.SaveCommand.Execute(null);
        }

        public class MessageTypeViewModel : ViewModelBase
        {
            private readonly MessageType messageType;
            private string filePath;

            private MessageViewModel selectedMessage;

            public MessageTypeViewModel(MessageType messageType, string file)
            {
                this.messageType = messageType;
                this.Messages = new ObservableCollection<MessageViewModel>();
                this.Messages.CollectionChanged += this.UpdateMessageIndices;

                this.filePath = $"data/data/Messages/{file}";

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(this.filePath);
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

                        this.Messages.Add(messageViewModel);
                    }
                }

                this.selectedMessage = this.Messages.FirstOrDefault();
            }

            public ObservableCollection<MessageViewModel> Messages { get; }

            public MessageViewModel SelectedMessage
            {
                get { return this.selectedMessage; }
                set { this.SetValue(ref this.selectedMessage, value); }
            }

            public string Label => this.messageType.ToString() + (this.IsDirty ? "*" : string.Empty);

            public bool IsDirty => this.Messages.Any(m => m.IsDirty);

            public ICommand SaveCommand => new RelayCommand(this.Save);

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
                    message.Save();
                }

                var xmlDoc = new XmlDocument();
                xmlDoc.AppendChild(xmlDoc.CreateElement("data"));
                using (var fs = new FileStream(this.filePath, FileMode.Create))
                {
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

                    using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                    {
                        xmlDoc.WriteTo(xw);
                    }
                }
            }

            private void Undo()
            {
                var dirtyMessages = this.Messages.Where(m => m.IsDirty);

                foreach (var message in dirtyMessages)
                {
                    message.Undo();
                }
            }
        }
    }
}
