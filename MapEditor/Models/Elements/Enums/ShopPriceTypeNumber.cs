using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ShopPriceTypeNumber
    {
        [Description("Zenny")]
        Zenny = 0,
        [Description("BugFrags")]
        BugFrags = 1,
        [Description("FrzFrags")]
        FrzFrags = 2,
        [Description("ErrFrags")]
        ErrFrags = 3,
    }
}
