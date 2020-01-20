using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class JackOutEvent : EventBase
    {
        public override string Info => "Jack out.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.PlugOut);

        protected override string GetStringValue()
        {
            return "PlugOut:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed jack out event \"{value}\".", v => v == "PlugOut:");
        }
    }
}
