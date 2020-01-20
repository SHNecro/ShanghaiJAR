using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum SpecialEventTypeNumber
    {
        [Description("Create Starter Folder")]
        CreateStarterFolder = 0,
        [Description("Set Extra Folder")]
        SetExtraFolder = 1,
        [Description("Set N1 Folder")]
        SetN1Folder = 2,
        [Description("Set Siren Folder")]
        SetSirenFolder = 3,
        [Description("Set Akin Folder")]
        SetAkinFolder = 4,
        [Description("Set Meiji Folder")]
        SetMeijiFolder = 5,
        [Description("Enable Style Change")]
        EnableStyleChange = 8,
        [Description("Remove 100 BugFrag")]
        Remove100BugFrag = 9,
        [Description("Add Virus Ball 1")]
        AddVirusBall1 = 10,
        [Description("Add Virus Ball 2")]
        AddVirusBall2 = 11,
        [Description("Add Virus Ball 3")]
        AddVirusBall3 = 12,
        [Description("Lock to Extra Folder")]
        LockExtraFolder = 13,
        [Description("Clear Flags 40-59")]
        ClearFlags = 14,
        [Description("Stop Parallel Events")]
        StopParallelEvents = 15,
    }
}
