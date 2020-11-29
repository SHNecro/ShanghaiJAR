using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Models.Elements.Enums;
using MapEditor.Models.Elements.Events;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace MapEditor.Models
{
    public class EventObject : StringRepresentation
    {
        private EventBase instance;

        public EventBase Instance
        {
            get
            {
                return this.instance;
            }

            set
            {
                if (this.Instance != null)
                {
                    this.Instance.PropertyChanged -= this.OnInstancePropertyChanged;
                }

                this.SetValue(ref this.instance, value);
                this.Instance.PropertyChanged += this.OnInstancePropertyChanged;
            }
        }

        public EventCategoryOption Category
        {
            get
            {
                switch (this.Instance)
                {
                    case EditItemEvent e:
                        return EventCategoryOption.EditItem;
                    case ItemGetEvent e:
                        return EventCategoryOption.ItemGet;
                    case ChipGetEvent e:
                        return EventCategoryOption.ChipGet;
                    case MoneyEvent e:
                        return EventCategoryOption.Money;
                    case HPChangeEvent e:
                        return EventCategoryOption.HPChange;
                    case ForumEvent e:
                        return EventCategoryOption.Forum;
                    case QuestEvent e:
                        return EventCategoryOption.Quest;
                    case VirusEvent e:
                        return EventCategoryOption.Virus;
                    case WantedEvent e:
                        return EventCategoryOption.Wanted;
                    case ChipTraderEvent e:
                        return EventCategoryOption.ChipTrader;
                    case ShopEvent e:
                        return EventCategoryOption.Shop;
                    case InteriorEvent e:
                        return EventCategoryOption.Interior;
                    case MessageEvent e:
                        return EventCategoryOption.Message;
                    case QuestionEvent e:
                        return EventCategoryOption.Question;
                    case NumSetEvent e:
                        return EventCategoryOption.NumSet;
                    case MessageOpenEvent e:
                        return EventCategoryOption.MessageOpen;
                    case MessageCloseEvent e:
                        return EventCategoryOption.MessageClose;
                    case BattleEvent e:
                        return EventCategoryOption.Battle;
                    case GetMailEvent e:
                        return EventCategoryOption.GetMail;
                    case GetPhoneEvent e:
                        return EventCategoryOption.GetPhone;
                    case SpecialEvent e:
                        return EventCategoryOption.Special;
                    case QuestEndEvent e:
                        return EventCategoryOption.QuestEnd;
                    case BranchEndEvent e:
                        return EventCategoryOption.BranchEnd;
                    case BranchHeadEvent e:
                        return EventCategoryOption.BranchHead;
                    case GoToLabelEvent e:
                        return EventCategoryOption.GoToLabel;
                    case LabelEvent e:
                        return EventCategoryOption.Label;
                    case IfFlagEvent e:
                        return EventCategoryOption.IfFlag;
                    case IfValueEvent e:
                        return EventCategoryOption.IfValue;
                    case IfChipEvent e:
                        return EventCategoryOption.IfChip;
                    case IfEndEvent e:
                        return EventCategoryOption.IfEnd;
                    case CanSkipEvent e:
                        return EventCategoryOption.CanSkip;
                    case StopSkipEvent e:
                        return EventCategoryOption.StopSkip;
                    case EventMoveEvent e:
                        return EventCategoryOption.EventMove;
                    case EventMoveEndEvent e:
                        return EventCategoryOption.EventMoveEnd;
                    case FaceScreenEvent e:
                        return EventCategoryOption.FaceHere;
                    case PositionSetEvent e:
                        return EventCategoryOption.PositionSet;
                    case MapChangeEvent e:
                        return EventCategoryOption.MapChange;
                    case MapWarpEvent e:
                        return EventCategoryOption.MapWarp;
                    case WarpEvent e:
                        return EventCategoryOption.Warp;
                    case WarpPlugOutEvent e:
                        return EventCategoryOption.WarpPlugOut;
                    case JackInEvent e:
                        return EventCategoryOption.PlugIn;
                    case JackInDirectEvent e:
                        return EventCategoryOption.PlugInNO;
                    case JackOutEvent e:
                        return EventCategoryOption.PlugOut;
                    case EditMenuEvent e:
                        return EventCategoryOption.EditMenu;
                    case StatusHideEvent e:
                        return EventCategoryOption.StatusHide;
                    case LoadBGMEvent e:
                        return EventCategoryOption.LoadBGM;
                    case SaveBGMEvent e:
                        return EventCategoryOption.SaveBGM;
                    case FadeBGMEvent e:
                        return EventCategoryOption.FadeBGM;
                    case EndBGMEvent e:
                        return EventCategoryOption.EndBGM;
                    case StartBGMEvent e:
                        return EventCategoryOption.StartBGM;
                    case CameraEvent e:
                        return EventCategoryOption.Camera;
					case CameraDefaultEvent e:
						return EventCategoryOption.CameraDefault;
					case CreditEvent e:
						return EventCategoryOption.Credit;
					case EditFlagEvent e:
                        return EventCategoryOption.EditFlag;
                    case EditValueEvent e:
                        return EventCategoryOption.EditValue;
                    case WaitEvent e:
                        return EventCategoryOption.Wait;
                    case EventRunEvent e:
                        return EventCategoryOption.EventRun;
                    case PianoEvent e:
                        return EventCategoryOption.Piano;
                    case PlayerHideEvent e:
                        return EventCategoryOption.PlayerHide;
                    case SEOnEvent e:
                        return EventCategoryOption.SEOn;
                    case ShakeEvent e:
                        return EventCategoryOption.Shake;
                    case ShakeStopEvent e:
                        return EventCategoryOption.ShakeStop;
                    case EffectEvent e:
                        return EventCategoryOption.Effect;
                    case EffectDeleteEvent e:
                        return EventCategoryOption.EffectDelete;
                    case EffectEndEvent e:
                        return EventCategoryOption.EffectEnd;
                    case EventDeathEvent e:
                        return EventCategoryOption.EventDeath;
                    case EventDeleteEvent e:
                        return EventCategoryOption.EventDelete;
                    case FadeEvent e:
                        return EventCategoryOption.Fade;
                    default:
                        throw new InvalidOperationException($"Invalid event.");
                }
            }

            set
            {
                switch (value)
                {
                    case EventCategoryOption.EditItem:
                        this.Instance = new EditItemEvent { ItemNumber = 0, IsAdding = true };
                        break;
                    case EventCategoryOption.ItemGet:
                        this.Instance = new ItemGetEvent { Mystery = Constants.BlankMysteryCreator() };
                        break;
                    case EventCategoryOption.ChipGet:
                        this.Instance = new ChipGetEvent { ChipID = 1, ChipCodeNumber = 0, IsAdding = true };
                        break;
                    case EventCategoryOption.Money:
                        this.Instance = new MoneyEvent { ZennyChange = 0 };
                        break;
                    case EventCategoryOption.HPChange:
                        this.Instance = new HPChangeEvent { HPChange = 0 };
                        break;
                    case EventCategoryOption.Forum:
                        this.Instance = new ForumEvent { ForumNumber = (int)ForumTypeNumber.GensoSquare };
                        break;
                    case EventCategoryOption.Quest:
                        this.Instance = new QuestEvent();
                        break;
                    case EventCategoryOption.Virus:
                        this.Instance = new VirusEvent();
                        break;
                    case EventCategoryOption.Wanted:
                        this.Instance = new WantedEvent();
                        break;
                    case EventCategoryOption.ChipTrader:
                        this.Instance = new ChipTraderEvent { IsSpecial = false };
                        break;
                    case EventCategoryOption.Shop:
                        this.Instance = new ShopEvent { ShopStockIndex = 0, ShopTypeNumber = (int)ShopTypeNumber.Chips, ShopClerkTypeNumber = (int)ShopClerkTypeNumber.Professional, Face = Common.FACE.NaviGreenRobot, ShopItems = new ShopItemCollection() };
                        break;
                    case EventCategoryOption.Interior:
                        this.Instance = new InteriorEvent();
                        break;
                    case EventCategoryOption.Message:
                        this.Instance = new MessageEvent { MessageKey = "Debug.UnimplementedText" };
                        break;
                    case EventCategoryOption.Question:
                        this.Instance = new QuestionEvent { QuestionKey = "Debug.UnimplementedQuestion", AnswerKey = "Debug.UnimplementedAnswer" };
                        break;
                    case EventCategoryOption.NumSet:
                        this.Instance = new NumSetEvent { NumSetKey = "Debug.UnimplementedNumset", TargetVariable = 0, NumberOfDigits = 1 };
                        break;
                    case EventCategoryOption.MessageOpen:
                        this.Instance = new MessageOpenEvent();
                        break;
                    case EventCategoryOption.MessageClose:
                        this.Instance = new MessageCloseEvent();
                        break;
                    case EventCategoryOption.Battle:
                        this.Instance = new BattleEvent();
                        break;
                    case EventCategoryOption.GetMail:
                        this.Instance = new GetMailEvent { MailNumber = 0, IsPlayingEffect = true };
                        break;
                    case EventCategoryOption.GetPhone:
                        this.Instance = new GetPhoneEvent();
                        break;
                    case EventCategoryOption.Special:
                        this.Instance = new SpecialEvent { SpecialEventNumber = (int)SpecialEventTypeNumber.CreateStarterFolder };
                        break;
                    case EventCategoryOption.QuestEnd:
                        this.Instance = new QuestEndEvent();
                        break;
                    case EventCategoryOption.BranchEnd:
                        this.Instance = new BranchEndEvent();
                        break;
                    case EventCategoryOption.BranchHead:
                        this.Instance = new BranchHeadEvent { BranchNumber = 0 };
                        break;
                    case EventCategoryOption.GoToLabel:
                        this.Instance = new GoToLabelEvent { Label = string.Empty };
                        break;
                    case EventCategoryOption.Label:
                        this.Instance = new LabelEvent { Label = string.Empty };
                        break;
                    case EventCategoryOption.IfFlag:
                        this.Instance = new IfFlagEvent { FlagNumber = 0, IsTrue = true, StatementID = 0 };
                        break;
                    case EventCategoryOption.IfValue:
                        this.Instance = new IfValueEvent { VariableLeft = 0, OperatorType = (int)IfValueOperatorTypeNumber.Equals, IsVariable = false, VariableOrConstantRight = 0, StatementID = 0 };
                        break;
                    case EventCategoryOption.IfChip:
                        this.Instance = new IfChipEvent { ChipID = 1, ChipCodeNumber = 0, IsPresent = true, StatementID = 0 };
                        break;
                    case EventCategoryOption.IfEnd:
                        this.Instance = new IfEndEvent { StatementID = 0 };
                        break;
                    case EventCategoryOption.CanSkip:
                        this.Instance = new CanSkipEvent();
                        break;
                    case EventCategoryOption.StopSkip:
                        this.Instance = new StopSkipEvent();
                        break;
                    case EventCategoryOption.EventMove:
                        this.Instance = new EventMoveEvent { IsMapIndex = false, ObjectID = string.Empty, Moves = new MoveCollection() };
                        break;
                    case EventCategoryOption.EventMoveEnd:
                        this.Instance = new EventMoveEndEvent();
                        break;
                    case EventCategoryOption.FaceHere:
                        this.Instance = new FaceScreenEvent();
                        break;
                    case EventCategoryOption.PositionSet:
                        this.Instance = new PositionSetEvent();
                        break;
                    case EventCategoryOption.MapChange:
                        this.Instance = MapChangeEvent.ConvertedFromEvent(this.Instance);
                        break;
                    case EventCategoryOption.MapWarp:
                        this.Instance = MapWarpEvent.ConvertedFromEvent(this.Instance);
                        break;
                    case EventCategoryOption.Warp:
                        this.Instance = WarpEvent.ConvertedFromEvent(this.Instance);
                        break;
                    case EventCategoryOption.WarpPlugOut:
                        this.Instance = new WarpPlugOutEvent();
                        break;
                    case EventCategoryOption.PlugIn:
                        this.Instance = JackInEvent.ConvertedFromEvent(this.Instance);
                        break;
                    case EventCategoryOption.PlugInNO:
                        this.Instance = JackInDirectEvent.ConvertedFromEvent(this.Instance);
                        break;
                    case EventCategoryOption.PlugOut:
                        this.Instance = new JackOutEvent();
                        break;
                    case EventCategoryOption.EditMenu:
                        this.Instance = new EditMenuEvent();
                        break;
                    case EventCategoryOption.StatusHide:
                        this.Instance = new StatusHideEvent { IsHiding = true };
                        break;
                    case EventCategoryOption.LoadBGM:
                        this.Instance = new LoadBGMEvent();
                        break;
                    case EventCategoryOption.SaveBGM:
                        this.Instance = new SaveBGMEvent();
                        break;
                    case EventCategoryOption.FadeBGM:
                        this.Instance = new FadeBGMEvent { EndVolume = 0, FadeTime = 0, IsWaiting = false };
                        break;
                    case EventCategoryOption.EndBGM:
                        this.Instance = new EndBGMEvent();
                        break;
                    case EventCategoryOption.StartBGM:
                        this.Instance = new StartBGMEvent { BGMName = "gen_city" };
                        break;
                    case EventCategoryOption.Camera:
                        this.Instance = new CameraEvent { X = 0, Y = 0, MoveTime = 5, IsAbsolute = true };
                        break;
                    case EventCategoryOption.CameraDefault:
                        this.Instance = new CameraDefaultEvent { MoveTime = 5 };
                        break;
                    case EventCategoryOption.Credit:
                        this.Instance = new CreditEvent { CreditKey = "Debug.UnimplementedText", X = 0, Y = 0, Centered = false, MovesWithCamera = false, FadeInTime = 0, HangTime = 30, FadeOutTime = 0 };
                        break;
                    case EventCategoryOption.EditFlag:
                        this.Instance = new EditFlagEvent { FlagNumber = 0, ValueToSet = true };
                        break;
                    case EventCategoryOption.EditValue:
                        this.Instance = new EditValueEvent { Index = 0, IsVariable = false, Operation = (int)EditValueOperatorTypeNumber.Set, ReferenceType = (int)EditValueReferenceTypeNumber.Value };
                        break;
                    case EventCategoryOption.Wait:
                        this.Instance = new WaitEvent { WaitFrames = 0, IsBlocking = false };
                        break;
                    case EventCategoryOption.EventRun:
                        this.Instance = new EventRunEvent { ID = string.Empty, Page = -1 };
                        break;
                    case EventCategoryOption.PlayerHide:
                        this.Instance = new PlayerHideEvent { IsHiding = true };
                        break;
                    case EventCategoryOption.SEOn:
                        this.Instance = new SEOnEvent { SoundEffect = "alert" };
                        break;
                    case EventCategoryOption.Piano:
                        this.Instance = new PianoEvent { NoteKey = "C", Octave = 6, Volume = 127, FrameDuration = 15 };
                        break;
                    case EventCategoryOption.Shake:
                        this.Instance = new ShakeEvent { Magnitude = 1, DurationFrames = 60 };
                        break;
                    case EventCategoryOption.ShakeStop:
                        this.Instance = new ShakeStopEvent();
                        break;
                    case EventCategoryOption.Effect:
                        this.Instance = new EffectEvent
                        {
                            EffectNumber = (int)EffectTypeNumber.AliceBed,
                            ID = string.Empty,
                            X = 0,
                            Y = 0,
                            Z = 0,
                            LocationType = (int)EffectLocationTypeNumber.Position,
                            Interval = -1,
                            RandomXY = 0,
                            RendType = 0,
                            SoundEffect = "none"
                        };
                        break;
                    case EventCategoryOption.EffectDelete:
                        this.Instance = new EffectDeleteEvent { ID = string.Empty };
                        break;
                    case EventCategoryOption.EffectEnd:
                        this.Instance = new EffectEndEvent();
                        break;
                    case EventCategoryOption.EventDeath:
                        this.Instance = new EventDeathEvent();
                        break;
                    case EventCategoryOption.EventDelete:
                        this.Instance = new EventDeleteEvent { ID = string.Empty };
                        break;
                    case EventCategoryOption.Fade:
                        this.Instance = new FadeEvent { FadeTime = 0, Color = Color.FromArgb(0, 0, 0, 0), IsWaiting = false };
                        break;
                    default:
                        throw new InvalidOperationException($"No event for category {value}.");
                }
                this.OnPropertyChanged(nameof(this.Category));
                this.OnPropertyChanged(nameof(this.StringValue));
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        public virtual string Name
        {
            get
            {
                try
                {
                    return this.Instance.Name;
                }
                catch
                {
                    return "INVALID EVENT";
                }
            }
        }

        public static EventObject FromString(string value)
        {
            var term = new EventObject();
            
            if (value.StartsWith("BranchEnd:", StringComparison.InvariantCulture))
            {
                term.Instance = new BranchEndEvent { StringValue = value };
            }
            else if (value.StartsWith("BranchHead:", StringComparison.InvariantCulture))
            {
                term.Instance = new BranchHeadEvent { StringValue = value };
            }
            else if (value.StartsWith("CameraDefault:", StringComparison.InvariantCulture))
            {
                term.Instance = new CameraDefaultEvent { StringValue = value };
            }
            else if (value.StartsWith("EditItem:", StringComparison.InvariantCulture))
            {
                term.Instance = new EditItemEvent { StringValue = value };
            }
            else if (value.StartsWith("EventLun:", StringComparison.InvariantCulture))
            {
                term.Instance = new EventRunEvent { StringValue = value };
            }
            else if (value.StartsWith("EventLunPara:", StringComparison.InvariantCulture))
            {
                term.Instance = new EventRunEvent { StringValue = value };
            }
            else if (value.StartsWith("Facehere:", StringComparison.InvariantCulture))
            {
                term.Instance = new FaceScreenEvent { StringValue = value };
            }
            else if (value.StartsWith("Forum:", StringComparison.InvariantCulture))
            {
                term.Instance = new ForumEvent { StringValue = value };
            }
            else if (value.StartsWith("GetMail:", StringComparison.InvariantCulture))
            {
                term.Instance = new GetMailEvent { StringValue = value };
            }
            else if (value.StartsWith("BranchHead:", StringComparison.InvariantCulture))
            {
                term.Instance = new BranchHeadEvent { StringValue = value };
            }
            else if (value.StartsWith("Getphone:", StringComparison.InvariantCulture))
            {
                term.Instance = new GetPhoneEvent { StringValue = value };
            }
            else if (value.StartsWith("HP:", StringComparison.InvariantCulture))
            {
                term.Instance = new HPChangeEvent { StringValue = value };
            }
            else if (value.StartsWith("Interior:", StringComparison.InvariantCulture))
            {
                term.Instance = new InteriorEvent { StringValue = value };
            }
            else if (value.StartsWith("ItemGet:", StringComparison.InvariantCulture))
            {
                term.Instance = new ItemGetEvent { StringValue = value };
            }
            else if (value.StartsWith("PlugOut:", StringComparison.InvariantCulture))
            {
                term.Instance = new JackOutEvent { StringValue = value };
            }
            else if (value.StartsWith("PositionSet:", StringComparison.InvariantCulture))
            {
                term.Instance = new PositionSetEvent { StringValue = value };
            }
            else if (value.StartsWith("Quest:", StringComparison.InvariantCulture))
            {
                term.Instance = new QuestEvent { StringValue = value };
            }
            else if (value.StartsWith("QuestEnd:", StringComparison.InvariantCulture))
            {
                term.Instance = new QuestEndEvent { StringValue = value };
            }
            else if (value.StartsWith("Special:", StringComparison.InvariantCulture))
            {
                term.Instance = new SpecialEvent { StringValue = value };
            }
            else if (value.StartsWith("StatusHide:", StringComparison.InvariantCulture))
            {
                term.Instance = new StatusHideEvent { StringValue = value };
            }
            else if (value.StartsWith("Virus:", StringComparison.InvariantCulture))
            {
                term.Instance = new VirusEvent { StringValue = value };
            }
            else if (value.StartsWith("Wanted:", StringComparison.InvariantCulture))
            {
                term.Instance = new WantedEvent { StringValue = value };
            }
            else if (value.StartsWith("battle:", StringComparison.InvariantCulture))
            {
                term.Instance = new BattleEvent { StringValue = value };
            }
            else if (value.StartsWith("bgmLoad:", StringComparison.InvariantCulture))
            {
                term.Instance = new LoadBGMEvent { StringValue = value };
            }
            else if (value.StartsWith("bgmSave:", StringComparison.InvariantCulture))
            {
                term.Instance = new SaveBGMEvent { StringValue = value };
            }
            else if (value.StartsWith("bgmfade:", StringComparison.InvariantCulture))
            {
                term.Instance = new FadeBGMEvent { StringValue = value };
            }
            else if (value.StartsWith("bgmoff:", StringComparison.InvariantCulture))
            {
                term.Instance = new EndBGMEvent { StringValue = value };
            }
            else if (value.StartsWith("bgmon:", StringComparison.InvariantCulture))
            {
                term.Instance = new StartBGMEvent { StringValue = value };
            }
            else if (value.StartsWith("camera:", StringComparison.InvariantCulture))
            {
                term.Instance = new CameraEvent { StringValue = value };
            }
            else if (value.StartsWith("BranchHead:", StringComparison.InvariantCulture))
            {
                term.Instance = new BranchHeadEvent { StringValue = value };
            }
            else if (value.StartsWith("canSkip:", StringComparison.InvariantCulture))
            {
                term.Instance = new CanSkipEvent { StringValue = value };
            }
            else if (value.StartsWith("chipGet:", StringComparison.InvariantCulture))
            {
                term.Instance = new ChipGetEvent { StringValue = value };
            }
            else if (value.StartsWith("chiptreader:", StringComparison.InvariantCulture))
            {
                term.Instance = new ChipTraderEvent { StringValue = value };
            }
            else if (value.StartsWith("credit:", StringComparison.InvariantCulture))
            {
                term.Instance = new CreditEvent { StringValue = value };
            }
            else if (value.StartsWith("editFlag:", StringComparison.InvariantCulture))
            {
                term.Instance = new EditFlagEvent { StringValue = value };
            }
            else if (value.StartsWith("editMenu:", StringComparison.InvariantCulture))
            {
                term.Instance = new EditMenuEvent { StringValue = value };
            }
            else if (value.StartsWith("editValue:", StringComparison.InvariantCulture))
            {
                term.Instance = new EditValueEvent { StringValue = value };
            }
            else if (value.StartsWith("effect:", StringComparison.InvariantCulture))
            {
                term.Instance = new EffectEvent { StringValue = value };
            }
            else if (value.StartsWith("effectDelete:", StringComparison.InvariantCulture))
            {
                term.Instance = new EffectDeleteEvent { StringValue = value };
            }
            else if (value.StartsWith("effectEnd:", StringComparison.InvariantCulture))
            {
                term.Instance = new EffectEndEvent { StringValue = value };
            }
            else if (value.StartsWith("emove:", StringComparison.InvariantCulture))
            {
                term.Instance = new EventMoveEvent { StringValue = value };
            }
            else if (value.StartsWith("emoveEnd:", StringComparison.InvariantCulture))
            {
                term.Instance = new EventMoveEndEvent { StringValue = value };
            }
            else if (value.StartsWith("eventDeath:", StringComparison.InvariantCulture))
            {
                term.Instance = new EventDeathEvent { StringValue = value };
            }
            else if (value.StartsWith("eventDelete:", StringComparison.InvariantCulture))
            {
                term.Instance = new EventDeleteEvent { StringValue = value };
            }
            else if (value.StartsWith("fade:", StringComparison.InvariantCulture))
            {
                term.Instance = new FadeEvent { StringValue = value };
            }
            else if (value.StartsWith("goto:", StringComparison.InvariantCulture))
            {
                term.Instance = new GoToLabelEvent { StringValue = value };
            }
            else if (value.StartsWith("ifEnd:", StringComparison.InvariantCulture))
            {
                term.Instance = new IfEndEvent { StringValue = value };
            }
            else if (value.StartsWith("ifFlag:", StringComparison.InvariantCulture))
            {
                term.Instance = new IfFlagEvent { StringValue = value };
            }
            else if (value.StartsWith("ifValue:", StringComparison.InvariantCulture))
            {
                term.Instance = new IfValueEvent { StringValue = value };
            }
            else if (value.StartsWith("ifchip:", StringComparison.InvariantCulture))
            {
                term.Instance = new IfChipEvent { StringValue = value };
            }
            else if (value.StartsWith("lavel:", StringComparison.InvariantCulture))
            {
                term.Instance = new LabelEvent { StringValue = value };
            }
            else if (value.StartsWith("mapChange:", StringComparison.InvariantCulture))
            {
                term.Instance = new MapChangeEvent { StringValue = value };
            }
            else if (value.StartsWith("mapWarp:", StringComparison.InvariantCulture))
            {
                term.Instance = new MapWarpEvent { StringValue = value };
            }
            else if (value.StartsWith("money:", StringComparison.InvariantCulture))
            {
                term.Instance = new MoneyEvent { StringValue = value };
            }
            else if (value.StartsWith("msg:", StringComparison.InvariantCulture))
            {
                term.Instance = new MessageEvent { StringValue = value };
            }
            else if (value.StartsWith("msgclose:", StringComparison.InvariantCulture))
            {
                term.Instance = new MessageCloseEvent { StringValue = value };
            }
            else if (value.StartsWith("msgopen:", StringComparison.InvariantCulture))
            {
                term.Instance = new MessageOpenEvent { StringValue = value };
            }
            else if (value.StartsWith("numset:", StringComparison.InvariantCulture))
            {
                term.Instance = new NumSetEvent { StringValue = value };
            }
            else if (value.StartsWith("playerHide:", StringComparison.InvariantCulture))
            {
                term.Instance = new PlayerHideEvent { StringValue = value };
            }
            else if (value.StartsWith("plugIn:", StringComparison.InvariantCulture))
            {
                term.Instance = new JackInEvent { StringValue = value };
            }
            else if (value.StartsWith("plugInNO:", StringComparison.InvariantCulture))
            {
                term.Instance = new JackInDirectEvent { StringValue = value };
            }
            else if (value.StartsWith("question:", StringComparison.InvariantCulture))
            {
                term.Instance = new QuestionEvent { StringValue = value };
            }
            else if (value.StartsWith("seon:", StringComparison.InvariantCulture))
            {
                term.Instance = new SEOnEvent { StringValue = value };
            }
            else if (value.StartsWith("piano:", StringComparison.InvariantCulture))
            {
                term.Instance = new PianoEvent { StringValue = value };
            }
            else if (value.StartsWith("shake:", StringComparison.InvariantCulture))
            {
                term.Instance = new ShakeEvent { StringValue = value };
            }
            else if (value.StartsWith("shakeStop:", StringComparison.InvariantCulture))
            {
                term.Instance = new ShakeStopEvent { StringValue = value };
            }
            else if (value.StartsWith("shop:", StringComparison.InvariantCulture))
            {
                term.Instance = new ShopEvent { StringValue = value };
            }
            else if (value.StartsWith("stopSkip:", StringComparison.InvariantCulture))
            {
                term.Instance = new StopSkipEvent { StringValue = value };
            }
            else if (value.StartsWith("wait:", StringComparison.InvariantCulture))
            {
                term.Instance = new WaitEvent { StringValue = value };
            }
            else if (value.StartsWith("warp:", StringComparison.InvariantCulture))
            {
                term.Instance = new WarpEvent { StringValue = value };
            }
            else if (value.StartsWith("warpPlugOut:", StringComparison.InvariantCulture))
            {
                term.Instance = new WarpPlugOutEvent { StringValue = value };
            }
            else
            {
                term.Errors.Add(Tuple.Create(new StringRepresentation[] { term }, $"Invalid event \"{value}\"."));
            }

            return term;
        }

        protected override string GetStringValue() => this.Instance.StringValue;

        protected override void SetStringValue(string value) => this.Instance = EventObject.FromString(value).Instance;

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors() => this.Instance?.Errors ?? new ObservableCollection<Tuple<StringRepresentation[], string>>();

        protected override string GetTypeName() => this.Instance?.TypeName;

        public override void ClearErrors() => this.Instance?.ClearErrors();

        private void OnInstancePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Instance));
            this.OnPropertyChanged(nameof(this.StringValue));
            this.OnPropertyChanged(nameof(this.Name));
        }
    }
}
