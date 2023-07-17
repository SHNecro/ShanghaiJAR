﻿using Common;
using ExtensionMethods;
using MapEditor.Core;
using System;
using System.IO;
using System.Reflection;

namespace MapEditor.ViewModels
{
    public class TranslationEntryViewModel : ViewModelBase
    {
        private string locale;
        private TranslationEntry translationEntry;

        public string Locale
        {
            get { return this.locale; }
            set { this.SetValue(ref this.locale, value); }
        }

        public string Key { get; set; }

        public TranslationEntry TranslationEntry
        {
            get
            {
                return this.translationEntry;
            }
            set
            {
                this.SetValue(ref this.translationEntry, value);
                this.OnPropertyChanged(nameof(this.Dialogue));
                this.OnPropertyChanged(nameof(this.FilePathShort));
            }
        }

        public Dialogue Dialogue
        {
            get
            {
                return this.translationEntry.Dialogue;
            }

            set
            {
                this.translationEntry.Dialogue = value;
                this.OnPropertyChanged(nameof(this.Dialogue));
                this.OnPropertyChanged(nameof(this.Face));
                this.OnPropertyChanged(nameof(this.IsMono));
            }
        }

        public string Text
        {
            get
            {
                return this.translationEntry.Dialogue.Text;
            }

            set
            {
                if (value == null || this.Dialogue?.Text == null)
                {
                    var baseEntry = this.BaseGetterAction?.Invoke();
                    this.Dialogue.Face = baseEntry?.Dialogue?.Face ?? FACE.None.ToFaceId(false);
                    this.OnPropertyChanged(nameof(this.Face));
                    this.OnPropertyChanged(nameof(this.IsMono));
                }
                this.translationEntry.Dialogue.Text = value;
                this.OnPropertyChanged(nameof(this.Text));
            }
        }

        public FACE Face
        {
            get
            {
                return this.translationEntry.Dialogue.Face.ToFace();
            }

            set
            {
                this.translationEntry.Dialogue.Face = value.ToFaceId(this.IsMono);
                this.OnPropertyChanged(nameof(this.Face));
            }
        }

        public bool IsMono
        {
            get
            {
                return this.translationEntry.Dialogue.Face.Mono;
            }

            set
            {
                this.translationEntry.Dialogue.Face = this.Face.ToFaceId(value);
                this.OnPropertyChanged(nameof(this.IsMono));
            }
        }

        public string FilePathShort
        {
            get
            {
                var assemblyLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
                return this.translationEntry.FilePath.Replace(assemblyLoc, "");
            }
        }

        public void SetFilePath(string filePath)
        {
            this.translationEntry.FilePath = filePath;
			this.OnPropertyChanged(nameof(this.FilePathShort));
		}

        public Func<TranslationEntryViewModel> BaseGetterAction { get; set; }

        public void UpdateProperties()
        {
            this.OnPropertyChanged(string.Empty);
        }
    }
}
