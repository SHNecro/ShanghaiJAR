using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class GetPhoneEvent : EventBase
    {
        public override string Info => "Trigger a phone call.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.Quest);

        protected override string GetStringValue()
        {
            return "Getphone:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed phonecall event \"{value}\".", v => v == "Getphone:");
        }
    }
}
