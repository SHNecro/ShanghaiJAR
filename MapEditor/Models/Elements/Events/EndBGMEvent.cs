using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class EndBGMEvent : EventBase
    {
        public override string Info => "Turns off the background music.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.EndBGM);

        protected override string GetStringValue()
        {
            return "bgmoff:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed BGM end event \"{value}\".", v => v == "bgmoff:");
        }
    }
}
