using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSEvent;
using NSGame;
using NSMap.Character.Menu;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NSAddOn;

namespace NSMap.Character
{
    public class Player : MapCharacterBase
    {
        public bool[] canmove = new bool[Enum.GetNames(typeof(MapCharacterBase.ANGLE)).Length];
        public int hitLange = 3;
        public int hitEvent = -1;
        public int hitPlug = -1;
        private List<EventManager> encounts = new List<EventManager>();
        public const bool EncountMax = false;
        public const int StopWait = 120;
        public bool stopping;
        public int stoptime;
        public bool openMenu;
        public TopMenu menu;
        private readonly SceneMain main;
        public bool companionMaked;
        public float stepCounter;
        private int encountCounter;
        private bool encount;
        public bool run;
        public InfoMessage info;
        private int infotype;
        private int encountNumber;
        public bool openmystery;
        private int conveyorSound;
        private bool conveyorSoundOn;
        public bool eventhit;
        public bool overStep;
        public SaveData savedata;
        public int encountInterval;
        public const int interval = 300;
        private int hidenumber;
        private bool encounter;
        private bool encounterBreak;

        public void CloseMenu()
        {
            this.openMenu = false;
        }

        public int MoveSpeed
        {
            get
            {
                if (this.savedata.ValList[27] > 0)
                {
                    this.run = true;
                    this.walkanime = 8;
                }
                else
                {
                    this.run = false;
                    this.walkanime = 6;
                }
                return 3 + this.savedata.ValList[27];
            }
        }

        public override Vector3 Position
        {
            get
            {
                return new Vector3(this.position.X - 8f, this.position.Y - 8f, this.position.Z);
            }
            set
            {
                base.Position = new Vector3(value.X + 8f, value.Y + 8f, value.Z);
            }
        }

        public Player(
          IAudioEngine s,
          SceneMap p,
          Point po,
          int floor,
          MapCharacterBase.ANGLE a,
          SceneMain m,
          SaveData save,
          float Z)
          : base(s, p, po, floor, a, null)
        {
            this.position.Z = Z;
            this.main = m;
            this.menu = new TopMenu(s, this, this.main, save);
            this.rendType = 1;
            this.savedata = save;
            this.player = true;
            this.info = new InfoMessage(this.sound, this.savedata);
        }

        public void FieldSet(MapField fi)
        {
            this.field = fi;
        }

        public override void Update()
        {
            if (this.savedata.FlagList[785] && !this.companionMaked)
            {
                this.parent.Field.Events.Add(new CompanionCharacter(this.sound, this.parent, new Point(), 0, this, MapCharacterBase.ANGLE.DOWN, this.savedata));
                this.companionMaked = true;
            }
            if (this.moveOrder.Length == 0)
                this.eventhit = this.parent.CanMove_EventHit(MapCharacterBase.ANGLE.none);
            if (!this.openMenu)
            {
                if (this.moveOrder.Length == 0)
                {
                    int num1 = this.parent.Field.Map_[this.floor, (int)this.Position.X / 8, (int)this.Position.Y / 8];
                    if (this.conveyorSoundOn)
                    {
                        if (this.conveyorSound >= 102)
                        {
                            this.conveyorSoundOn = false;
                            this.conveyorSound = 0;
                        }
                        ++this.conveyorSound;
                    }
                    if (num1 < 14)
                    {
                        if (!this.parent.eventmanager.playevent && this.parent.NoEvent)
                            this.Control();
                        if (this.conveyorSoundOn && this.conveyorSound > 4)
                        {
                            this.sound.StopSE(SoundEffect.conveyor);
                            this.conveyorSoundOn = false;
                            this.conveyorSound = 0;
                        }
                    }
                    else
                    {
                        int num2 = 2;
                        this.animeflame = 0;
                        if (!this.conveyorSoundOn)
                        {
                            this.sound.PlaySE(SoundEffect.conveyor);
                            this.conveyorSoundOn = true;
                            this.conveyorSound = 0;
                        }
                        switch (num1)
                        {
                            case 14:
                                this.Angle = MapCharacterBase.ANGLE.UPRIGHT;
                                this.position.Y -= num2;
                                break;
                            case 15:
                                this.Angle = MapCharacterBase.ANGLE.DOWNLEFT;
                                this.position.Y += num2;
                                break;
                            case 16:
                                this.Angle = MapCharacterBase.ANGLE.UPLEFT;
                                this.position.X -= num2;
                                break;
                            case 17:
                                this.Angle = MapCharacterBase.ANGLE.DOWNRIGHT;
                                this.position.X += num2;
                                break;
                        }
                    }
                }
                else if (this.movingOrder >= this.moveOrder.Length)
                {
                    this.movingOrder = 0;
                    this.moveOrder = new EventMove[0];
                    this.moving = false;
                }
                else
                {
                    this.moveOrder[this.movingOrder].Move(MoveSpeed);
                    if (this.moveOrder[this.movingOrder].MoveEnd())
                        ++this.movingOrder;
                }
                this.FlamePlus();
            }
            else
                this.menu.UpDate();
            this.SaveUpdate();
        }

        private void SaveUpdate()
        {
            this.savedata.nowX = this.position.X;
            this.savedata.nowY = this.position.Y;
            this.savedata.nowZ = this.position.Z;
            this.savedata.nowFroor = this.floor;
            this.savedata.steptype = (int)this.parent.step;
            this.savedata.stepoverX = this.parent.stepover[0];
            this.savedata.stepoverY = this.parent.stepover[1];
            this.savedata.stepCounter = this.stepCounter;
        }

        public void PositionSet(Point po, int f, MapCharacterBase.ANGLE a)
        {
            this.position = new Vector3(po.X, po.Y, 0.0f);
            this.angle = a;
            this.floor = f;
        }

        public MapCharacterBase.ANGLE RebirthAngle()
        {
            int num = (int)(this.angle + 4);
            if (num >= 8)
                num -= 8;
            return (MapCharacterBase.ANGLE)num;
        }

        private void Control()
        {
            if (this.openmystery)
            {
                this.openmystery = false;
                this.savedata.selectQuestion = 1;
                this.field.Events[this.hitEvent].StartEvent();
                this.parent.eventmanager.EventClone(this.field.Events[this.hitEvent].LunPage.eventmanager);
                this.parent.eventmanager.playevent = true;
                this.animeflame = 0;
            }
            else
            {
                this.parent.eventmanager.skipnow = false;
                if (Input.IsPress(Button._Start))
                {
                    if (!this.savedata.FlagList[270])
                    {
                        this.menu.Init();
                        this.openMenu = true;
                        this.sound.PlaySE(SoundEffect.menuopen);
                    }
                    else
                        this.sound.PlaySE(SoundEffect.error);
                }
                else
                {
                    if (Input.IsPush(Button._B) || (uint)this.parent.step > 0U)
                    {
                        this.walkSpeed = 1f;
                        this.animespeed = this.savedata.isJackedIn ? 4 : 6;
                        this.run = true;
                        if (Input.IsPress(Button._B))
                            this.animeflame = 0;
                    }
                    else
                    {
                        this.walkSpeed = 0.5f;
                        this.animespeed = 6;
                        this.run = false;
                        if (Input.IsUp(Button._B))
                            this.animeflame = 0;
                    }
                    int num = this.parent.Field.Map_[this.floor, (int)this.Position.X / 8, (int)this.Position.Y / 8];
                    this.OverMove(num);
                    bool flag = this.MoveKey(num);
                    if (this.savedata.addonSkill[30] && this.savedata.isJackedIn && !this.savedata.FlagList[98])
                        this.MoveKey(num);
                    if (this.encount)
                    {
                        this.Encount();
                        return;
                    }
                    if (Input.IsPress(Button._A) && this.hitEvent >= 0 && this.field.Events[this.hitEvent].LunPage.startterms == EventPage.STARTTERMS.Abutton && !this.savedata.FlagList[272])
                    {
                        this.savedata.selectQuestion = 0;
                        this.field.Events[this.hitEvent].StartEvent();
                        this.field.Events[this.hitEvent].stop = true;
                        this.parent.eventmanager.eventCharacter = this.field.Events[this.hitEvent];
                        this.parent.eventmanager.EventClone(this.field.Events[this.hitEvent].LunPage.eventmanager);
                        this.parent.eventmanager.playevent = true;
                        this.animeflame = 0;
                        return;
                    }
                    if (Input.IsPress(Button._R))
                    {
                        if (!this.savedata.FlagList[271] && !this.savedata.FlagList[0] && !this.savedata.FlagList[785])
                        {
                            if (this.savedata.isJackedIn)
                            {
                                this.parent.eventmanager.EventClone(this.PlugOut());
                                this.parent.eventmanager.playevent = true;
                                this.animeflame = 0;
                                return;
                            }
                            if (this.hitPlug >= 0)
                            {
                                if (this.field.Events[this.hitPlug].LunPage.startterms == EventPage.STARTTERMS.Rbutton)
                                {
                                    this.position = new Vector3((int)this.position.X, (int)this.position.Y, (int)this.position.Z);
                                    this.sound.PlaySE(SoundEffect.pi);
                                    this.angle = MapCharacterBase.ANGLE.DOWN;
                                    this.field.Events[this.hitPlug].StartEvent();
                                    this.parent.eventmanager.EventClone(this.field.Events[this.hitPlug].LunPage.eventmanager);
                                    this.parent.eventmanager.playevent = true;
                                    this.animeflame = 0;
                                    return;
                                }
                            }
                        }
						this.sound.PlaySE(SoundEffect.error);
                    }
                    if (Input.IsPress(Button._L))
                    {
                        if (!this.savedata.FlagList[271])
                        {
                            if (!this.field.masterMake)
                            {
                                this.Angle = MapCharacterBase.ANGLE.DOWN;
                                this.animeflame = 0;
                                if (this.savedata.message == 0)
                                {
                                    if (this.infotype == 1 && this.savedata.ValList[4] <= 0 || this.savedata.FlagList[0] || this.infotype > 1)
                                        this.infotype = 0;
                                    this.parent.eventmanager.EventClone(this.info.GetMessage(this.infotype * 2 + (this.savedata.isJackedIn ? 1 : 0), this.savedata.ValList[3 + this.infotype]));
                                    if (this.infotype == 0)
                                    {
                                        if (this.savedata.ValList[4] > 0)
                                            this.infotype = 1;
                                    }
                                    else
                                        this.infotype = 0;
                                }
                                else
                                {
                                    var bothHumorAndEirinCall = this.savedata.equipAddon.Select((b, i) => b ? this.savedata.haveAddon[i] : null).Count(a => a != null && (a is HumorSense || a is EirinCall)) == 2;
                                    if (bothHumorAndEirinCall)
                                    {
                                        this.savedata.message = 1;
                                        this.infotype = 41;
                                    }

                                    this.parent.eventmanager.EventClone(this.info.GetMessage(this.savedata.message + 3, this.infotype));
                                    ++this.infotype;
                                    switch (this.savedata.message)
                                    {
                                        case 1:
                                            if (this.infotype > 40)
                                            {
                                                this.infotype = 0;
                                                break;
                                            }
                                            break;
                                        case 4:
                                            if ((uint)this.infotype > 0U)
                                            {
                                                for (int infotype = this.infotype; infotype <= 42 && !this.savedata.FlagList[299 + this.infotype]; ++infotype)
                                                    ++this.infotype;
                                            }
                                            if (this.infotype >= 42)
                                            {
                                                this.infotype = 0;
                                                break;
                                            }
                                            break;
                                    }
                                }
                                this.parent.eventmanager.playevent = true;
                                return;
                            }
                            this.sound.PlaySE(SoundEffect.decide);
                            EventManager eventManager = new EventManager(this.parent, this.sound);
                            eventManager.AddEvent(new StatusHide(this.sound, eventManager, true, this.parent, this.savedata));
                            eventManager.AddEvent(new InteriorSetting(this.sound, eventManager, this.parent, this.savedata));
                            eventManager.AddEvent(new StatusHide(this.sound, eventManager, false, this.parent, this.savedata));
                            this.parent.eventmanager.EventClone(eventManager);
                            this.parent.eventmanager.playevent = true;
                            return;
                        }
                        this.sound.PlaySE(SoundEffect.error);
                    }
                    if (flag)
                    {
                        if (this.animeflame == 0)
                            this.animeflame = 1;
                        else if (this.frame % this.animespeed == 0)
                        {
                            ++this.animeflame;
                            if (this.run && !this.savedata.isJackedIn)
                            {
                                if (this.animeflame >= 9)
                                    this.animeflame = 1;
                            }
                            else if (this.animeflame >= 7)
                                this.animeflame = 1;
                        }
                    }
                    else
                        this.animeflame = 0;
                }
            }
        }

        private void OverMove(int p)
        {
            switch (p)
            {
                case 6:
                    this.position.X += 0.25f;
                    this.position.Y -= 0.25f;
                    break;
                case 7:
                    this.position.X -= 0.25f;
                    this.position.Y += 0.25f;
                    break;
                case 8:
                    this.position.X += 0.25f;
                    this.position.Y += 0.25f;
                    break;
                case 9:
                    this.position.X -= 0.25f;
                    this.position.Y -= 0.25f;
                    break;
            }
        }

        private EventManager PlugOut()
        {
            EventManager m = new EventManager(this.sound);
            if (!this.savedata.FlagList[1])
            {
                m.AddEvent(new OpenMassageWindow(this.sound, m));
                var question = ShanghaiEXE.Translate("Player.JackOutQuestion");
                var options = ShanghaiEXE.Translate("Player.JackOutOptions");
                m.AddEvent(new Question(this.sound, m, question[0], options[0], options[1], false, false, true, question.Face, this.savedata, true));
                m.AddEvent(new BranchHead(this.sound, m, 0, this.savedata));
                var dialogue = ShanghaiEXE.Translate("Player.JackOutYesResponse");
                m.AddEvent(new CommandMessage(this.sound, m, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                m.AddEvent(new CloseMassageWindow(this.sound, m));
                m.AddEvent(new Fade(this.sound, m, 20, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedata));
                m.AddEvent(new BGMoff(this.sound, m, 0, this.savedata));
                m.AddEvent(new PlugOut(this.sound, m, this.parent, this.savedata));
                m.AddEvent(new Fade(this.sound, m, 20, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, true, this.savedata));
                m.AddEvent(new BranchHead(this.sound, m, 1, this.savedata));
                m.AddEvent(new CloseMassageWindow(this.sound, m));
                m.AddEvent(new BranchEnd(this.sound, m, this.savedata));
            }
            else
            {
                m.AddEvent(new OpenMassageWindow(this.sound, m));
                var dialogue = ShanghaiEXE.Translate("Player.JackOutFailedDialogue1");
                m.AddEvent(new CommandMessage(this.sound, m, dialogue[0], dialogue[1], dialogue[2], false, dialogue.Face, dialogue.Face.Mono, this.savedata));
                m.AddEvent(new CloseMassageWindow(this.sound, m));
            }
            return m;
        }

        private bool Move(MapCharacterBase.ANGLE a)
        {
            if (!this.parent.CanMove(a, this, this.floor))
                return false;
            if (NSGame.Debug.Encount && !this.savedata.FlagList[271])
                this.EncountCheck((int)a);
            switch (a)
            {
                case MapCharacterBase.ANGLE.DOWN:
                    this.position.X += this.walkSpeed;
                    this.position.Y += this.walkSpeed;
                    if ((uint)this.parent.step > 0U)
                    {
                        this.position.Z += this.walkSpeed;
                        this.stepCounter += this.walkSpeed;
                        break;
                    }
                    break;
                case MapCharacterBase.ANGLE.DOWNRIGHT:
                    this.position.X += this.walkSpeed;
                    if (this.parent.step == SceneMap.STEPS.rightstep)
                    {
                        this.position.Z += this.walkSpeed;
                        this.stepCounter += this.walkSpeed;
                        break;
                    }
                    break;
                case MapCharacterBase.ANGLE.RIGHT:
                    this.position.X += this.walkSpeed / 2f;
                    this.position.Y += (float)(-walkSpeed / 2.0);
                    if (this.parent.step == SceneMap.STEPS.rightstep)
                    {
                        this.position.Z += this.walkSpeed / 2f;
                        this.stepCounter += this.walkSpeed / 2f;
                        if (this.overStep)
                        {
                            this.position.X += this.walkSpeed / 2f;
                            this.position.Y += (float)(-walkSpeed / 2.0);
                            this.position.Z += this.walkSpeed / 2f;
                            this.stepCounter += this.walkSpeed / 2f;
                            break;
                        }
                        break;
                    }
                    if (this.parent.step == SceneMap.STEPS.leftstepp)
                    {
                        this.position.Z -= this.walkSpeed / 2f;
                        this.stepCounter -= this.walkSpeed / 2f;
                        if (this.overStep)
                        {
                            this.position.X += this.walkSpeed / 2f;
                            this.position.Y += (float)(-walkSpeed / 2.0);
                            this.position.Z -= this.walkSpeed / 2f;
                            this.stepCounter -= this.walkSpeed / 2f;
                        }
                        break;
                    }
                    break;
                case MapCharacterBase.ANGLE.UPRIGHT:
                    this.position.Y += -this.walkSpeed;
                    if (this.parent.step == SceneMap.STEPS.leftstepp)
                    {
                        this.position.Z -= this.walkSpeed;
                        this.stepCounter -= this.walkSpeed;
                        break;
                    }
                    break;
                case MapCharacterBase.ANGLE.UP:
                    this.position.X += -this.walkSpeed;
                    this.position.Y += -this.walkSpeed;
                    if ((uint)this.parent.step > 0U)
                    {
                        this.position.Z -= this.walkSpeed;
                        this.stepCounter -= this.walkSpeed;
                        break;
                    }
                    break;
                case MapCharacterBase.ANGLE.UPLEFT:
                    this.position.X += -this.walkSpeed;
                    if (this.parent.step == SceneMap.STEPS.rightstep)
                    {
                        this.position.Z -= this.walkSpeed;
                        this.stepCounter -= this.walkSpeed;
                        break;
                    }
                    break;
                case MapCharacterBase.ANGLE.LEFT:
                    this.position.X += (float)(-walkSpeed / 2.0);
                    this.position.Y += this.walkSpeed / 2f;
                    if (this.parent.step == SceneMap.STEPS.rightstep)
                    {
                        this.position.Z -= this.walkSpeed / 2f;
                        this.stepCounter -= this.walkSpeed / 2f;
                        if (this.overStep)
                        {
                            this.position.X += (float)(-walkSpeed / 2.0);
                            this.position.Y += this.walkSpeed / 2f;
                            this.position.Z -= this.walkSpeed / 2f;
                            this.stepCounter -= this.walkSpeed / 2f;
                            break;
                        }
                        break;
                    }
                    if (this.parent.step == SceneMap.STEPS.leftstepp)
                    {
                        this.position.Z += this.walkSpeed / 2f;
                        this.stepCounter += this.walkSpeed / 2f;
                        if (this.overStep)
                        {
                            this.position.X += (float)(-walkSpeed / 2.0);
                            this.position.Y += this.walkSpeed / 2f;
                            this.position.Z += this.walkSpeed / 2f;
                            this.stepCounter += this.walkSpeed / 2f;
                        }
                        break;
                    }
                    break;
                case MapCharacterBase.ANGLE.DOWNLEFT:
                    this.position.Y += this.walkSpeed;
                    if (this.parent.step == SceneMap.STEPS.leftstepp)
                    {
                        this.position.Z += this.walkSpeed;
                        this.stepCounter += this.walkSpeed;
                        break;
                    }
                    break;
            }
            return true;
        }

        public override void Render(IRenderer dg)
        {
            if (this.NoPrint)
                return;
            this._rect = new Rectangle(animeflame * 32 + (this.savedata.isJackedIn ? 0 : (!this.run || this.animeflame <= 0 ? 224 : 416)), (int)this.angle * 48, 32, 48);
            double num1 = 120.0 - field.CameraPlus.X;
            Point shake = this.Shake;
            double x = shake.X;
            double num2 = num1 + x;
            double num3 = 64.0 - field.CameraPlus.Y;
            shake = this.Shake;
            double y = shake.Y;
            double num4 = num3 + y;
            this._position = new Vector2((float)num2, (float)num4);
            base.Render(dg);
            this._position.Y += jumpY;
            dg.DrawImage(dg, "charachip1", this._rect, false, this._position, Color.White);
        }

        public void OpenMenu()
        {
            this.menu.Init();
            this.openMenu = true;
        }

        private bool MoveKey(int chip)
        {
            bool flag = false;
            if (Input.IsPush(Button.Up) && Input.IsPush(Button.Right))
            {
                flag = true;
                this.angle = MapCharacterBase.ANGLE.UPRIGHT;
                this.eventhit = this.parent.CanMove_EventHit(this.angle);
                if (this.eventhit && (!this.Move(this.Angle) && this.eventhit))
                {
                    switch (chip)
                    {
                        case 3:
                            this.angle = MapCharacterBase.ANGLE.UP;
                            if (!this.Move(this.Angle) && !this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                        case 4:
                            this.angle = MapCharacterBase.ANGLE.RIGHT;
                            if (!this.Move(this.Angle) && !this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                        default:
                            if (!this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                    }
                }
                this.angle = MapCharacterBase.ANGLE.UPRIGHT;
            }
            else if (Input.IsPush(Button.Down) && Input.IsPush(Button.Right))
            {
                flag = true;
                this.angle = MapCharacterBase.ANGLE.DOWNRIGHT;
                this.eventhit = this.parent.CanMove_EventHit(this.angle);
                if (this.eventhit && (!this.Move(this.Angle) && this.eventhit))
                {
                    switch (chip)
                    {
                        case 3:
                            this.angle = MapCharacterBase.ANGLE.DOWN;
                            if (!this.Move(this.Angle) && !this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                        case 5:
                            this.angle = MapCharacterBase.ANGLE.RIGHT;
                            if (!this.Move(this.Angle) && !this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                        default:
                            if (!this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                    }
                }
                this.angle = MapCharacterBase.ANGLE.DOWNRIGHT;
            }
            else if (Input.IsPush(Button.Down) && Input.IsPush(Button.Left))
            {
                flag = true;
                this.angle = MapCharacterBase.ANGLE.DOWNLEFT;
                this.eventhit = this.parent.CanMove_EventHit(this.angle);
                if (this.eventhit && (!this.Move(this.Angle) && this.eventhit))
                {
                    switch (chip)
                    {
                        case 2:
                            this.angle = MapCharacterBase.ANGLE.DOWN;
                            if (!this.Move(this.Angle) && !this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                        case 5:
                            this.angle = MapCharacterBase.ANGLE.LEFT;
                            if (!this.Move(this.Angle) && !this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                        default:
                            if (!this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                    }
                }
                this.angle = MapCharacterBase.ANGLE.DOWNLEFT;
            }
            else if (Input.IsPush(Button.Up) && Input.IsPush(Button.Left))
            {
                flag = true;
                this.angle = MapCharacterBase.ANGLE.UPLEFT;
                this.eventhit = this.parent.CanMove_EventHit(this.angle);
                if (this.eventhit && (!this.Move(this.Angle) && this.eventhit))
                {
                    switch (chip)
                    {
                        case 2:
                            this.angle = MapCharacterBase.ANGLE.UP;
                            if (!this.Move(this.Angle) && !this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                        case 4:
                            this.angle = MapCharacterBase.ANGLE.LEFT;
                            if (!this.Move(this.Angle) && !this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                        default:
                            if (!this.Move(this.Angle - 1))
                            {
                                this.Move(this.Angle + 1);
                                break;
                            }
                            break;
                    }
                }
                this.angle = MapCharacterBase.ANGLE.UPLEFT;
            }
            else if (Input.IsPush(Button.Up))
            {
                flag = true;
                this.angle = MapCharacterBase.ANGLE.UP;
                this.eventhit = this.parent.CanMove_EventHit(this.angle);
                if (this.eventhit && (!this.Move(this.Angle) && this.eventhit && !this.Move(this.Angle - 1)))
                    this.Move(this.Angle + 1);
            }
            else if (Input.IsPush(Button.Right))
            {
                flag = true;
                this.angle = MapCharacterBase.ANGLE.RIGHT;
                this.eventhit = this.parent.CanMove_EventHit(this.angle);
                if (this.eventhit && (!this.Move(this.Angle) && this.eventhit && !this.Move(this.Angle - 1)))
                    this.Move(this.Angle + 1);
            }
            else if (Input.IsPush(Button.Down))
            {
                flag = true;
                this.angle = MapCharacterBase.ANGLE.DOWN;
                this.eventhit = this.parent.CanMove_EventHit(this.angle);
                if (this.eventhit && (!this.Move(this.Angle) && this.eventhit && !this.Move(MapCharacterBase.ANGLE.DOWNLEFT)))
                    this.Move(this.Angle + 1);
            }
            else if (Input.IsPush(Button.Left))
            {
                flag = true;
                this.angle = MapCharacterBase.ANGLE.LEFT;
                this.eventhit = this.parent.CanMove_EventHit(this.angle);
                if (this.eventhit && !this.Move(this.Angle) && !this.Move(this.Angle - 1))
                    this.Move(this.Angle + 1);
            }
            return flag;
        }

        private bool SubChipCount()
        {
            bool flag = true;
            if (this.savedata.runSubChips[0])
            {
                if (this.savedata.ValList[16] > 0)
                    --this.savedata.ValList[16];
                else if (flag)
                {
                    this.savedata.runSubChips[0] = false;
                    this.animeflame = 0;
                    flag = false;
                    this.parent.eventmanager.events.Clear();
                    this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
                    var dialogue = ShanghaiEXE.Translate("Player.FirewallFadeDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
                }
            }
            if (this.savedata.runSubChips[1])
            {
                if (this.savedata.ValList[17] > 0)
                    --this.savedata.ValList[17];
                else if (flag)
                {
                    this.savedata.runSubChips[1] = false;
                    this.animeflame = 0;
                    flag = false;
                    this.parent.eventmanager.events.Clear();
                    this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
                    var dialogue = ShanghaiEXE.Translate("Player.OpenPortFadeDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
                }
            }
            if (this.savedata.runSubChips[3])
            {
                if (this.savedata.ValList[18] > 0)
                    --this.savedata.ValList[18];
                else if (flag)
                {
                    this.savedata.runSubChips[3] = false;
                    this.animeflame = 0;
                    flag = false;
                    this.parent.eventmanager.events.Clear();
                    this.parent.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.parent.eventmanager));
                    var dialogue = ShanghaiEXE.Translate("Player.VirusScnFadeDialogue1");
                    this.parent.eventmanager.AddEvent(new CommandMessage(this.sound, this.parent.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    this.parent.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.parent.eventmanager));
                }
            }
            return flag;
        }

        private void EncountCheck(int plus, bool ignoreSpecialEncounters = false)
        {
            this.hidenumber = this.field.encountCap[1];
            // If subchip message is shown, or no encounters (after handling special), return
            // Always evaluate SubChipCount, has side-effect of decrementing timers
            if (!this.SubChipCount() || (this.field.encounts.Count - (this.savedata.FlagList[this.field.encountCap[0]] ? 0 : this.hidenumber) <= 0))
                return;
            if (this.encountInterval <= 0)
            {
                this.encountCounter += plus * this.Random.Next(10);
                if (NSGame.Debug.EncountMax || this.savedata.runSubChips[1] || this.encounter)
                    this.encountCounter = 1021;
                if (!this.savedata.runSubChips[1] && this.encountCounter % 1024 == 1021
                    || (this.encountCounter % 1024 == 872 || this.encountCounter % 1024 == 234)
                    || this.encountCounter % 1024 == 662
                    || (this.savedata.runSubChips[1] && this.encountCounter % 1024 == 1021
                    || (this.encountCounter % 1024 == 872 || this.encountCounter % 1024 == 234)
                    || (this.encountCounter % 1024 == 662 || this.encountCounter % 1024 == 326 || this.encountCounter % 1024 == 234)
                    || this.encountCounter % 1024 == 448))
                {
                    if (this.Random.Next(100) < 75 || this.savedata.runSubChips[1] || this.encounter)
                    {
                        this.encounter = false;
                        if (this.savedata.runSubChips[3])
                        {
                            this.encounts = new List<EventManager>(this.field.encounts);
                            this.encountNumber = this.savedata.ValList[19];
                        }
                        else
                        {
                            int count = this.field.encounts.Count;
                            if (!this.savedata.FlagList[this.field.encountCap[0]]
                                || (ignoreSpecialEncounters && this.field.encountCap[1] != this.field.encounts.Count))
                            {
                                this.encounts = new List<EventManager>();
                                for (int index = 0; index < this.field.encounts.Count - this.field.encountCap[1]; ++index)
                                    this.encounts.Add(this.field.encounts[index]);
                            }
                            else
                            {
                                this.encounts = new List<EventManager>(field.encounts);
                            }
                            if (this.savedata.addonSkill[7])
                                this.encounts = this.Element(ChipBase.ELEMENT.heat);
                            else if (this.savedata.addonSkill[8])
                                this.encounts = this.Element(ChipBase.ELEMENT.aqua);
                            else if (this.savedata.addonSkill[9])
                                this.encounts = this.Element(ChipBase.ELEMENT.leaf);
                            else if (this.savedata.addonSkill[10])
                                this.encounts = this.Element(ChipBase.ELEMENT.eleki);
                            else if (this.savedata.addonSkill[11])
                                this.encounts = this.Element(ChipBase.ELEMENT.poison);
                            else if (this.savedata.addonSkill[12])
                                this.encounts = this.Element(ChipBase.ELEMENT.earth);
                            this.encountNumber = this.Random.Next(this.encounts.Count);
                        }
                        this.encountInterval = 300;
                        if ((!this.savedata.runSubChips[0] || this.IsBypassingFirewall(this.encountNumber) || this.encounterBreak) && this.encounts.Count > this.encountNumber)
                        {
                            this.encounterBreak = false;
                            this.encount = true;
                            this.savedata.ValList[19] = this.field.encounts.IndexOf(this.encounts[this.encountNumber]);
                        }
                    }
                }
                if (this.encountCounter > 300000)
                    this.encountCounter = 0;
            }
            else
                --this.encountInterval;
        }

        private void Encount()
        {
            this.parent.eventmanager.EventClone(this.encounts[this.encountNumber]);
            this.parent.eventmanager.playevent = true;
            this.encountCounter = 0;
            this.encount = false;
            this.parent.battleflag = true;
        }

        public void EncountSet(bool ignoreSpecialEncounters)
        {
            this.encounter = true;
            this.encounterBreak = true;
            this.encountInterval = 0;
            this.EncountCheck(0, ignoreSpecialEncounters);
        }

        private bool IsBypassingFirewall(int number)
        {
            var totalEnemyHp = 0;
            if (!(this.field.encounts[number].events[1] is Battle))
                return false;
            var battle = (Battle)this.field.encounts[number].events[1];
            var strongEnemyCheck = false;
            for (int index = 0; index < 3; ++index)
            {
                EnemyBase e = EnemyBase.EnemyMake((int)battle.enemy[index], null, true);
                if (e != null)
                {
                    e.version = battle.lank[index];
                    EnemyBase enemyBase = EnemyBase.EnemyMake((int)battle.enemy[index], e, true);
                    totalEnemyHp += enemyBase.HpMax;

                    if (this.savedata.runSubChips[1] && (e is NaviBase || e.version == 0))
                    {
                        strongEnemyCheck = true;
                    }
                }
            }
            var maxHpCheck = this.savedata.HPMax + 100 < totalEnemyHp;


            return maxHpCheck || strongEnemyCheck;
        }

        private List<EventManager> Element(ChipBase.ELEMENT element)
        {
            List<EventManager> eventManagerList1 = new List<EventManager>();
            List<EventManager> eventManagerList2 = new List<EventManager>(encounts);
            int num = -1;
            for (int index1 = eventManagerList2.Count - 1; index1 >= 0; --index1)
            {
                ++num;
                bool flag = true;
                if (eventManagerList2[index1].events[1] is Battle battle)
                {
                    for (int index2 = 0; index2 < 3; ++index2)
                    {
                        EnemyBase enemyBase = EnemyBase.EnemyMake((int)battle.enemy[index2], null, true);
                        if (enemyBase == null)
                        {
                            continue;
                        }

                        enemyBase.version = battle.lank[index2];
                        var enemyVersion = EnemyBase.EnemyMake((int)battle.enemy[index2], enemyBase, true);
                        if (enemyVersion.Element == element)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    if (num < this.field.encountCap[1])
                        --this.hidenumber;
                    eventManagerList2.RemoveAt(index1);
                }
            }
            if (eventManagerList2.Count <= 0)
                return this.encounts;
            return eventManagerList2;
        }

        public override float RendSetter()
        {
            return this.Position.X + this.Position.Y;
        }
    }
}
