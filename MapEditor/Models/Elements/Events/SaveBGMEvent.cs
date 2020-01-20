using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class SaveBGMEvent : EventBase
    {
        public override string Info => "Stores the BGM to be restored in the LoadBGM event.";

        public override string Name => new EnumDescriptionTypeConverter(typeof(EventCategoryOption)).ConvertToString(EventCategoryOption.SaveBGM);

        protected override string GetStringValue()
        {
            return "bgmSave:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed BGM restore event \"{value}\".", v => v == "bgmSave:");
        }
    }
}
