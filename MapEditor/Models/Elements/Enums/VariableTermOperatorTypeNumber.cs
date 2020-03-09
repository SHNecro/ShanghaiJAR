using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum VariableTermOperatorTypeNumber
    {
        [Description("==")]
        Equals = 0,
        [Description("<=")]
        LessThanOrEquals = 1,
        [Description(">=")]
        GreaterThanOrEquals = 2,
        [Description("<")]
        LessThan = 3,
        [Description(">")]
        GreaterThan = 4,
        [Description("!=")]
        NotEquals = 5
    }
}
