using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum AngleTypeNumber
    {
        [Description("SouthEast")]
        SouthEast = 0,
        [Description("East")]
        East = 1,
        [Description("NorthEast")]
        NorthEast = 2,
        [Description("North")]
        North = 3,
        [Description("NorthWest")]
        NorthWest = 4,
        [Description("West")]
        West = 5,
        [Description("SouthWest")]
        SouthWest = 6,
        [Description("South")]
        South = 7
    }
}
