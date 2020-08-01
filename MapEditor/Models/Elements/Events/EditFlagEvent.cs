namespace MapEditor.Models.Elements.Events
{
    public class EditFlagEvent : EventBase
    {
        private int flagNumber;
        private bool valueToSet;

        public int FlagNumber
        {
            get
            {
                return this.flagNumber;
            }

            set
            {
                this.SetValue(ref this.flagNumber, value);
            }
        }

        public bool ValueToSet
        {
            get
            {
                return this.valueToSet;
            }

            set
            {
                this.SetValue(ref this.valueToSet, value);
            }
        }

        public override string Info => "Set the value of a flag.";

        public override string Name
        {
            get
            {
                var valueToSetString = this.ValueToSet ? "TRUE" : "FALSE";
                return $"Flag {this.FlagNumber} = {valueToSetString}";
            }
        }

        protected override string GetStringValue()
        {
            var valueToSetString = this.ValueToSet.ToString();
            return $"editFlag:{this.FlagNumber}:{valueToSetString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed edit flag event \"{value}\".", e => e.Length == 3 && e[0] == "editFlag"))
            {
                return;
            }

            var newFlagNumber = this.ParseIntOrAddError(entries[1]);
            var newValueToSet = this.ParseBoolOrAddError(entries[2]);

            this.FlagNumber = newFlagNumber;
            this.ValueToSet = newValueToSet;
        }
    }
}
