using Common;
using System;
using System.Collections.Generic;
using System.Xml;

namespace NSGame
{
    [Serializable]
    internal class KeyItem
    {
        private static readonly Dictionary<int, KeyItem> KeyItems;

        public List<string> info = new List<string>();
        public string name;

        static KeyItem()
        {
            KeyItems = new Dictionary<int, KeyItem>();
            LoadKeyItems();
        }

        public KeyItem(int index)
        {
            this.name = KeyItems[index].name;
            this.info = KeyItems[index].info;
        }

        private KeyItem(string name, List<string> info)
        {
            this.name = name;
            this.info = info;
        }

        private static void LoadKeyItems()
        {
            var languageDoc = new XmlDocument();
            languageDoc.Load($"data/data/KeyItems.xml");

            var keyItemNodes = languageDoc.SelectNodes("data/KeyItem");
            foreach (XmlNode keyItemNode in keyItemNodes)
            {
                var index = int.Parse(keyItemNode?.Attributes["Index"]?.Value ?? "-1");

                if (index == -1)
                {
                    throw new InvalidOperationException("Invalid Key Item index.");
                }

                var name = ShanghaiEXE.Translate(keyItemNode?.Attributes["Name"].Value);

                var info = new List<string>();
                var dialogues = keyItemNode.ChildNodes;
                foreach (XmlNode dialogueXml in dialogues)
                {
                    var dialogue = ShanghaiEXE.Translate(dialogueXml.Attributes["Key"].Value);
                    info.Add(dialogue[0]);
                    info.Add(dialogue[1]);
                    info.Add(dialogue[2]);
                }

                KeyItems[index] = new KeyItem(name, info);
            }
        }
    }
}
