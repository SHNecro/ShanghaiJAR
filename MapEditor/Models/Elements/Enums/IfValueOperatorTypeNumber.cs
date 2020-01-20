using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum IfValueOperatorTypeNumber
    {
        [Description("==")]
        Equals = 0,
        [Description(">=")]
        GreaterThanOrEquals = 1,
        [Description("<=")]
        LessThanOrEquals = 2,
        [Description(">")]
        GreaterThan = 3,
        [Description("<")]
        LessThan = 4,
        [Description("!=")]
        NotEquals = 5,
    }
}
