using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class LoadBGMEvent : EventBase
    {
        public override string Info => "Restores the BGM stored by a SaveBGM event (if changed by a battle, etc.).";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.LoadBGM);

        protected override string GetStringValue()
        {
            return "bgmLoad:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed BGM store event \"{value}\".", v => v == "bgmLoad:");
        }
    }
}
