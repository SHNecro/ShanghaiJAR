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
    public class KeyItemDataViewModel : StringRepresentation
    {
        private ObservableCollection<KeyItemViewModel> keyItems;
        private KeyItemViewModel selectedKeyItem;

        private int lastSelectedIndex;
        private string originalStringValue;

        public KeyItemDataViewModel()
        {
            this.KeyItems = new ObservableCollection<KeyItemViewModel>(Constants.KeyItemDefinitions.Select(kvp => new KeyItemViewModel(kvp.Key, kvp.Value)));
            this.KeyItems.CollectionChanged += this.KeyItemsCollectionChanged;

            this.SelectedKeyItem = this.KeyItems.FirstOrDefault();

            foreach (var keyItem in this.KeyItems)
            {
                keyItem.PropertyChanged += this.KeyItemIsDirtyChanged;
            }

            this.originalStringValue = this.StringValue;
        }

        public ObservableCollection<KeyItemViewModel> KeyItems
        {
            get
            {
                return this.keyItems;
            }
            set
            {
                if (this.KeyItems != null)
                {
                    this.KeyItems.CollectionChanged -= this.KeyItemsCollectionChanged;
                    foreach (var keyItem in this.KeyItems)
                    {
                        keyItem.PropertyChanged -= this.KeyItemIsDirtyChanged;
                    }
                }

                this.SetValue(ref this.keyItems, value);

                this.KeyItems.CollectionChanged += this.KeyItemsCollectionChanged;
                foreach (var keyItem in this.KeyItems)
                {
                    keyItem.PropertyChanged += this.KeyItemIsDirtyChanged;
                }

                this.SelectedKeyItem = this.KeyItems.FirstOrDefault();
            }
        }

        public KeyItemViewModel SelectedKeyItem
        {
            get
            {
                return this.selectedKeyItem;
            }
            set
            {
                if (value != null || this.KeyItems.Count == 0)
                {
                    this.SetValue(ref this.selectedKeyItem, value);
                    this.lastSelectedIndex = this.KeyItems.IndexOf(this.SelectedKeyItem);
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

            var globalKeyItems = Constants.LoadKeyItems(xmlDoc);
            this.KeyItems = new ObservableCollection<KeyItemViewModel>(globalKeyItems.Select(kvp => new KeyItemViewModel(kvp.Key, kvp.Value)));

            this.SelectedKeyItem = this.KeyItems.FirstOrDefault();
        }

        protected override string GetStringValue()
        {
            return this.GetXmlDocument().OuterXml;
        }

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

            if (!this.KeyItems.Contains(this.SelectedKeyItem))
            {
                this.SelectedKeyItem = this.lastSelectedIndex < this.KeyItems.Count && this.lastSelectedIndex >= 0 ? this.KeyItems[this.lastSelectedIndex] : this.KeyItems.LastOrDefault();
            }

            this.OnPropertyChanged(nameof(this.KeyItems));
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

            var xmlDoc = this.GetXmlDocument();

            using (var fs = new FileStream("data/data/KeyItems.xml", FileMode.Create))
            {
                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    xmlDoc.WriteTo(xw);
                }
            }
            this.originalStringValue = this.StringValue;

            var globalKeyItemKeys = Constants.KeyItemDefinitions.Keys.ToList();
            foreach (var globalDefinitionKey in globalKeyItemKeys)
            {
                Constants.KeyItemDefinitions.Remove(globalDefinitionKey);
            }

            foreach (var keyItem in this.KeyItems)
            {
                Constants.KeyItemDefinitions.Add(keyItem.Index, new KeyItemDefinition
                {
                    NameKey = keyItem.NameKey,
                    DialogueKeys = keyItem.DialogueKeys.Select(ws => ws.Value).ToList()
                });
            }

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

            return xmlDoc;
        }
    }
}
