using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum VariableTermOperatorTypeNumber
    {
        [Description("==")]
        Equals = 0,
        [Description(">=")]
        GreaterThanOrEquals = 1,
        [Description("<")]
        LessThan = 2,
        [Description(">")]
        GreaterThan = 3,
        [Description("<=")]
        LessThanOrEquals = 4,
        [Description("!=")]
        NotEquals = 5
    }
}
