using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class VirusEvent : EventBase
    {
        public override string Info => "Opens the virus manager.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.Virus);

        protected override string GetStringValue()
        {
            return "Virus:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed virus manager event \"{value}\".", v => v == "Virus:");
        }
    }
}
