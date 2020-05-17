using Data;
using MapEditor.Core;
using MapEditor.Models.Elements.Enums;
using MapEditor.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Xml;

namespace MapEditor.ViewModels
{
    public class CharacterInfoDataViewModel : ViewModelBase
    {
        private static bool[,] UnmodifiedFloatingCharacters;
        private static bool[,] UnmodifiedNoShadowCharacters;

        private bool isDirty;

        private bool[,] floatingCharacters;
        private bool[,] noShadowCharacters;

        private int currentSheetIndex;
        private int characterIndex;
        private int angle;
        private bool isWalking;

        static CharacterInfoDataViewModel()
        {
            CharacterInfo.LoadCharacterInfo(out UnmodifiedFloatingCharacters, out UnmodifiedNoShadowCharacters);
        }

        public CharacterInfoDataViewModel()
        {
            CharacterInfo.LoadCharacterInfo(out this.floatingCharacters, out this.noShadowCharacters);
        }

        public bool IsDirty
        {
            get
            {
                if (CharacterInfoDataViewModel.UnmodifiedFloatingCharacters.GetLength(0) != this.floatingCharacters.GetLength(0))
                {
                    return true;
                }

                for (var i = 0; i < floatingCharacters.GetLength(0); i++)
                {
                    for (var ii = 0; ii <= 7; ii++)
                    {
                        if (CharacterInfoDataViewModel.UnmodifiedFloatingCharacters[i, ii] != floatingCharacters[i, ii]
                            || CharacterInfoDataViewModel.UnmodifiedNoShadowCharacters[i, ii] != noShadowCharacters[i, ii])
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public int CurrentSheetIndex
        {
            get
            {
                return this.currentSheetIndex;
            }

            set
            {
                this.SetValue(ref this.currentSheetIndex, value);
                this.OnPropertyChanged(nameof(this.IsFloating));
                this.OnPropertyChanged(nameof(this.NoShadow));
            }
        }

        public int CharacterIndex
        {
            get
            {
                return this.characterIndex;
            }

            set
            {
                this.SetValue(ref this.characterIndex, value);
                this.OnPropertyChanged(nameof(this.IsFloating));
                this.OnPropertyChanged(nameof(this.NoShadow));
            }
        }

        public int Angle
        {
            get
            {
                return this.angle;
            }

            set
            {
                this.SetValue(ref this.angle, value);
            }
        }

        public bool IsFloating
        {
            get
            {
                return this.IsFloatingCharacter(this.CurrentSheetIndex, this.CharacterIndex);
            }

            set
            {
                this.SetValue(() => this.IsFloating, (val) => this.floatingCharacters[this.CurrentSheetIndex, this.CharacterIndex] = val, value);
                this.OnPropertyChanged(nameof(this.IsDirty));
            }
        }

        public bool NoShadow
        {
            get
            {
                return this.IsNoShadowCharacter(this.CurrentSheetIndex, this.CharacterIndex);
            }

            set
            {
                this.SetValue(() => this.NoShadow, (val) => this.noShadowCharacters[this.CurrentSheetIndex, this.CharacterIndex] = val, value);
                this.OnPropertyChanged(nameof(this.IsDirty));
            }
        }

        public bool IsWalking
        {
            get
            {
                return this.isWalking;
            }

            set
            {
                this.SetValue(ref this.isWalking, value);
            }
        }

        public string CurrentCharacterSheet => $"charachip{this.CurrentSheetIndex}";

        public bool AngleUp => this.Angle == 3 || this.Angle == 5;

        public int TexX => (this.CharacterIndex < 4 ? 0 : 448) + 0;

        public int TexY => (this.CharacterIndex % 4) * 192 + (this.AngleUp ? 96 : 0);

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public bool IsFloatingCharacter(int sheet, int index)
        {
            return sheet < this.floatingCharacters.GetLength(0) && index < 8 && this.floatingCharacters[sheet, index];
        }

        public bool IsNoShadowCharacter(int sheet, int index)
        {
            return sheet < this.noShadowCharacters.GetLength(0) && index < 8 && this.noShadowCharacters[sheet, index];
        }

        private void Save()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateElement("data"));
            using (var fs = new FileStream("data/data/CharacterInfo.xml", FileMode.Create))
            {
                for (var i = 0; i < floatingCharacters.GetLength(0); i++)
                {
                    for (var ii = 0; ii <= 7; ii++)
                    {
                        var isFloating = this.IsFloatingCharacter(i, ii);
                        var noShadow = this.IsNoShadowCharacter(i, ii);

                        if (!isFloating && !noShadow)
                        {
                            continue;
                        }

                        var characterNode = xmlDoc.CreateElement("Character");

                        var sheetAttribute = xmlDoc.CreateAttribute("Sheet");
                        sheetAttribute.Value = i.ToString();
                        characterNode.Attributes.Append(sheetAttribute);

                        var indexAttribute = xmlDoc.CreateAttribute("Index");
                        indexAttribute.Value = ii.ToString();
                        characterNode.Attributes.Append(indexAttribute);

                        var isFloatingAttribute = xmlDoc.CreateAttribute("IsFloating");
                        isFloatingAttribute.Value = isFloating ? "True" : "False";
                        characterNode.Attributes.Append(isFloatingAttribute);

                        var noShadowAttribute = xmlDoc.CreateAttribute("NoShadow");
                        noShadowAttribute.Value = noShadow ? "True" : "False";
                        characterNode.Attributes.Append(noShadowAttribute);

                        xmlDoc.SelectSingleNode("data").AppendChild(characterNode);
                    }
                }

                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    xmlDoc.WriteTo(xw);
                }
            }

            CharacterInfo.LoadCharacterInfo(out UnmodifiedFloatingCharacters, out UnmodifiedNoShadowCharacters);
            this.OnPropertyChanged(nameof(this.IsDirty));
        }

        public static bool UnmodifiedIsFloatingCharacter(int sheet, int index)
        {
            return sheet < CharacterInfoDataViewModel.UnmodifiedFloatingCharacters.GetLength(0) && index < 8
                && CharacterInfoDataViewModel.UnmodifiedFloatingCharacters[sheet, index];
        }

        public static bool UnmodifiedIsNoShadowCharacter(int sheet, int index)
        {
            return sheet < CharacterInfoDataViewModel.UnmodifiedNoShadowCharacters.GetLength(0) && index < 8
                && CharacterInfoDataViewModel.UnmodifiedNoShadowCharacters[sheet, index];
        }
    }
}
