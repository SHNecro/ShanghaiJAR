using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum RenderTypeNumber
    {
        [Description("Under")]
        Under = 0,
        [Description("Normal")]
        Normal = 1,
        [Description("Over")]
        Over = 2
    }
}
