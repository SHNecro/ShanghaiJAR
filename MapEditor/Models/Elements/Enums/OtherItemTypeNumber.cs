using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum OtherItemTypeNumber
    {
        [Description("HPMemory")]
        HPMemory = 0,
        [Description("RegUp")]
        RegUp = 1,
        [Description("SubMemory")]
        SubMemory = 2,
        [Description("CorePlus")]
        CorePlus = 3,
        [Description("HertzUp")]
        HertzUp = 4,
        [Description("BugFrag")]
        BugFrag = 5,
        [Description("FrzFrag")]
        FrzFrag = 6,
        [Description("ErrFrag")]
        ErrFrag = 7,
        [Description("Zenny/Text")]
        Zenny = 8,
        [Description("Interior")]
        Interior = 9
    }
}
