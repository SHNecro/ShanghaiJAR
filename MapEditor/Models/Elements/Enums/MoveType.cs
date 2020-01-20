using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MoveType
    {
        [Description("Walk North")]
        WalkUp = 0,
        [Description("Walk South")]
        WalkDown = 1,
        [Description("Walk West")]
        WalkLeft = 2,
        [Description("Walk East")]
        WalkRight = 3,
        [Description("Jump")]
        Jump = 4,
        [Description("Wait")]
        Wait = 5,
        [Description("Face North")]
        AngleUp = 6,
        [Description("Face South")]
        AngleDown = 7,
        [Description("Face West")]
        AngleLeft = 8,
        [Description("Face East")]
        AngleRight = 9,
        [Description("Lock Angle")]
        AngleLock = 10,
        [Description("Unlock Angle")]
        AngleUnlock = 11,
        [Description("Teleport North")]
        WarpUp = 12,
        [Description("Teleport South")]
        WarpDown = 13,
        [Description("Teleport West")]
        WarpLeft = 14,
        [Description("Teleport East")]
        WarpRight = 15,
    }
}
