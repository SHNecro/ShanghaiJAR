using Common;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Services
{
    public class LanguageTranslationService : ILanguageTranslationService
    {
        private readonly Dictionary<string, Dialogue> language;

        public LanguageTranslationService(string locale)
        {
            this.language = new Dictionary<string, Dialogue>();

            this.LoadTranslationKeys($"language/{locale}");
            this.LoadTranslationKeys($"language/{locale}/Map");
            this.LoadTranslationKeys($"language/{locale}/Messages");
        }

        public bool CanTranslate(string key) => key != null && language.ContainsKey(key);

        public Dialogue Translate(string key)
        {
            var decodedKey = System.Web.HttpUtility.HtmlDecode(key);
            if (!language.ContainsKey(decodedKey))
            {
                // throw new KeyNotFoundException($"Key not found: {decodedKey}");
                return new Dialogue { Face = FACE.None.ToFaceId(), Text = decodedKey };
            }
            return language[decodedKey];
        }

        private void LoadTranslationKeys(string directory)
        {
            var languageFiles = new DirectoryInfo(directory).GetFiles("*.xml");
            foreach (var keyFile in languageFiles)
            {
                var languageDoc = new XmlDocument();
                languageDoc.Load($"{directory}/{keyFile}");

                var text = languageDoc.SelectNodes("data/Text");
                foreach (XmlNode node in text)
                {
                    var key = node.Attributes["Key"].Value;
                    var value = node.Attributes["Value"].Value;
                    this.language.Add(key, new Dialogue { Text = value });
                }

                var dialogue = languageDoc.SelectNodes("data/Dialogue");
                foreach (XmlNode node in dialogue)
                {
                    var key = node.Attributes["Key"].Value;
                    var value = node.Attributes["Value"].Value;
                    var mono = bool.Parse(node.Attributes["Mono"].Value);
                    var face = ((FACE)Enum.Parse(typeof(FACE), node.Attributes["Face"].Value)).ToFaceId(mono);
                    this.language.Add(key, new Dialogue { Text = value, Face = face });
                }
            }
        }
    }
}
