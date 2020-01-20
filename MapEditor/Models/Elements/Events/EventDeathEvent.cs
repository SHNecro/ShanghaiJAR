using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class EventDeathEvent : EventBase
    {
        public override string Info => "Delete self.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.EventDeath);

        protected override string GetStringValue()
        {
            return "eventDeath:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed event death event \"{value}\".", v => v == "eventDeath:");
        }
    }
}
