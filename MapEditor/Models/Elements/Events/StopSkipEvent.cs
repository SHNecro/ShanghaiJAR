using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class StopSkipEvent : EventBase
    {
        public override string Info => "Marks the endpoint of a skippable section besides a battle.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.StopSkip);

        protected override string GetStringValue()
        {
            return "stopSkip:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed skip stop event \"{value}\".", v => v == "stopSkip:");
        }
    }
}
