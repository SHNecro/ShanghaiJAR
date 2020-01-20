using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum TermCategoryOption
    {
        [Description("None")]
        None = 0,
        [Description("Flag")]
        Flag = 1,
        [Description("Chip")]
        Chip = 2,
        [Description("Variable")]
        Variable = 3
    }
}
