using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class CanSkipEvent : EventBase
    {
        public override string Info => "Marks start of a section that can be skipped, jumping to the next StopSkip or Battle event.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.CanSkip);

        protected override string GetStringValue()
        {
            return "canSkip:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed skip section event \"{value}\".", v => v == "canSkip:");
        }
    }
}
