using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class BranchEndEvent : EventBase
    {
        public override string Info => "Closing event for BranchHead, marks end of skipped section when question response does not match.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.BranchEnd);

        protected override string GetStringValue()
        {
            return "BranchEnd:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, "Malformed branch end.", v => v == "BranchEnd:");
        }
    }
}
