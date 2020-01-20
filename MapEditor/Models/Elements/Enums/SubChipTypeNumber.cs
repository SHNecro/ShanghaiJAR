using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SubChipTypeNumber
    {
        [Description("HalfEnrg")]
        HalfEnrg = 0,
        [Description("FullEnrg")]
        FullEnrg = 1,
        [Description("FireWall")]
        FireWall = 2,
        [Description("OpenPort")]
        OpenPort = 3,
        [Description("Anti-Vrs")]
        AntiVrs = 4,
        [Description("VirusScn")]
        VirusScn = 5,
        [Description("CrakTool")]
        CrakTool = 6
    }
}
