using Common;
using ExtensionMethods;
using Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

namespace MapEditor.Core
{
    public class TrackingLanguageTranslationService : ILanguageTranslationService
    {
        public event EventHandler KeysReloaded;

        public TrackingLanguageTranslationService(string[] locales)
        {
            this.Locales = new HashSet<string>(locales);
            // Key, Locale => Dialogue, FilePath
            this.LanguageEntries = new Dictionary<Tuple<string, string>, TranslationEntry>();

            this.DefaultLocale = this.Locales.FirstOrDefault() ?? "en-US";

            foreach (var locale in locales)
            {
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
                    this.LoadTranslationKeys(locale, dir);
                }
            }
        }

        public Dictionary<Tuple<string, string>, TranslationEntry> LanguageEntries { get; }

        public string DefaultLocale { get; set; }

        public HashSet<string> Locales { get; set; }

        public bool CanTranslate(string key) => key != null && this.Locales.All(l => this.LanguageEntries.ContainsKey(new Tuple<string, string>(key, l)));

        public Dialogue Translate(string key) => this.Translate(key, this.DefaultLocale);

        public Dialogue Translate(string key, string locale)
        {
            var decodedKey = System.Web.HttpUtility.HtmlDecode(key);
            var locales = this.LanguageEntries.Keys.Select(t => t.Item2).Distinct();
            if (!this.Locales.All(l => this.LanguageEntries.ContainsKey(new Tuple<string, string>(key, l))))
            {
                throw new KeyNotFoundException($"Key not found: \"{decodedKey}\" in locale(s):{Environment.NewLine}{string.Join(Environment.NewLine, this.Locales.Where(l => !this.LanguageEntries.ContainsKey(new Tuple<string, string>(key, l))))}");
            }
            return LanguageEntries[new Tuple<string, string>(key, locale)].Dialogue;
        }

        public Tuple<string, Rectangle> GetLocalizedSprite(string key) => throw new NotImplementedException("No editor implemented for localized sprites");

        public string GetFontOverride() => null;//throw new NotImplementedException("No editor implemented for localized fonts");

        public IEnumerable<string> GetFilePaths(string locale)
        {
            return this.LanguageEntries.Where(kvp => kvp.Key.Item2 == locale).Select(kvp => kvp.Value.FilePath).Distinct();
        }

        public void ReloadTranslationKeys()
        {
            this.LanguageEntries.Clear();
            foreach (var locale in this.Locales)
            {
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
                    this.LoadTranslationKeys(locale, dir);
                }
            }

            this.KeysReloaded?.Invoke(null, null);
        }

        public bool IsKeyDialogue(string key)
        {
            foreach (var locale in this.Locales)
            {
                if (!this.LanguageEntries.TryGetValue(Tuple.Create(key, locale), out var translationEntry))
                {
                    throw new InvalidOperationException("Key does not exist.");
                }

                var languageDoc = new XmlDocument();
                languageDoc.Load(translationEntry.FilePath);

                using (var fs = new FileStream(translationEntry.FilePath, FileMode.Open))
                {
                    var editedNode = languageDoc.SelectSingleNode($"data/Dialogue[@Key='{key}']");
                    var originalIsDialogue = editedNode != null;
                    if (editedNode == null)
                    {
                        editedNode = languageDoc.SelectSingleNode($"data/Text[@Key='{key}']");
                    }
                    if (editedNode == null)
                    {
                        throw new InvalidOperationException("Key does not exist.");
                    }

                    return originalIsDialogue;
                }
            }

            throw new InvalidOperationException("No locales loaded.");
        }

        public void EditTranslation(string key, string locale, Dialogue newDialogue, bool isDialogue, bool refreshKeys)
        {
            if (!this.LanguageEntries.TryGetValue(Tuple.Create(key, locale), out var translationEntry))
            {
                throw new InvalidOperationException("Attempted edit on nonexistent key.");
            }

            var languageDoc = new XmlDocument();
            languageDoc.Load(translationEntry.FilePath);

            using (var fs = new FileStream(translationEntry.FilePath, FileMode.Create))
            {
                var editedNode = languageDoc.SelectSingleNode($"data/Dialogue[@Key='{key}']");
                var originalIsDialogue = editedNode != null;
                if (editedNode == null)
                {
                    editedNode = languageDoc.SelectSingleNode($"data/Text[@Key='{key}']");
                }
                if (editedNode == null)
                {
                    throw new InvalidOperationException("Attempted edit on nonexistent key.");
                }

                editedNode.Attributes["Value"].Value = newDialogue.Text;
                if (isDialogue)
                {
                    var newNode = languageDoc.CreateElement("Dialogue");
                    newNode.Attributes.Append(editedNode.Attributes["Key"]);
                    newNode.Attributes.Append(editedNode.Attributes["Value"]);

                    var faceAttribute = languageDoc.CreateAttribute("Face");
                    faceAttribute.Value = newDialogue.Face.ToString();
                    newNode.Attributes.Append(faceAttribute);

                    if (newDialogue.Face.Mono)
					{
						var monoAttribute = languageDoc.CreateAttribute("Mono");
						monoAttribute.Value = newDialogue.Face.Mono ? "True" : "False";
						newNode.Attributes.Append(monoAttribute);
					}

					if (newDialogue.Face.Auto)
					{
						var autoAttribute = languageDoc.CreateAttribute("Auto");
						autoAttribute.Value = newDialogue.Face.Auto ? "True" : "False";
						newNode.Attributes.Append(autoAttribute);
					}

					editedNode.ParentNode.ReplaceChild(newNode, editedNode);
                }
                else if (originalIsDialogue)
                {
                    var newNode = languageDoc.CreateElement("Text");
                    newNode.Attributes.Append(editedNode.Attributes["Key"]);
                    newNode.Attributes.Append(editedNode.Attributes["Value"]);

                    editedNode.ParentNode.ReplaceChild(newNode, editedNode);
                }

                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    languageDoc.WriteTo(xw);
                }
            }

            if (refreshKeys)
            {
                this.ReloadTranslationKeys();
            }
        }

        public void AddTranslation(string key, TranslationEntry translationEntry, bool isDialogue, bool refreshKeys)
        {
            var languageDoc = new XmlDocument();
            if (File.Exists(translationEntry.FilePath))
            {
                languageDoc.Load(translationEntry.FilePath);
            }
            else
            {
                languageDoc.AppendChild(languageDoc.CreateElement("data"));
            }

            var newDialogue = translationEntry.Dialogue;

            using (var fs = new FileStream(translationEntry.FilePath, FileMode.Create))
            {
                var newNode = languageDoc.CreateElement(isDialogue ? "Dialogue" : "Text");

                var keyAttribute = languageDoc.CreateAttribute("Key");
                keyAttribute.Value = key;
                newNode.Attributes.Append(keyAttribute);

                var valueAttribute = languageDoc.CreateAttribute("Value");
                valueAttribute.Value = newDialogue.Text;
                newNode.Attributes.Append(valueAttribute);
                if (isDialogue)
                {
                    var faceAttribute = languageDoc.CreateAttribute("Face");
                    faceAttribute.Value = newDialogue.Face.ToString();
                    newNode.Attributes.Append(faceAttribute);

                    if (newDialogue.Face.Mono)
                    {
                        var monoAttribute = languageDoc.CreateAttribute("Mono");
                        monoAttribute.Value = newDialogue.Face.Mono ? "True" : "False";
                        newNode.Attributes.Append(monoAttribute);
					}

					if (newDialogue.Face.Auto)
					{
						var autoAttribute = languageDoc.CreateAttribute("Auto");
						autoAttribute.Value = newDialogue.Face.Auto ? "True" : "False";
						newNode.Attributes.Append(autoAttribute);
					}
				}
                languageDoc.SelectSingleNode("data").AppendChild(newNode);

                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    languageDoc.WriteTo(xw);
                }
            }

            if (refreshKeys)
            {
                this.ReloadTranslationKeys();
            }
        }

        public void DeleteTranslation(string key, string locale, bool refreshKeys)
        {
            if (!this.LanguageEntries.TryGetValue(Tuple.Create(key, locale), out var translationEntry))
            {
                throw new InvalidOperationException("Attempted delete on nonexistent key.");
            }

            var languageDoc = new XmlDocument();
            languageDoc.Load(translationEntry.FilePath);

            using (var fs = new FileStream(translationEntry.FilePath, FileMode.Create))
            {
                var editedNode = languageDoc.SelectSingleNode($"data/Dialogue[@Key='{key}']");
                var originalIsDialogue = editedNode != null;
                if (editedNode == null)
                {
                    editedNode = languageDoc.SelectSingleNode($"data/Text[@Key='{key}']");
                }
                if (editedNode == null)
                {
                    throw new InvalidOperationException("Attempted delete on nonexistent key.");
                }

                editedNode.ParentNode.RemoveChild(editedNode);

                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    languageDoc.WriteTo(xw);
                }
            }


            if (refreshKeys)
            {
                this.ReloadTranslationKeys();
            }
        }

        public void DeleteAllTranslations(string key)
        {
            foreach (var locale in this.Locales)
            {
                this.DeleteTranslation(key, locale, false);
            }

            this.ReloadTranslationKeys();
        }

        private void LoadTranslationKeys(string locale, string directory)
        {
            var skippedFaces = new List<string>();
            var languageFiles = new DirectoryInfo(directory).GetFiles("*.xml");
            foreach (var keyFile in languageFiles)
            {
                var keyPath = Path.Combine(directory, keyFile.Name);
                var languageDoc = new XmlDocument();
                languageDoc.Load(keyPath);
                
                var entries = languageDoc.SelectNodes("data/*");
                foreach (XmlNode node in entries)
                {
                    if (node.Name != "Dialogue" && node.Name != "Text")
                    {
                        continue;
                    }
                    var key = node.Attributes["Key"].Value;
                    var value = node.Attributes["Value"].Value;

                    var newDialogue = new Dialogue { Text = value };
                    if (node.Name == "Dialogue")
					{
						var monoProperty = node.Attributes["Mono"]?.Value ?? "False";
						var mono = bool.Parse(monoProperty);
						var autoProperty = node.Attributes["Auto"]?.Value ?? "False";
						var auto = bool.Parse(autoProperty);
						var faceString = node.Attributes["Face"].Value;
                        if (Enum.TryParse(faceString, out FACE faceEnum))
                        {
                            newDialogue.Face = faceEnum.ToFaceId(mono, auto);
						}
						else
						{
							var manualFaceTokens = node.Attributes["Face"].Value.Split(',');
							int sheet;
							byte index;
							if (manualFaceTokens.Length == 2
								&& int.TryParse(manualFaceTokens[0], out sheet)
								&& byte.TryParse(manualFaceTokens[1], out index))
							{
								newDialogue.Face = new FaceId(sheet, index, mono, auto);
							}
							else
							{
								newDialogue.Face = FACE.None.ToFaceId(mono, auto);
							}
						}
					}

                    var newKey = new Tuple<string, string>(key, locale);
                    if (!this.LanguageEntries.ContainsKey(newKey))
                    {
                        this.LanguageEntries.Add(newKey, new TranslationEntry { Dialogue = newDialogue, FilePath = keyPath });
                    }
                }
            }
        }
    }
}
