﻿using Common;
using System;
using System.Collections.Generic;
using System.Xml;

namespace NSGame
{
    [Serializable]
    internal class TestItem : KeyItem
    {
        private static readonly Dictionary<int, TestItem> KeyItems;

        static TestItem()
        {
            KeyItems = new Dictionary<int, TestItem>();
            LoadKeyItems();
        }

        public TestItem(int index)
        {
            this.name = KeyItems[index].name;
            this.info = KeyItems[index].info;
        }

        private TestItem(string name, List<string> info)
        {
            this.name = name;
            this.info = info;
        }

        public static void LoadKeyItems()
        {
            var languageDoc = new XmlDocument();
            languageDoc.Load($"data/data/KeyItems.xml");

            var characterNodes = languageDoc.SelectNodes("data/KeyItem");
            foreach (XmlNode characterNode in characterNodes)
            {
                var index = int.Parse(characterNode?.Attributes["Index"]?.Value ?? "-1");

                if (index == -1)
                {
                    throw new InvalidOperationException("Invalid Key Item index.");
                }

                var name = ShanghaiEXE.Translate(characterNode?.Attributes["Name"].Value);

                var info = new List<string>();
                var dialogues = characterNode.ChildNodes;
                foreach (XmlNode dialogueXml in dialogues)
                {
                    var dialogue = ShanghaiEXE.Translate(dialogueXml.Attributes["Key"].Value);
                    info.Add(dialogue[0]);
                    info.Add(dialogue[1]);
                    info.Add(dialogue[2]);
                }

                KeyItems[index] = new TestItem(name, info);
            }
        }
    }
}
