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
    public class CharacterInfoDataViewModel : StringRepresentation
    {
        private readonly bool[,] floatingCharacters;
        private readonly bool[,] noShadowCharacters;

        private int currentSheetIndex;
        private int characterIndex;
        private int angle;
        private bool isWalking;

        private string originalStringValue;

        public CharacterInfoDataViewModel()
        {
            CharacterInfo.LoadCharacterInfo(out this.floatingCharacters, out this.noShadowCharacters);

            this.originalStringValue = this.StringValue;
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

        public bool IsDirty => this.originalStringValue != this.StringValue;

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand UndoCommand => new RelayCommand(this.Undo);

        public bool IsFloatingCharacter(int sheet, int index)
        {
            return sheet < this.floatingCharacters.GetLength(0) && index < 8 && this.floatingCharacters[sheet, index];
        }

        public bool IsNoShadowCharacter(int sheet, int index)
        {
            return sheet < this.noShadowCharacters.GetLength(0) && index < 8 && this.noShadowCharacters[sheet, index];
        }

        protected override void SetStringValue(string value)
        {
            for (var sheet = 0; sheet < this.floatingCharacters.GetLength(0); sheet++)
            {
                for (var charIndex = 0; charIndex < this.floatingCharacters.GetLength(1); charIndex++)
                {
                    this.floatingCharacters[sheet, charIndex] = Constants.IsFloatingCharacter(sheet, charIndex);
                }
            }

            for (var sheet = 0; sheet < this.noShadowCharacters.GetLength(0); sheet++)
            {
                for (var charIndex = 0; charIndex < this.noShadowCharacters.GetLength(1); charIndex++)
                {
                    this.noShadowCharacters[sheet, charIndex] = Constants.IsNoShadowCharacter(sheet, charIndex);
                }
            }

            this.OnPropertyChanged(nameof(this.IsFloating));
            this.OnPropertyChanged(nameof(this.NoShadow));
        }

        protected override string GetStringValue()
        {
            return this.GetXmlDocument().OuterXml;
        }

        private void Save()
        {
            var xmlDoc = this.GetXmlDocument();

            using (var fs = new FileStream("data/data/CharacterInfo.xml", FileMode.Create))
            {
                using (var xw = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    xmlDoc.WriteTo(xw);
                }
            }

            this.originalStringValue = this.StringValue;
            this.OnPropertyChanged(nameof(this.IsDirty));

            CharacterInfo.LoadCharacterInfo(out Constants.FloatingCharacters, out Constants.NoShadowCharacters);
        }

        private void Undo()
        {
            this.StringValue = this.originalStringValue;
        }

        private XmlDocument GetXmlDocument()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateElement("data"));
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

            return xmlDoc;
        }
    }
}
