using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class WantedEvent : EventBase
    {
        public override string Info => "Opens the SP Virus board.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.Wanted);

        protected override string GetStringValue()
        {
            return "Wanted:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed SP virus board event \"{value}\".", v => v == "Wanted:");
        }
    }
}
