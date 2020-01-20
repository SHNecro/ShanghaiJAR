using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum KeyItemTypeNumber
    {
        [Description("Compressed Data")]
        CompressedData = 0,
        [Description("Heat Data")]
        HeatData = 1,
        [Description("Antifreeze")]
        Antifreeze = 2,
        [Description("Metro Pass")]
        MetroPass = 3,
        [Description("Ice Crystal Data")]
        IceCrystalData = 4,
        [Description("Package Data")]
        PackageData = 5,
        [Description("Mari P-Code")]
        MariPCode = 6,
        [Description("Remilia P-Code")]
        RemiliaPCode = 7,
        [Description("Rika P-Code")]
        RikaPCode = 8,
        [Description("Tsubaki P-Code")]
        TsubakiPCode = 9,
        [Description("Tenshi P-Code")]
        TenshiPCode = 10,
        [Description("SpareCode")]
        SpareCode = 11,
        [Description("Old Chip Data")]
        OldChipData = 12,
        [Description("Teacher ID")]
        TeacherID = 13,
        [Description("Cruise Ticket")]
        CruiseTicket = 14,
        [Description("Yin Key")]
        YinKey = 15,
        [Description("Yang Key")]
        YangKey = 16,
        [Description("Admin P-Code")]
        AdminPCode = 17,
        [Description("Small Parcel")]
        SmallParcel = 18,
        [Description("Wriggle's Pendant")]
        WrigglePendant = 19,
        [Description("ROM ID")]
        ROMID = 20,
        [Description("Preserved Flowers")]
        PreservedFlowers = 21,
        [Description("Old Key Data")]
        OldKeyData = 22,
        [Description("Old ID Data")]
        OldIDData = 23,
        [Description("Old Passcode")]
        OldPasscode = 24,
        [Description("Reincarnation Program")]
        ReincarnationProgram = 25
    }
}
