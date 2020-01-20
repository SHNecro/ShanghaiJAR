using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EditValueReferenceTypeNumber
    {
        [Description("Value")]
        Value = 0,
        [Description("var[value]")]
        Reference = 1,
        [Description("var[var[value]]")]
        ReferenceReference = 2,
        [Description("Random")]
        Random = 3,
        [Description("Player Value")]
        PlayerValue = 4,
        [Description("Player Position")]
        Position = 5,
        [Description("Player Angle")]
        Angle = 6,
        [Description("Question Ans.")]
        Answer = 7,
    }
}
