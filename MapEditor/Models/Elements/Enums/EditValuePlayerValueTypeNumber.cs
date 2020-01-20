using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EditValuePlayerValueTypeNumber
    {
        [Description("Max HP")]
        HPMax = 0,
        [Description("Current HP")]
        HPNow = 1,
        [Description("Money")]
        Money = 2,
        [Description("BugFrag")]
        BugFrag = 3,
        [Description("ErrFrag")]
        ErrFrag = 4,
        [Description("FrzFrag")]
        FrzFrag = 5
    }
}
