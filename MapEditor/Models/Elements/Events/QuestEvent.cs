using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class QuestEvent : EventBase
    {
        public override string Info => "Opens the Request board.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.Quest);

        protected override string GetStringValue()
        {
            return "Quest:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed quest end event \"{value}\".", v => v == "Quest:");
        }
    }
}
