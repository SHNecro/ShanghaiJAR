﻿using Common;
using System;
using System.Collections.Generic;
using System.Xml;

namespace NSGame
{
    [Serializable]
    internal class TestMail : Mail
    {
        private static readonly Dictionary<int, TestMail> Mail;

        static TestMail()
        {
            Mail = new Dictionary<int, TestMail>();
            LoadMail();
        }

        private static void LoadMail()
        {
            var languageDoc = new XmlDocument();
            languageDoc.Load($"data/data/Mail.xml");

            var characterNodes = languageDoc.SelectNodes("data/Mail");
            foreach (XmlNode characterNode in characterNodes)
            {
                var index = int.Parse(characterNode?.Attributes["Index"]?.Value ?? "-1");

                if (index == -1)
                {
                    throw new InvalidOperationException("Invalid Key Item index.");
                }

                var subject = ShanghaiEXE.Translate(characterNode?.Attributes["Subject"].Value);
                var sender = ShanghaiEXE.Translate(characterNode?.Attributes["Sender"].Value);

                var dialogues = new List<Dialogue>();

                var info = new List<string>();
                var dialogueNodes = characterNode.ChildNodes;
                foreach (XmlNode dialogueXml in dialogueNodes)
                {
                    dialogues.Add(ShanghaiEXE.Translate(dialogueXml.Attributes["Key"].Value));
                }

                Mail[index] = new TestMail(subject, sender, dialogues);
            }
        }

        public TestMail(int number)
        {
            this.title = Mail[number].title;
            this.parson = Mail[number].parson;
            this.txt = Mail[number].txt;
        }

        private TestMail(string subject, string sender, List<Dialogue> dialogues)
        {
            this.title = subject;
            this.parson = sender;
            this.txt = dialogues;
        }
    }
}
