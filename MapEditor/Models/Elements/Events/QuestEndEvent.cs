using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class QuestEndEvent : EventBase
    {
        public override string Info => "Marks the current quest as complete.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.QuestEnd);

        protected override string GetStringValue()
        {
            return "QuestEnd:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed quest end event \"{value}\".", v => v == "QuestEnd:");
        }
    }
}
