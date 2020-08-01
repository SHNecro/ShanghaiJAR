namespace MapEditor.Models.Elements.Events
{
    public class ChipTraderEvent : EventBase
    {
        private bool isSpecial;

        public bool IsSpecial
        {
            get
            {
                return this.isSpecial;
            }

            set
            {
                this.SetValue(ref this.isSpecial, value);
            }
        }

        public override string Info => "Opens the normal or special chiptrader screen.";

        public override string Name
        {
            get
            {
                var specialString = this.IsSpecial ? "Special " : string.Empty;
                return $"Open {specialString}Chip Trader";
            }
        }

        protected override string GetStringValue()
        {
            var specialString = this.IsSpecial ? "1" : "0";
            return $"chiptreader:{specialString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed chiptrader event \"{value}\".", e => e.Length == 2 && e[0] == "chiptreader"))
            {
                return;
            }

            var newIsSpecial = this.ParseIntOrAddError(entries[1]) == 1;

            this.IsSpecial = newIsSpecial;
        }
    }
}
