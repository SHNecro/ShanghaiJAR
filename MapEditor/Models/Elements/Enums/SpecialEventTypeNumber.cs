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
        [Description("Check CrakTool (Q=1)")]
        CheckCrakTool = 16,
        [Description("Remove CrakTool if Q==0 (Q=1 or Q=0)")]
        RemoveCrakTool = 17,
        [Description("Piano Collision Check")]
        PianoCollision = 18,
        [Description("Set Bounty Completion Var 7 (of 41)")]
        IfSPBounties = 19,
        [Description("Set Std Library Completion Var 7 (of 190)")]
        IfStdChips = 20,
        [Description("Set PA Library Completion Var 7 (of 32)")]
        IfStdPAs = 21,
    }
}
