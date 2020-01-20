using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ForumTypeNumber
    {
        [Description("Genso Square")]
        GensoSquare = 0,
        [Description("City Square")]
        CitySquare = 1,
        [Description("Eien Square")]
        EienSquare = 2,
        [Description("Japan UnderSquare")]
        JapanUnderSquare = 3,
        [Description("World UnderSquare")]
        WorldUnderSquare = 4
    }
}
