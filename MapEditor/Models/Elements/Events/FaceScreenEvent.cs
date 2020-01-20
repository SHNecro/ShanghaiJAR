using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class FaceScreenEvent : EventBase
    {
        public override string Info => "Changes the player angle to face the camera.";

        public override string Name => (new EnumDescriptionTypeConverter(typeof(EventCategoryOption))).ConvertToString(EventCategoryOption.FaceHere);

        protected override string GetStringValue()
        {
            return "Facehere:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, $"Malformed facehere event \"{value}\".", v => v == "Facehere:");
        }
    }
}
