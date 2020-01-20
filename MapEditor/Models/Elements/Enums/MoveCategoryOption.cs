using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MoveCategoryOption
    {
        [Description("Walk")]
        Walk = 0,
        [Description("Teleport")]
        Warp = 1,
        [Description("Change Angle")]
        Angle = 2,
        [Description("Jump")]
        Jump = 3,
        [Description("Lock Angle")]
        Lock = 4,
        [Description("Unlock Angle")]
        Unlock = 5,
        [Description("Wait")]
        Wait = 6,
    }
}
