using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MysteryTypeNumber
    {
        [Description("Green")]
        Green = 0,
        [Description("Blue")]
        Blue = 1,
        [Description("Purple")]
        Purple = 2
    }
}
