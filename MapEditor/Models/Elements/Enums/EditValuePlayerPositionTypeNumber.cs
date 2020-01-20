using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EditValuePlayerPositionTypeNumber
    {
        [Description("X")]
        X = 0,
        [Description("Y")]
        Y = 1,
        [Description("Z")]
        Z = 2
    }
}
