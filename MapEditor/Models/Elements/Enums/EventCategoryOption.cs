using MapEditor.Core.Converters;
using System.ComponentModel;

namespace MapEditor.Models.Elements.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum EventCategoryOption
    {
        // Items and HP
        [Description("Add/Remove Chip")]
        ChipGet,
        [Description("Add/Remove Zenny")]
        Money,
        [Description("Add/Remove HP")]
        HPChange,
        [Description("Add/Remove Key Item")]
        EditItem,
        [Description("Get MysteryData Item")]
        ItemGet,

        // Open Pages
        [Description("Open Shop")]
        Shop,
        [Description("Open Chip Trader")]
        ChipTrader,
        [Description("Open BBS")]
        Forum,
        [Description("Open Request Board")]
        Quest,
        [Description("Open SP Virus Board")]
        Wanted,
        [Description("Open Virus Manager")]
        Virus,

        // Message
        [Description("Message")]
        Message,
        [Description("Question")]
        Question,
        [Description("Number Input")]
        NumSet,
        [Description("Open Message Box")]
        MessageOpen,
        [Description("Close Message Box")]
        MessageClose,

        // Events 
        [Description("Battle")]
        Battle,
        [Description("Get Mail")]
        GetMail,
        [Description("Get Phonecall")]
        GetPhone,
        [Description("Special Event")]
        Special,
        [Description("Complete Request")]
        QuestEnd,
        [Description("Arrange Interior")]
        Interior,

        // Logic
        [Description("Question Branch Head")]
        BranchHead,
        [Description("Branch End")]
        BranchEnd,
        [Description("Go To Label:")]
        GoToLabel,
        [Description("Label")]
        Label,
        [Description("If Flag")]
        IfFlag,
        [Description("If Variable")]
        IfValue,
        [Description("If Have Chip")]
        IfChip,
        [Description("End If")]
        IfEnd,
        [Description("Start Skippable Section")]
        CanSkip,
        [Description("Stop Skippable Section")]
        StopSkip,

        //Variables and Flags
        [Description("Set Flag Value")]
        EditFlag,
        [Description("Set Variable Value")]
        EditValue,
        [Description("Store Player Position")]
        PositionSet,

        // Movement
        [Description("Wait")]
        Wait,
        [Description("Move Object")]
        EventMove,
        [Description("Wait For Movement End")]
        EventMoveEnd,
        [Description("Player Face Screen")]
        FaceHere,
        [Description("Change Position/Map")]
        MapChange,
        [Description("Warp (In Map/Pan)")]
        MapWarp,
        [Description("Warp (Out of Map/Fadeout)")]
        Warp,
        [Description("Warp (Jack Out)")]
        WarpPlugOut,
        [Description("Jack In")]
        PlugIn,
        [Description("Jack In (silent)")]
        PlugInNO,
        [Description("Jack Out")]
        PlugOut,

        // Cutscene / Story events
        [Description("Show/Hide Menu Option")]
        EditMenu,
        [Description("Show/Hide Player")]
        PlayerHide,
        [Description("Show/Hide HUD")]
        StatusHide,
        [Description("Restore BGM")]
        LoadBGM,
        [Description("Store BGM")]
        SaveBGM,
        [Description("Fade BGM Volume")]
        FadeBGM,
        [Description("Disable BGM")]
        EndBGM,
        [Description("Set BGM")]
        StartBGM,
        [Description("Set Camera Position")]
        Camera,
        [Description("Reset Camera Position")]
        CameraDefault,
        [Description("Run Event")]
        EventRun,
		[Description("Show Onscreen Text")]
		Credit,
		[Description("Start Screen Shake")]
        Shake,
        [Description("End Screen Shake")]
        ShakeStop,
        [Description("Play Sound Effect")]
        SEOn,
        [Description("Play Piano Note")]
        Piano,
        [Description("Play Effect")]
        Effect,
        [Description("End Repeating Effect")]
        EffectDelete,
        [Description("End Certain Effects")]
        EffectEnd,
        [Description("Delete self")]
        EventDeath,
        [Description("Delete object")]
        EventDelete,
        [Description("Fade Screen")]
        Fade,
    }
}
