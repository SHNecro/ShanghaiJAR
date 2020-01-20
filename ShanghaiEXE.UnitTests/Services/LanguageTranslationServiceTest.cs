using Common;
using ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace ShanghaiEXE.UnitTests
{
    [TestClass]
    public class LanguageTranslationServiceTest
    {
        private static readonly HashSet<string> Forbidden = new HashSet<string> {
            ".git",
            "obj",
            "Debug",
            "Release",
            "script"
        };

        [TestMethod]
        public void Translate_AllTranslateCalls_RefersToRealEnglishKeys()
        {
            var languageTranslationService = new LanguageTranslationService("en-US");
            var failures = new List<string>();
            WalkAllCodeFiles(file => {
                var reader = file.OpenText();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var matches = Regex.Matches(line, "ShanghaiEXE\\.Translate\\(\"([^\"]*)\"\\)");
                    if (matches.Count != 0)
                    {
                        try
                        {
                            languageTranslationService.Translate(matches[0].Groups[1].Value);
                        }
                        catch
                        {
                            failures.Add(matches[0].Groups[1].Value);
                        }
                    }
                }
            });

            Assert.AreEqual(0, failures.Count);
        }

        [TestMethod]
        public void Translate_AllTranslateCalls_RefersToRealJapansesKeys()
        {
            var languageTranslationService = new LanguageTranslationService("ja-JP");
            var failures = new List<string>();
            WalkAllCodeFiles(file => {
                var reader = file.OpenText();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var matches = Regex.Matches(line, "ShanghaiEXE\\.Translate\\(\"([^\"]*)\"\\)");
                    if (matches.Count != 0)
                    {
                        try
                        {
                            languageTranslationService.Translate(matches[0].Groups[1].Value);
                        }
                        catch
                        {
                            failures.Add(matches[0].Groups[1].Value);
                        }
                    }
                }
            });

            Assert.AreEqual(0, failures.Count);
        }

        [TestMethod]
        public void Translate_AllKeys_LanguagesMatch()
        {
            var locales = new List<string> {
                "en-US",
                "ja-JP"
            };
            var language = new Dictionary<string, Dictionary<string, Dialogue>>();
            foreach (var locale in locales)
            {
                language[locale] = new Dictionary<string, Dialogue>();
                void loadTranslationKeys(string directory)
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
                            language[locale].Add(key, new Dialogue { Text = value });
                        }

                        var dialogue = languageDoc.SelectNodes("data/Dialogue");
                        foreach (XmlNode node in dialogue)
                        {
                            var key = node.Attributes["Key"].Value;
                            var value = node.Attributes["Value"].Value;
                            var mono = bool.Parse(node.Attributes["Mono"].Value);
                            var face = ((FACE)Enum.Parse(typeof(FACE), node.Attributes["Face"].Value)).ToFaceId(mono);
                            language[locale].Add(key, new Dialogue { Text = value, Face = face });
                        }
                    }
                }

                loadTranslationKeys($"language/{locale}");
                loadTranslationKeys($"language/{locale}/Map");
                loadTranslationKeys($"language/{locale}/Messages");
            };


            var failures = new List<string>();
            for (int i = 0; i < locales.Count; i++)
            {
                var origLanguage = language[locales[i]];
                var comparedLocales = new List<string>();
                for (int ii = i+1; ii < locales.Count; ii++)
                {
                    comparedLocales.Add(locales[ii]);
                }
            
                foreach (var compLocale in comparedLocales)
                {
                    var compLanguage = language[compLocale];

                    foreach (var key in origLanguage.Keys)
                    {
                        if (!compLanguage.ContainsKey(key))
                        {
                            failures.Add(key);
                        }
                        else
                        {
                            var origDialogue = origLanguage[key];
                            var compDialogue = compLanguage[key];

                            if (origDialogue.Face.ToFace() != compDialogue.Face.ToFace() || origDialogue.Face.Mono != compDialogue.Face.Mono)
                            {
                                failures.Add(key);
                            }
                        }
                    }
                }
            }

            Assert.AreEqual(0, failures.Count);
        }

        private void WalkAllCodeFiles(Action<FileInfo> act)
        {
            var projectDir = new DirectoryInfo("../../ShanghaiEXE");

            void walk(DirectoryInfo dir)
            {
                foreach (var file in dir.GetFiles("*.xml"))
                {
                    act(file);
                }
                foreach (var file in dir.GetFiles("*.cs"))
                {
                    act(file);
                }
                foreach (var file in dir.GetFiles("*.shd"))
                {
                    act(file);
                }

                foreach (var childDir in dir.GetDirectories())
                {
                    if (!Forbidden.Contains(childDir.Name))
                    {
                        walk(childDir);
                    }
                }
            }

            walk(projectDir);
        }
    }
}
