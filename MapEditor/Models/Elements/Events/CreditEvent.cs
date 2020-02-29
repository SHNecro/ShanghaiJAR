namespace MapEditor.Models.Elements.Events
{
    public class CreditEvent : EventBase
    {
		private string creditKey;
        private int x;
        private int y;
        private bool centered;
		private int fadeInTime;
		private int hangTime;
		private int fadeOutTime;

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
				var dialogue = Constants.TranslationService.Translate(this.CreditKey);
				return $"Credits ({this.X}, {this.Y}): {dialogue.Text}";
			}
        }

        protected override string GetStringValue()
        {
            var centeredText = this.Centered ? "True" : "False";
            return $"credit:{this.CreditKey}:{this.X}:{this.Y}:{centeredText}:{this.FadeInTime}:{this.HangTime}:{this.FadeOutTime}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed credit event \"{value}\".", e => e.Length == 8 && e[0] == "credit"))
            {
                return;
            }

			var newCreditKey = entries[1];
			this.Validate(newCreditKey, "Credit key does not exist.", k => Constants.TranslationService.CanTranslate(k));
			var newX = this.ParseIntOrAddError(entries[2]);
            var newY = this.ParseIntOrAddError(entries[3]);
            var newCentered = this.ParseBoolOrAddError(entries[4]);
            var newFadeInTime = this.ParseIntOrAddError(entries[5]);
            var newHangTime = this.ParseIntOrAddError(entries[6]);
			var newFadeOutTime = this.ParseIntOrAddError(entries[7]);

			if (!this.HasErrors)
            {
				this.CreditKey = newCreditKey;
				this.X = newX;
				this.Y = newY;
                this.Centered = newCentered;
				this.FadeInTime = newFadeInTime;
				this.HangTime = newHangTime;
				this.FadeOutTime = newFadeOutTime;
            }
        }
    }
}
