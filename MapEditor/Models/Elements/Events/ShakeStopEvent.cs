using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class ShakeStopEvent : EventBase
    {
        public override string Info => "Ends the screenshake.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.ShakeStop);

        protected override string GetStringValue()
        {
            return "shakeStop:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed shake stop event \"{value}\".", v => v == "shakeStop:");
        }
    }
}
