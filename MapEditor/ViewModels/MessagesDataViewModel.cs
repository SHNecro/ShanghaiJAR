using Data;
using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

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

            foreach (var kvp in this.messages)
            {
                kvp.Value.PropertyChanged += this.MessageTypeIsDirtyChanged;
            }

            this.selectedMessageType = this.messages.First().Value;
        }

        public List<MessageTypeViewModel> MessageTypes => this.messages.Values.ToList();

        public MessageTypeViewModel SelectedMessageTypeModel
        {
            get { return this.selectedMessageType; }
            set { this.SetValue(ref this.selectedMessageType, value); }
        }

        public bool IsDirty => this.MessageTypes.Any(mt => mt.IsDirty);

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand UndoCommand => new RelayCommand(this.Undo);

        private void Save()
        {
            foreach (var messageType in this.MessageTypes)
            {
                messageType.SaveCommand.Execute(null);
            }

            this.OnPropertyChanged(nameof(this.IsDirty));
        }

        private void Undo()
        {
            foreach (var messageType in this.MessageTypes)
            {
                messageType.UndoCommand.Execute(null);
            }
        }

        private void MessageTypeIsDirtyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MessageViewModel.IsDirty))
            {
                this.OnPropertyChanged(nameof(this.IsDirty));
            }
        }
    }
}
