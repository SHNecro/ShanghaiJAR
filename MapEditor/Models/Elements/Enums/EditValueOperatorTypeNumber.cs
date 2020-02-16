using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EditValueOperatorTypeNumber
    {
        [Description("=")]
        Set = 0,
        [Description("+=")]
        Add = 1,
        [Description("-=")]
        Subtract = 2,
        [Description("*=")]
        Multiply = 3,
        [Description("/=")]
        Divide = 4,
        [Description("%=")]
        Modulo = 5,
        [Description("^=")]
        Power = 6
    }
}
