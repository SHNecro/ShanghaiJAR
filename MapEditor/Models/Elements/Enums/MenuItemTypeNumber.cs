using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MenuItemTypeNumber
    {
        [Description("Folder")]
        Folder = 0,
        [Description("SubChip")]
        SubChip = 1,
        [Description("Library")]
        Library = 2,
        [Description("Navi")]
        Navi = 3,
        [Description("Virus")]
        Virus = 4,
        [Description("Mail")]
        Mail = 5,
        [Description("KeyItem")]
        KeyItem = 6,
        [Description("NetWork")]
        NetWork = 7,
        [Description("Save")]
        Save = 8
    }
}
