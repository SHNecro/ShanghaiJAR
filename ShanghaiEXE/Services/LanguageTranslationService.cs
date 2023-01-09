using Common;
using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;

namespace Services
{
    public class LanguageTranslationService : ILanguageTranslationService
    {
        private readonly Dictionary<string, Dialogue> language;
        private readonly Dictionary<string, Tuple<string, Rectangle>> localizedSprites;

        private string fontOverride;

        public LanguageTranslationService(string locale)
        {
            this.language = new Dictionary<string, Dialogue>();
            this.localizedSprites = new Dictionary<string, Tuple<string, Rectangle>>();

            //this.LoadTranslationKeys($"language/{locale}");
            //this.LoadTranslationKeys($"language/{locale}/Map");
            //this.LoadTranslationKeys($"language/{locale}/Messages");

            var localeLanguageFolder = new DirectoryInfo($"language/{locale}");
            var subFolders = new List<string> { localeLanguageFolder.FullName };
            var unexploredSubFolders = localeLanguageFolder.GetDirectories().ToList();
            while (unexploredSubFolders.Count > 0)
            {
                subFolders.AddRange(unexploredSubFolders.Select(dir => dir.FullName));
                var subSubFolders = unexploredSubFolders.SelectMany(dir => dir.GetDirectories());
                unexploredSubFolders.Clear();
                unexploredSubFolders.AddRange(subSubFolders);
            }

            foreach (var dir in subFolders)
            {
                this.LoadTranslationKeys(dir);
            }
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

        public Tuple<string, Rectangle> GetLocalizedSprite(string key) => this.localizedSprites[key];

        public string GetFontOverride() => this.fontOverride;

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

                var spriteRectangleConverter = new RectangleConverter();
                var sprite = languageDoc.SelectNodes("data/Sprite");
                foreach (XmlNode node in sprite)
                {
                    var key = node.Attributes["Key"].Value;
                    var sheet = node.Attributes["Sheet"].Value;
                    var rect = (Rectangle)spriteRectangleConverter.ConvertFromString(node.Attributes["Sprite"].Value.Replace(' ', ','));

                    this.localizedSprites.Add(key, Tuple.Create(sheet, rect));
                }

                var font = languageDoc.SelectNodes("data/FontOverride");
                foreach (XmlNode node in font)
                {
                    var ttf = node.Attributes["TTF"].Value;
                    this.fontOverride = ttf;
                }
            }
        }
    }
}
