using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class EventMoveEndEvent : EventBase
    {
        public override string Info => "Wait for all movement to end.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.EventMoveEnd);

        protected override string GetStringValue()
        {
            return "emoveEnd:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed event move end event \"{value}\".", v => v == "emoveEnd:");
        }
    }
}
