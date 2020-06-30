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
    public class KeyItemDataViewModel : ViewModelBase
    {
        private KeyItemViewModel selectedKeyItem;

        public KeyItemDataViewModel()
        {
            this.KeyItems = new ObservableCollection<KeyItemViewModel>(Constants.KeyItemDefinitions.Select(kvp => new KeyItemViewModel(kvp.Key, kvp.Value)));
            this.KeyItems.CollectionChanged += this.KeyItemsCollectionChanged;

            this.SelectedKeyItem = this.KeyItems.FirstOrDefault();

            foreach (var keyItem in this.KeyItems)
            {
                keyItem.PropertyChanged += this.KeyItemIsDirtyChanged;
            }
        }

        public KeyItemViewModel SelectedKeyItem
        {
            get { return this.selectedKeyItem; }
            set { this.SetValue(ref this.selectedKeyItem, value); }
        }

        public bool IsDirty => this.KeyItems.Any(k => k.IsDirty);

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ObservableCollection<KeyItemViewModel> KeyItems { get; }

        private void KeyItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (var i = 0; i < this.KeyItems.Count; i++)
            {
                this.KeyItems[i].Index = i;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newObject in e.NewItems)
                {
                    var newMessage = newObject as MessageViewModel;
                    if (newMessage != null)
                    {
                        newMessage.PropertyChanged += this.KeyItemIsDirtyChanged;
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
                        oldMessage.PropertyChanged -= this.KeyItemIsDirtyChanged;
                    }
                }
            }
            
            this.OnPropertyChanged(nameof(this.IsDirty));
        }

        private void KeyItemIsDirtyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MessageViewModel.IsDirty))
            {
                this.OnPropertyChanged(nameof(this.IsDirty));
            }
        }

        private void Save()
        {
            var dirtyKeyItems = this.KeyItems.Where(k => k.IsDirty);

            foreach (var keyItem in dirtyKeyItems)
            {
                keyItem.Save();
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateElement("data"));
            using (var fs = new FileStream("data/data/KeyItems.xml", FileMode.Create))
            {
                foreach (var keyItem in this.KeyItems)
                {
                    var keyItemNode = xmlDoc.CreateElement("KeyItem");

                    var indexAttribute = xmlDoc.CreateAttribute("Index");
                    indexAttribute.Value = keyItem.Index.ToString();
                    keyItemNode.Attributes.Append(indexAttribute);

                    var nameAttribute = xmlDoc.CreateAttribute("Name");
                    nameAttribute.Value = keyItem.NameKey;
                    keyItemNode.Attributes.Append(nameAttribute);

                    foreach (var dialogue in keyItem.DialogueKeys)
                    {
                        var dialogueNode = xmlDoc.CreateElement("Dialogue");
                        var keyAttribute = xmlDoc.CreateAttribute("Key");
                        keyAttribute.Value = dialogue.Value;
                        dialogueNode.Attributes.Append(keyAttribute);

                        keyItemNode.AppendChild(dialogueNode);
                    }

                    xmlDoc.SelectSingleNode("data").AppendChild(keyItemNode);
                }

                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    xmlDoc.WriteTo(xw);
                }
            }
        }
    }
}
