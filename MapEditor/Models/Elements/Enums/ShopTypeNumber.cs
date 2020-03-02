using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ShopTypeNumber
    {
        [Description("Chips / HPMemory")]
        Chips = 0,
        [Description("SubChips")]
        SubChips = 1,
        [Description("AddOns")]
        AddOns = 2,
        [Description("Interiors")]
        Interiors = 3,
    }
}
