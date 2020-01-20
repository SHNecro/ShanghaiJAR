using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ItemTypeNumber
    {
        [Description("Chip")]
        Chip = 0,
        [Description("SubChip")]
        SubChip = 1,
        [Description("AddOn")]
        AddOn = 2,
        [Description("Other")]
        Other = 3,
        [Description("Virus")]
        Virus = 4,
    }
}
