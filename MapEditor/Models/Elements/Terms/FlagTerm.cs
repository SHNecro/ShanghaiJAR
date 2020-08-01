namespace MapEditor.Models.Elements.Terms
{
    public class FlagTerm : TermBase
    {
        private bool inverted;
        private int flag;

        public bool Inverted
        {
            get
            {
                return this.inverted;
            }

            set
            {
                this.SetValue(ref this.inverted, value);
            }
        }

        public int Flag
        {
            get
            {
                return this.flag;
            }

            set
            {
                this.SetValue(ref this.flag, value);
            }
        }

        public override string Name
        {
            get
            {
                var notPrefix = this.Inverted ? "NOT " : string.Empty;
                return $"{notPrefix}Flag {this.Flag}";
            }
        }

        protected override string GetStringValue()
        {
            var flagString = this.Inverted ? "!flag" : "flag";
            return $"{flagString}/{this.Flag}";
        }

        protected override void SetStringValue(string value)
        {
            var newInverted = default(bool);
            int newFlag = default(int);
            var flagParams = value.Split('/');
            if (this.Validate(flagParams, $"Malformed flag term \"{value}\".", fp => fp.Length == 2))
            {
                newInverted = flagParams[0] == "!flag";
                newFlag = this.ParseIntOrAddError(flagParams[1]);
            }

            this.Inverted = newInverted;
            this.Flag = newFlag;
        }
    }
}
