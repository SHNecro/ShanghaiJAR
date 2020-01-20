using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class WarpPlugOutEvent : EventBase
    {
        public override string Info => "Plays the jack out animation and jacks out.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.WarpPlugOut);

        protected override string GetStringValue()
        {
            return "warpPlugOut:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed jack out warp event \"{value}\".", v => v == "warpPlugOut:");
        }
    }
}
