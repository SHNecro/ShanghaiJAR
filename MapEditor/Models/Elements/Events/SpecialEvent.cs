using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class SpecialEvent : EventBase
    {
        private int specialEventNumber;

        public int SpecialEventNumber
        {
            get { return this.specialEventNumber; }
            set { this.SetValue(ref this.specialEventNumber, value); }
        }

        public override string Info => "Performs a hardcoded special event.";

        public override string Name
        {
            get
            {
                var specialEventString = (new EnumDescriptionTypeConverter(typeof(SpecialEventTypeNumber))).ConvertToString((SpecialEventTypeNumber)this.SpecialEventNumber);
                return specialEventString;
            }
        }

        protected override string GetStringValue()
        {
            return $"Special:{this.SpecialEventNumber}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed special event \"{value}\".", e => e.Length == 2 && e[0] == "Special"))
            {
                return;
            }
            
            var newSpecialEventNumber = this.ParseIntOrAddError(entries[1]);
            this.ParseEnumOrAddError<SpecialEventTypeNumber>(entries[1]);

            this.SpecialEventNumber = newSpecialEventNumber;
        }
    }
}
