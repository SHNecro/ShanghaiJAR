using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class PositionSetEvent : EventBase
    {
        public override string Info => "Stores the player X,Y,Z position in variables 0,1,2.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.PositionSet);

        protected override string GetStringValue()
        {
            return "PositionSet:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed position set event \"{value}\".", v => v == "PositionSet:");
        }
    }
}
