using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;
using System;

namespace MapEditor.Models.Elements.Events
{
    public class InteriorEvent : EventBase
    {
        public override string Info => "Place all interior items to saved locations.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.Interior);

        protected override string GetStringValue()
        {
            return "Interior:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed interior event \"{value}\".", v => v.StartsWith("Interior:", StringComparison.InvariantCulture));
        }
    }
}
