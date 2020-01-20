using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum HitFormType
    {
        [Description("Square")]
        Square = 0,
        [Description("Circle")]
        Circle = 1
    }
}
