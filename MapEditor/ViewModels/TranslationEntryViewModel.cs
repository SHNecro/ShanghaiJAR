using Common;
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
				this.translationEntry.Dialogue.Face = value.ToFaceId(this.IsMono, this.IsAuto);
				this.OnPropertyChanged(nameof(this.Face));
				this.OnPropertyChanged(nameof(this.FaceId));

				this.OnPropertyChanged(nameof(this.CustomFaceSheet));
				this.OnPropertyChanged(nameof(this.CustomFaceIndex));
			}
        }

        public FaceId FaceId
        {
            get
            {
                return this.translationEntry.Dialogue.Face;
            }

            set
            {
				this.translationEntry.Dialogue.Face = value;
				this.OnPropertyChanged(nameof(this.Face));
				this.OnPropertyChanged(nameof(this.FaceId));

				this.OnPropertyChanged(nameof(this.CustomFaceSheet));
				this.OnPropertyChanged(nameof(this.CustomFaceIndex));
				this.OnPropertyChanged(nameof(this.IsMono));
				this.OnPropertyChanged(nameof(this.IsAuto));
			}
		}

		public int CustomFaceSheet
		{
			get
			{
                return this.FaceId.Sheet;
			}

			set
			{
                this.FaceId = new FaceId(value, this.CustomFaceIndex, this.IsMono, this.IsAuto);
			}
		}

		public byte CustomFaceIndex
		{
			get
			{
				return this.FaceId.Index;
			}

			set
			{
				this.FaceId = new FaceId(this.CustomFaceSheet, value, this.IsMono, this.IsAuto);
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
				this.FaceId = new FaceId(this.CustomFaceSheet, this.CustomFaceIndex, value, this.IsAuto);
			}
		}

		public bool IsAuto
		{
			get
			{
				return this.translationEntry.Dialogue.Face.Auto;
			}

			set
			{
				this.FaceId = new FaceId(this.CustomFaceSheet, this.CustomFaceIndex, this.IsMono, value);
			}
		}

		public bool IsCustomFace => !Enum.IsDefined(typeof(FACE), ((this.CustomFaceSheet - 1) * 16) + this.CustomFaceIndex);

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
