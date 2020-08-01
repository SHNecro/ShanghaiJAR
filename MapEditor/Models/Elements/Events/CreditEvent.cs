using MapEditor.Core;
using System;

namespace MapEditor.Models.Elements.Events
{
    public class CreditEvent : EventBase, ITranslatedModel
    {
        public const int Timed = 0;
        public const int FadeIn = 1;
        public const int FadeOut = 2;

        private int creditType;

        private string creditKey;
        private int x;
        private int y;
        private bool centered;
        private bool movesWithCamera;
        private int fadeInTime;
        private int hangTime;
        private int fadeOutTime;

        public int CreditType
        {
            get
            {
                return this.creditType;
            }

            set
            {
                this.UpdateCreditType(value);
                this.SetValue(ref this.creditType, value);
            }
        }

		public string CreditKey
		{
			get
			{
				return this.creditKey;
			}

			set
			{
				this.SetValue(ref this.creditKey, value);
			}
		}

		public int X
		{
			get
			{
				return this.x;
			}

			set
			{
				this.SetValue(ref this.x, value);
			}
		}

		public int Y
        {
            get
            {
                return this.y;
            }

            set
            {
                this.SetValue(ref this.y, value);
            }
        }

        public bool Centered
        {
            get
            {
                return this.centered;
            }

            set
            {
                this.SetValue(ref this.centered, value);
            }
        }

        public bool MovesWithCamera
        {
            get
            {
                return this.movesWithCamera;
            }

            set
            {
                this.SetValue(ref this.movesWithCamera, value);
            }
        }

        public int FadeInTime
        {
            get
            {
                return this.fadeInTime;
            }

            set
            {
                this.SetValue(ref this.fadeInTime, value);
            }
		}

		public int HangTime
		{
			get
			{
				return this.hangTime;
			}

			set
			{
				this.SetValue(ref this.hangTime, value);
			}
		}

		public int FadeOutTime
		{
			get
			{
				return this.fadeOutTime;
			}

			set
			{
				this.SetValue(ref this.fadeOutTime, value);
			}
		}

		public override string Info => "Shows text with the given key onscreen at the (screen) X and Y positions, fading in and out.";

        public override string Name
        {
            get
			{
                var typeText = this.CreditType == CreditEvent.FadeOut ? $"FadeOut " : string.Empty;
				var dialogue = Constants.TranslationService.Translate(this.CreditKey);
				return $"{typeText}Onscreen Text ({this.X}, {this.Y}): {dialogue.Text}";
			}
        }

        public void RefreshTranslation()
        {
            this.OnPropertyChanged(nameof(this.Name));
        }

        protected override string GetStringValue()
        {
            var centeredText = this.Centered ? "True" : "False";
            var movesWithCameraText = this.movesWithCamera ? "True" : "False";
            var adjustedHangTime = this.HangTime;
            switch (this.CreditType)
            {
                case CreditEvent.FadeIn:
                    adjustedHangTime = -1;
                    break;
                case CreditEvent.FadeOut:
                    adjustedHangTime = -2;
                    break;
            }
            return $"credit:{this.CreditKey}:{this.X}:{this.Y}:{centeredText}:{movesWithCamera}:{this.FadeInTime}:{adjustedHangTime}:{this.FadeOutTime}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed credit event \"{value}\".", e => e.Length == 9 && e[0] == "credit"))
            {
                return;
            }

			var newCreditKey = entries[1];
			this.Validate(newCreditKey, () => this.CreditKey, s => $"Credit key \"{s}\" does not exist.", Constants.TranslationService.CanTranslate);
			var newX = this.ParseIntOrAddError(entries[2]);
            var newY = this.ParseIntOrAddError(entries[3]);
            var newCentered = this.ParseBoolOrAddError(entries[4]);
            var newMovesWithCamera = this.ParseBoolOrAddError(entries[5]);
            var newFadeInTime = this.ParseIntOrAddError(entries[6]);
            var newHangTime = this.ParseIntOrAddError(entries[7]);
			var newFadeOutTime = this.ParseIntOrAddError(entries[8]);

            this.CreditKey = newCreditKey;
            this.X = newX;
            this.Y = newY;
            this.Centered = newCentered;
            this.MovesWithCamera = newMovesWithCamera;
            this.FadeInTime = newFadeInTime;
            this.HangTime = newHangTime < 0 ? 30 : newHangTime;
            this.FadeOutTime = newFadeOutTime;

            switch (newHangTime)
            {
                case -1:
                    this.creditType = CreditEvent.FadeIn;
                    break;
                case -2:
                    this.creditType = CreditEvent.FadeOut;
                    break;
                default:
                    this.creditType = CreditEvent.Timed;
                    break;
            }
        }

        private void UpdateCreditType(int value)
        {
            switch (this.CreditType)
            {
                case 0:
                    switch (value)
                    {
                        case 1:
                            this.FadeOutTime = 0;
                            break;
                        case 2:
                            this.FadeInTime = 0;
                            break;
                    }
                    break;
                case 1:
                    switch (value)
                    {
                        case 0:
                            this.FadeOutTime = 0;
                            break;
                        case 2:
                            var id = this.fadeOutTime;
                            var fadeTime = this.FadeInTime;
                            this.FadeInTime = id;
                            this.FadeOutTime = fadeTime;
                            break;
                    }
                    break;
                case 2:
                    switch (value)
                    {
                        case 0:
                            this.FadeInTime = 0;
                            break;
                        case 1:
                            var id = this.FadeInTime;
                            var fadeTime = this.fadeOutTime;
                            this.FadeOutTime = this.FadeInTime;
                            this.FadeInTime = fadeTime;
                            this.FadeOutTime = id;
                            break;
                    }
                    break;
            }
        }
    }
}
