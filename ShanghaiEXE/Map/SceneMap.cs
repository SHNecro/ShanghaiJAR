using NSAddOn;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSGame;
using NSMap.Character;
using Common.Vectors;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace NSMap
{
    public class SceneMap : SceneBase
    {
        public int loadflame = 120;
        public Color fadeColor = new Color();
        public int[] stepover = new int[2];
        public byte battlecouunt = 0;
        public float alpha = 0.0f;
        public bool outoCamera = true;
        private byte endAlpha = 0;
        private byte endR = 0;
        private byte endG = 0;
        private byte endB = 0;
        public bool battleflag;
        public SceneMap.STEPS step;
        public SceneMain main;
        public EventManager eventmanager;
        public EventManager eventmanagerParallel;
        private NSMap.Character.Player player;
        private MapField field;
        public HPGauge HP;
        private byte phoneAnime;
        public DebugMode debugmode;
        public bool setCameraOn;
        public Vector3 setCamera;
        public Vector2 cameraPlus;
        public bool hideStatus;
        private bool mailsound;
        private int mailflame;
        private int mailloop;
        private bool mailflag;
        private bool fadeflug;
        private float R;
        private float G;
        private float B;
        private float plusAlpha;
        private float plusR;
        private float plusG;
        private float plusB;
        private int fadeFlame;
        private int fadeTime;
		internal List<IPersistentEvent> persistentEvents;

        public bool NoEvent
        {
            get
            {
                foreach (MapEventBase mapEventBase in this.field.Events)
                {
                    if (mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Auto)
                        return false;
                }
                return true;
            }
        }

        public int MapsizeX
        {
            get
            {
                return this.field.MapsizeX;
            }
        }

        public int MapsizeY
        {
            get
            {
                return this.field.MapsizeY;
            }
        }

        public NSMap.Character.Player Player
        {
            get
            {
                return this.player;
            }
        }

        public MapField Field
        {
            get
            {
                return this.field;
            }
        }

        public byte PhoneAnime
        {
            get
            {
                return this.phoneAnime;
            }
            set
            {
                this.phoneAnime = value;
                if (this.phoneAnime < 4)
                    return;
                this.phoneAnime = 0;
            }
        }

        private bool DebugOn
        {
            get
            {
                return this.debugmode != null && this.debugmode.menuOn;
            }
        }

        public SceneMap(IAudioEngine s, ShanghaiEXE p, SceneMain m, EventManager e, SaveData save)
          : base(s, p, save)
        {
            this.main = m;
            this.eventmanager = new EventManager(this, this.sound);
            this.eventmanagerParallel = new EventManager(this, this.sound);
            this.HP = new HPGauge(this.sound, this.savedata.HPNow, this.savedata.HPMax);
			this.persistentEvents = new List<IPersistentEvent>();
        }

        public void NewGame(int plus)
        {
            this.player = new NSMap.Character.Player(this.sound, this, new Point(32, 32), 0, MapCharacterBase.ANGLE.DOWN, this.main, this.savedata, 0.0f);
            this.field = new MapField(this.sound, "aliceroom", this.savedata, this);
            this.savedata.isJackedIn = false;
            if (plus >= 0)
            {
                this.savedata.ValList[28] = plus;
            }
            else
            {
                switch (plus)
                {
                    case -2:
                        this.savedata.GetAddon(new OwataManBody(AddOnBase.ProgramColor.dark));
                        break;
                    case -1:
                        this.savedata.GetAddon(new Haisui(AddOnBase.ProgramColor.dark));
                        this.savedata.GetAddon(new RShield(AddOnBase.ProgramColor.red));
                        this.savedata.GetAddon(new LBeastRock(AddOnBase.ProgramColor.gleen));
                        break;
                }
            }
            this.player.FieldSet(this.field);
            this.fadeColor = Color.Black;
            this.alpha = byte.MaxValue;
            this.player.NoPrint = true;
        }

        public void LoadGame()
        {
            this.player = new NSMap.Character.Player(this.sound, this, new Point((int)this.savedata.nowX, (int)this.savedata.nowY), this.savedata.nowFroor, MapCharacterBase.ANGLE.DOWN, this.main, this.savedata, this.savedata.nowZ);
            this.step = (SceneMap.STEPS)this.savedata.steptype;
            this.stepover[0] = this.savedata.stepoverX;
            this.stepover[1] = this.savedata.stepoverY;
            this.player.stepCounter = this.savedata.stepCounter;
			this.persistentEvents.Clear();
			this.field = new MapField(this.sound, this.savedata.nowMap, this.savedata, this);
			this.player.FieldSet(this.field);
            this.fadeColor = Color.Black;
            this.alpha = byte.MaxValue;
			this.FadeStart(Color.FromArgb(0, this.fadeColor), 5);
        }

        public void FieldSet(string name, Point posi, int floor, MapCharacterBase.ANGLE angle)
        {
			this.persistentEvents.Clear();
            this.field = new MapField(this.sound, name, this.savedata, this);
            this.player.PositionSet(posi, floor, angle);
            this.player.position.Z = this.savedata.pluginZ;
            this.player.field = this.field;
        }

        public override void Updata()
        {
            if (!this.DebugOn)
            {
                if (this.outoCamera)
                    this.CameraMove();
                this.Player.hitEvent = -1;
                this.Player.hitPlug = -1;
                if (this.mailflag)
                    this.MailAnime();
                if (this.ShakeFlag)
                    this.Shaking();
                if (this.fadeflug)
                    this.Fadeing();
                if (this.eventmanager.playevent)
                    this.eventmanager.UpDate();
                else if (this.eventmanagerParallel.playevent && !this.player.openMenu)
                    this.eventmanagerParallel.UpDate();
                if (!this.player.openMenu && !this.eventmanager.playevent)
                    this.TimerUpdate();
                // Temporary copy made in case a persistent event (runevent) adds another persistent event, will need to start next tick
				foreach (var persistentEvent in this.persistentEvents.ToArray())
				{
					if (persistentEvent.IsActive)
					{
						persistentEvent.PersistentUpdate();
					}
				}
                this.persistentEvents.RemoveAll(pe => !pe.IsActive);
                this.MapUpdate();
                this.HP.HPDown(this.savedata.HPNow, this.savedata.HPMax);
                this.field.Update();
            }
            else
                this.debugmode.Update();
        }

        private void TimerUpdate()
        {
            if (!this.savedata.FlagList[4] || this.savedata.FlagList[69])
                return;
            if (this.savedata.ValList[40] < this.savedata.ValList[41])
            {
                ++this.savedata.ValList[40];
                if (this.savedata.ValList[40] % 100 >= 60)
                    this.savedata.ValList[40] += 40;
                if (this.savedata.ValList[40] % 10000 >= 6000)
                    this.savedata.ValList[40] += 4000;
            }
            else if (this.savedata.ValList[40] > this.savedata.ValList[41])
            {
                int num1 = this.savedata.ValList[40] / 10000;
                int num2 = this.savedata.ValList[40] % 10000 / 100;
                int num3 = this.savedata.ValList[40] % 100 - 1;
                if (num3 < 0)
                {
                    num3 = 59;
                    --num2;
                }
                if (num2 < 0)
                {
                    num2 = 59;
                    --num1;
                }
                this.savedata.ValList[40] = num1 * 10000 + num2 * 100 + num3;
            }
            else if (this.savedata.ValList[40] == this.savedata.ValList[41])
            {
                this.sound.PlaySE(SoundEffect.rockon);
                this.savedata.FlagList[69] = true;
            }
        }

        private void MapUpdate()
        {
            this.player.Update();
        }

        public bool CanMove(MapCharacterBase.ANGLE angle, NSMap.Character.Player player, int floor)
        {
            Vector3 position = player.Position;
            player.overStep = false;
            bool flag1 = true;
            if (this.step == SceneMap.STEPS.normal)
            {
                int num1 = 0;
                int num2 = 0;
                int num3 = 3;
                switch (angle)
                {
                    case MapCharacterBase.ANGLE.DOWN:
                        num1 += num3;
                        num2 += num3;
                        break;
                    case MapCharacterBase.ANGLE.DOWNRIGHT:
                        num1 += num3;
                        break;
                    case MapCharacterBase.ANGLE.RIGHT:
                        num1 += num3;
                        num2 += -num3;
                        break;
                    case MapCharacterBase.ANGLE.UPRIGHT:
                        num2 += -num3;
                        break;
                    case MapCharacterBase.ANGLE.UP:
                        num1 += -num3;
                        num2 += -num3;
                        break;
                    case MapCharacterBase.ANGLE.UPLEFT:
                        num1 += -num3;
                        break;
                    case MapCharacterBase.ANGLE.LEFT:
                        num1 += -num3;
                        num2 += num3;
                        break;
                    case MapCharacterBase.ANGLE.DOWNLEFT:
                        num2 += num3;
                        break;
                }
                Point point1 = new Point((int)position.X / 8, (int)position.Y / 8);
                Point point2 = new Point((int)position.X % 8 + num1, (int)position.Y % 8 + num2);
                bool flag2 = false;
                if (point2.X < 0)
                {
                    if (point1.X - 1 < 0)
                        flag1 = false;
                    else if (this.field.Map_[floor, point1.X - 1, point1.Y] == 0 || this.field.Map_[floor, point1.X - 1, point1.Y] == 3 || this.field.Map_[floor, point1.X - 1, point1.Y] == 5 || this.field.Map_[floor, point1.X - 1, point1.Y] >= 6 && this.field.Map_[floor, point1.X - 1, point1.Y] <= 9)
                    {
                        flag1 = false;
                        if (this.field.Map_[floor, point1.X, point1.Y] == 2 && this.field.Map_[floor, point1.X - 1, point1.Y] == 6 && angle == MapCharacterBase.ANGLE.UP)
                            flag1 = true;
                        if (this.field.Map_[floor, point1.X, point1.Y] == 4 && this.field.Map_[floor, point1.X - 1, point1.Y] == 8 && angle == MapCharacterBase.ANGLE.LEFT)
                            flag1 = true;
                    }
                    flag2 = true;
                }
                else if (point2.X >= 8)
                {
                    if (point1.X + 1 >= this.field.Map_.GetLength(1) || this.field.Map_[floor, point1.X + 1, point1.Y] == 0 || (this.field.Map_[floor, point1.X + 1, point1.Y] == 2 || this.field.Map_[floor, point1.X + 1, point1.Y] == 4) || this.field.Map_[floor, point1.X + 1, point1.Y] >= 6 && this.field.Map_[floor, point1.X + 1, point1.Y] <= 9)
                        flag1 = false;
                    if (this.field.Map_[floor, point1.X, point1.Y] == 3 && this.field.Map_[floor, point1.X + 1, point1.Y] == 7 && angle == MapCharacterBase.ANGLE.DOWN)
                        flag1 = true;
                    if (this.field.Map_[floor, point1.X, point1.Y] == 5 && this.field.Map_[floor, point1.X + 1, point1.Y] == 9 && angle == MapCharacterBase.ANGLE.RIGHT)
                        flag1 = true;
                    flag2 = true;
                }
                if (point2.Y < 0)
                {
                    if (point1.Y - 1 < 0)
                        flag1 = false;
                    else if (this.field.Map_[floor, point1.X, point1.Y - 1] == 0 || this.field.Map_[floor, point1.X, point1.Y - 1] == 2 || this.field.Map_[floor, point1.X, point1.Y - 1] == 5 || this.field.Map_[floor, point1.X, point1.Y - 1] >= 6 && this.field.Map_[floor, point1.X, point1.Y - 1] <= 9)
                    {
                        flag1 = false;
                        if (this.field.Map_[floor, point1.X, point1.Y] == 3 && this.field.Map_[floor, point1.X, point1.Y - 1] == 7 && angle == MapCharacterBase.ANGLE.UP)
                            flag1 = true;
                        if (this.field.Map_[floor, point1.X, point1.Y] == 4 && this.field.Map_[floor, point1.X, point1.Y - 1] == 8 && angle == MapCharacterBase.ANGLE.RIGHT)
                            flag1 = true;
                    }
                    flag2 = true;
                }
                else if (point2.Y >= 8)
                {
                    if (point1.Y + 1 >= this.field.Map_.GetLength(2) || this.field.Map_[floor, point1.X, point1.Y + 1] == 0 || (this.field.Map_[floor, point1.X, point1.Y + 1] == 3 || this.field.Map_[floor, point1.X, point1.Y + 1] == 4) || this.field.Map_[floor, point1.X, point1.Y + 1] >= 6 && this.field.Map_[floor, point1.X, point1.Y + 1] <= 9)
                        flag1 = false;
                    if (this.field.Map_[floor, point1.X, point1.Y] == 2 && this.field.Map_[floor, point1.X, point1.Y + 1] == 6 && angle == MapCharacterBase.ANGLE.DOWN)
                        flag1 = true;
                    if (this.field.Map_[floor, point1.X, point1.Y] == 5 && this.field.Map_[floor, point1.X, point1.Y + 1] == 9 && angle == MapCharacterBase.ANGLE.LEFT)
                        flag1 = true;
                    flag2 = true;
                }
                if (!flag2)
                {
                    switch (this.field.Map_[floor, point1.X, point1.Y])
                    {
                        case 0:
                            flag1 = false;
                            break;
                        case 2:
                            if (point2.X * point2.Y < point2.Y * point2.Y)
                            {
                                flag1 = false;
                                break;
                            }
                            break;
                        case 3:
                            if (point2.X * point2.Y < point2.X * point2.X)
                            {
                                flag1 = false;
                                break;
                            }
                            break;
                        case 4:
                            if (point2.X * (7 - point2.Y) < (7 - point2.Y) * (7 - point2.Y))
                            {
                                flag1 = false;
                                break;
                            }
                            break;
                        case 5:
                            if (point2.X * (7 - point2.Y) < point2.X * point2.X)
                            {
                                flag1 = false;
                                break;
                            }
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            flag1 = false;
                            break;
                    }
                }
                if (flag1)
                {
                    Point point3 = new Point((int)player.position.X + num1, (int)player.position.Y + num2);
                    int num4 = -1;
                    foreach (MapEventBase mapEventBase in this.field.Events)
                    {
                        ++num4;
                        if (mapEventBase.LunPage.hitform
							&& mapEventBase.floor == player.floor
							&& mapEventBase.rendType == 1
							&& (point3.X >= mapEventBase.position.X + (double)mapEventBase.LunPage.hitShift.X - mapEventBase.LunPage.hitrange.X / 2
							&& point3.X <= mapEventBase.position.X + (double)mapEventBase.LunPage.hitShift.X + mapEventBase.LunPage.hitrange.X / 2
							&& point3.Y >= mapEventBase.position.Y + (double)mapEventBase.LunPage.hitShift.Y - mapEventBase.LunPage.hitrange.Y / 2
							&& point3.Y <= mapEventBase.position.Y + (double)mapEventBase.LunPage.hitShift.Y + mapEventBase.LunPage.hitrange.Y / 2))
                        {
                            flag1 = false;
                            if (mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Touch)
                                player.hitEvent = num4;
                        }
                    }
                }
                int num5 = this.field.Map_[floor, point1.X, point1.Y];
                int num6 = 1;
                switch (num5)
                {
                    case 10:
                    case 12:
                        this.step = SceneMap.STEPS.rightstep;
                        player.stepCounter = num5 != 10 ? this.field.Height / 2 : 0.0f;
                        Point point4 = new Point(point1.X, point1.Y);
                        int num7;
                        do
                        {
                            --point4.Y;
                            num7 = this.field.Map_[floor, point4.X, point4.Y];
                            if (num7 == num5)
                            {
                                ++num6;
                            }
                            else
                            {
                                ++point4.Y;
                                this.stepover[0] = point4.Y * 8;
                            }
                        }
                        while (num7 == num5);
                        point4 = new Point(point1.X, point1.Y);
                        int num8;
                        do
                        {
                            ++point4.Y;
                            num8 = this.field.Map_[floor, point4.X, point4.Y];
                            if (num8 == num5)
                            {
                                ++num6;
                            }
                            else
                            {
                                this.stepover[1] = this.stepover[0] + num6 * 8;
                                this.stepover[0] += 3;
                                this.stepover[1] -= 3;
                            }
                        }
                        while (num8 == num5);
                        break;
                    case 11:
                    case 13:
                        this.step = SceneMap.STEPS.leftstepp;
                        player.stepCounter = num5 != 11 ? this.field.Height / 2 : 0.0f;
                        Point point5 = new Point(point1.X, point1.Y);
                        int num9;
                        do
                        {
                            --point5.X;
                            num9 = this.field.Map_[floor, point5.X, point5.Y];
                            if (num9 == num5)
                            {
                                ++num6;
                            }
                            else
                            {
                                ++point5.X;
                                this.stepover[0] = point5.X * 8;
                            }
                        }
                        while (num9 == num5);
                        point5 = new Point(point1.X, point1.Y);
                        int num10;
                        do
                        {
                            ++point5.X;
                            num10 = this.field.Map_[floor, point5.X, point5.Y];
                            if (num10 == num5)
                            {
                                ++num6;
                            }
                            else
                            {
                                this.stepover[1] = this.stepover[0] + num6 * 8;
                                this.stepover[0] += 3;
                                this.stepover[1] -= 3;
                            }
                        }
                        while (num10 == num5);
                        break;
                    default:
                        this.step = SceneMap.STEPS.normal;
                        break;
                }
                if (this.field.Height > 1)
                    player.floor = (int)player.Position.Z / (this.field.Height / 2);
                else
                    player.floor = 0;
            }
            else
            {
                this.StepOut();
                int num1 = (int)player.Position.Z / (this.field.Height / 2 - 1);
                int num2 = 0;
                int num3 = 0;
                int num4 = 1;
                switch (angle)
                {
                    case MapCharacterBase.ANGLE.DOWN:
                        num2 += num4;
                        num3 += num4;
                        break;
                    case MapCharacterBase.ANGLE.DOWNRIGHT:
                        num2 += num4;
                        break;
                    case MapCharacterBase.ANGLE.RIGHT:
                        num2 += num4;
                        num3 += -num4;
                        break;
                    case MapCharacterBase.ANGLE.UPRIGHT:
                        num3 += -num4;
                        break;
                    case MapCharacterBase.ANGLE.UP:
                        num2 += -num4;
                        num3 += -num4;
                        break;
                    case MapCharacterBase.ANGLE.UPLEFT:
                        num2 += -num4;
                        break;
                    case MapCharacterBase.ANGLE.LEFT:
                        num2 += -num4;
                        num3 += num4;
                        break;
                    case MapCharacterBase.ANGLE.DOWNLEFT:
                        num3 += num4;
                        break;
                }
                Point point = new Point((int)(position.X + (double)num2) / 8, (int)(position.Y + (double)num3) / 8);
                player.floor = num1;
                if (player.stepCounter < 0.0 || player.stepCounter > (double)(this.field.Height / 2))
                {
                    if ((uint)player.floor > 0U)
                        player.position.Z = this.field.Height / 2 * player.floor;
                    else
                        player.position.Z = 0.0f;
                    this.step = SceneMap.STEPS.normal;
                }
            }
            return flag1;
        }

        public bool CanMove_EventHit(MapCharacterBase.ANGLE a)
        {
            this.player.stopping = false;
            bool flag1 = true;
            Vector2 vector2 = new Vector2();
            switch (a)
            {
                case MapCharacterBase.ANGLE.DOWN:
                    vector2.X += this.player.Speed;
                    vector2.Y += this.player.Speed;
                    break;
                case MapCharacterBase.ANGLE.DOWNRIGHT:
                    vector2.X += this.player.Speed;
                    break;
                case MapCharacterBase.ANGLE.RIGHT:
                    vector2.X += this.player.Speed / 2f;
                    vector2.Y -= this.player.Speed / 2f;
                    break;
                case MapCharacterBase.ANGLE.UPRIGHT:
                    vector2.Y -= this.player.Speed;
                    break;
                case MapCharacterBase.ANGLE.UP:
                    vector2.X -= this.player.Speed;
                    vector2.Y -= this.player.Speed;
                    break;
                case MapCharacterBase.ANGLE.UPLEFT:
                    vector2.X -= this.player.Speed;
                    break;
                case MapCharacterBase.ANGLE.LEFT:
                    vector2.X -= this.player.Speed / 2f;
                    vector2.Y += this.player.Speed / 2f;
                    break;
                case MapCharacterBase.ANGLE.DOWNLEFT:
                    vector2.Y += this.player.Speed;
                    break;
            }
            int num1 = -1;
            foreach (MapEventBase mapEventBase in this.field.Events)
            {
                ++num1;
                if (mapEventBase.floor == this.player.floor && (mapEventBase.LunPage.hitrange.X != 0 || mapEventBase.LunPage.hitrange.Y != 0))
                {
                    bool isColliding = false;
                    Vector3 vector3_1 = new Vector3();
                    vector3_1 = !mapEventBase.LunPage.character ? this.player.position : this.player.Position;
                    vector3_1.X += vector2.X;
                    vector3_1.Y += vector2.Y;
                    // circle collision check (walkthrough, handling later)
                    if (!mapEventBase.LunPage.hitform)
                    {
                        float num2 = MyMath.Pow(mapEventBase.position.X + mapEventBase.LunPage.hitShift.X - vector3_1.X, 2)
                                   + MyMath.Pow(mapEventBase.position.Y + mapEventBase.LunPage.hitShift.Y - vector3_1.Y, 2);
                        isColliding = MyMath.Pow(mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Touch
							                ? mapEventBase.LunPage.hitrange.X + 2
							                : mapEventBase.LunPage.hitrange.X + (a != MapCharacterBase.ANGLE.none
																	                ? this.player.hitLange
																	                : this.player.hitLange + 1),
                                            2)
								>= (double)num2;
                        // disable collision for 0-size circles
                        isColliding &= mapEventBase.LunPage.hitrange.X != 0;
                    }
                    else if (mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Abutton || mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Rbutton)
                    {
                        if ((uint)mapEventBase.LunPage.move.Length > 0U)
                        {
                            int num2 = 0;
                            int num3 = 0;
                            int num4 = 0;
                            int num5 = 0;
                            switch (mapEventBase.LunPage.angle)
                            {
                                case MapCharacterBase.ANGLE.DOWNRIGHT:
                                    num2 = 16;
                                    break;
                                case MapCharacterBase.ANGLE.UPRIGHT:
                                    num5 = 16;
                                    break;
                                case MapCharacterBase.ANGLE.UPLEFT:
                                    num3 = 16;
                                    break;
                                case MapCharacterBase.ANGLE.DOWNLEFT:
                                    num4 = 16;
                                    break;
                            }
                            int num6 = this.player.hitLange + 3;
                            isColliding = vector3_1.X >= mapEventBase.position.X + (double)mapEventBase.LunPage.hitShift.X - mapEventBase.LunPage.hitrange.X / 2 - num6 - num3
								&& vector3_1.X <= mapEventBase.position.X + (double)mapEventBase.LunPage.hitShift.X + mapEventBase.LunPage.hitrange.X / 2 + num6 + num2
								&& vector3_1.Y >= mapEventBase.position.Y + (double)mapEventBase.LunPage.hitShift.Y - mapEventBase.LunPage.hitrange.Y / 2 - num6 - num5
								&& vector3_1.Y <= mapEventBase.position.Y + (double)mapEventBase.LunPage.hitShift.Y + mapEventBase.LunPage.hitrange.Y / 2 + num6 + num4;
                        }
                        else
                        {
                            int num2 = this.player.hitLange + 3;
                            isColliding = vector3_1.X >= mapEventBase.position.X + (double)mapEventBase.LunPage.hitShift.X - mapEventBase.LunPage.hitrange.X / 2 - num2
								&& vector3_1.X <= mapEventBase.position.X + (double)mapEventBase.LunPage.hitShift.X + mapEventBase.LunPage.hitrange.X / 2 + num2
								&& vector3_1.Y >= mapEventBase.position.Y + (double)mapEventBase.LunPage.hitShift.Y - mapEventBase.LunPage.hitrange.Y / 2 - num2
								&& vector3_1.Y <= mapEventBase.position.Y + (double)mapEventBase.LunPage.hitShift.Y + mapEventBase.LunPage.hitrange.Y / 2 + num2;
                        }
                    }
                    if (isColliding && mapEventBase.LunPage.NormalMan)
                        this.player.stopping = true;
                    if (isColliding && (this.player.stoptime <= 120 || !mapEventBase.LunPage.NormalMan))
                    {
                        mapEventBase.playeHit = true;
                        if (mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Touch)
                            this.player.hitEvent = num1;
                        this.player.canmove = new bool[Enum.GetNames(typeof(MapCharacterBase.ANGLE)).Length];
                        if (mapEventBase.LunPage.hitform)
                        {
                            float num2 = mapEventBase.position.Y + mapEventBase.LunPage.hitShift.Y - mapEventBase.LunPage.hitrange.Y / 2;
                            float num3 = mapEventBase.position.Y + mapEventBase.LunPage.hitShift.Y + mapEventBase.LunPage.hitrange.Y / 2;
                            float num4 = mapEventBase.position.X + mapEventBase.LunPage.hitShift.X - mapEventBase.LunPage.hitrange.X / 2;
                            float num5 = mapEventBase.position.X + mapEventBase.LunPage.hitShift.X + mapEventBase.LunPage.hitrange.X / 2;
                            Vector3 vector3_2 = vector3_1;
                            if (vector3_2.Y < (double)num2)
                            {
                                if (vector3_2.X < (double)num4)
                                    this.player.canmove[0] = true;
                                else if (vector3_2.X >= (double)num4 && vector3_2.X <= (double)num5)
                                {
                                    this.player.canmove[0] = true;
                                    this.player.canmove[7] = true;
                                    this.player.canmove[6] = true;
                                }
                                else if (vector3_2.X > (double)num5)
                                    this.player.canmove[6] = true;
                            }
                            else if (vector3_2.Y >= (double)num2 && vector3_2.Y <= (double)num3)
                            {
                                if (vector3_2.X < (double)num4)
                                {
                                    this.player.canmove[1] = true;
                                    this.player.canmove[2] = true;
                                    this.player.canmove[0] = true;
                                }
                                else
                                {
                                    this.player.canmove[5] = true;
                                    this.player.canmove[6] = true;
                                    this.player.canmove[4] = true;
                                }
                            }
                            else if (vector3_2.X < (double)num4)
                                this.player.canmove[2] = true;
                            else if (vector3_2.X >= (double)num4 && vector3_2.X <= (double)num5)
                            {
                                this.player.canmove[3] = true;
                                this.player.canmove[4] = true;
                                this.player.canmove[2] = true;
                            }
                            else if (vector3_2.X > (double)num5)
                                this.player.canmove[4] = true;
                        }
                        else
                        {
                            if (mapEventBase.LunPage.NormalMan)
                            {
                                if (!this.eventmanager.playevent && !this.player.openMenu)
                                {
                                    this.player.stopping = true;
                                    ++this.player.stoptime;
                                }
                                else
                                    this.player.stoptime = 0;
                            }
                            float num2 = mapEventBase.position.Y + mapEventBase.LunPage.hitShift.Y - mapEventBase.LunPage.hitrange.X;
                            float num3 = mapEventBase.position.Y + mapEventBase.LunPage.hitShift.Y + mapEventBase.LunPage.hitrange.X;
                            float num4 = mapEventBase.position.X + mapEventBase.LunPage.hitShift.X - mapEventBase.LunPage.hitrange.X;
                            float num5 = mapEventBase.position.X + mapEventBase.LunPage.hitShift.X + mapEventBase.LunPage.hitrange.X;
                            Vector3 vector3_2 = vector3_1;
                            if (vector3_2.Y < (double)num2)
                            {
                                if (vector3_2.X < (double)num4)
                                    this.player.canmove[0] = true;
                                else if (vector3_2.X >= (double)num4 && vector3_2.X <= (double)num5)
                                {
                                    this.player.canmove[0] = true;
                                    this.player.canmove[7] = true;
                                    this.player.canmove[6] = true;
                                }
                                else if (vector3_2.X > (double)num5)
                                    this.player.canmove[6] = true;
                            }
                            else if (vector3_2.Y >= (double)num2 && vector3_2.Y <= (double)num3)
                            {
                                if (vector3_2.X < (double)num4)
                                {
                                    this.player.canmove[1] = true;
                                    this.player.canmove[2] = true;
                                    this.player.canmove[0] = true;
                                }
                                else
                                {
                                    this.player.canmove[5] = true;
                                    this.player.canmove[6] = true;
                                    this.player.canmove[4] = true;
                                }
                            }
                            else if (vector3_2.X < (double)num4)
                                this.player.canmove[2] = true;
                            else if (vector3_2.X >= (double)num4 && vector3_2.X <= (double)num5)
                            {
                                this.player.canmove[3] = true;
                                this.player.canmove[4] = true;
                                this.player.canmove[2] = true;
                            }
                            else if (vector3_2.X > (double)num5)
                                this.player.canmove[4] = true;
                        }
                        if (this.player.canmove[(int)this.player.Angle])
                        {
                            if (mapEventBase.LunPage.startterms == EventPage.STARTTERMS.Rbutton)
                                this.player.hitPlug = num1;
                            else
                                this.player.hitEvent = num1;
                        }
                        if (flag1)
                            flag1 = mapEventBase.LunPage.hitform && mapEventBase.LunPage.rendType <= 1;
                    }
                    else
                        mapEventBase.playeHit = false;
                }
            }
            if (!this.player.stopping)
                this.player.stoptime = 0;
            return flag1;
        }

        public void StepOut()
        {
            switch (this.step)
            {
                case SceneMap.STEPS.rightstep:
                    if (player.Position.Y < (double)this.stepover[0])
                    {
                        this.player.overStep = true;
                        this.player.position.Y = this.stepover[0] + 8;
                    }
                    if (player.Position.Y <= (double)this.stepover[1])
                        break;
                    this.player.overStep = true;
                    this.player.position.Y = this.stepover[1] + 8;
                    break;
                case SceneMap.STEPS.leftstepp:
                    if (player.Position.X < (double)this.stepover[0])
                    {
                        this.player.overStep = true;
                        this.player.position.X = this.stepover[0] + 8;
                    }
                    if (player.Position.X <= (double)this.stepover[1])
                        break;
                    this.player.overStep = true;
                    this.player.position.X = this.stepover[1] + 8;
                    break;
            }
        }

        private void CameraMove()
        {
            float num1 = !this.setCameraOn ? this.player.Position.X : this.setCamera.X;
            float num2 = !this.setCameraOn ? this.player.Position.Y : this.setCamera.Y;
            float num3 = !this.setCameraOn ? this.player.Position.Z : this.setCamera.Z;
            this.field.camera.X = (int)(this.field.MapsizeX / 2 + 2.0 * num1 - num2 * 2.0 + cameraPlus.X);
            this.field.camera.Y = (int)(num1 + (double)num2 + num3 - 4.0 + cameraPlus.Y) - 8f;
        }

        public override void Render(IRenderer dg)
        {
            this.field.Render(dg);
            if (this.player.openMenu)
            {
                if (alpha > 0.0)
                {
                    Color color = Color.FromArgb((int)this.alpha, this.fadeColor);
                    this._rect = new Rectangle(0, 0, 240, 160);
                    this._position = new Vector2(0.0f, 0.0f);
                    dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
                }
                this.player.menu.Render(dg);
            }
            else
            {
                if (!this.hideStatus)
                {
                    if (this.savedata.isJackedIn)
                    {
                        Vector2 vector2 = new Vector2(24f, 8f);
                        this._rect = new Rectangle(80, 0, 44, 16);
                        this._position = vector2;
                        dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, Color.White);
                        this.HP.HPRender(dg, new Vector2(vector2.X + 12f, vector2.Y - 1f));
                        // 38: ROM element
                        if (this.savedata.ValList[38] > 0)
                        {
                            if (this.savedata.ValList[38] == 8 || this.savedata.ValList[38] == 7)
                            {
                                this._rect = new Rectangle(216, 88, 16, 16);
                                this._position = new Vector2(48f, 0.0f);
                                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                            }
                            else
                            {
                                this._rect = new Rectangle(216 + this.savedata.ValList[38] * 16, 88, 16, 16);
                                this._position = new Vector2(48f, 0.0f);
                                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                            }
                        }
                    }
                    else
                    {
                        this._rect = new Rectangle(208 + phoneAnime % 2 * 16, 264, 16, 24);
                        this._position = new Vector2(0.0f, 0.0f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        if (this.phoneAnime > 0)
                        {
                            this._rect = new Rectangle(240 + phoneAnime * 16, 264, 16, 16);
                            this._position = new Vector2(16f, 0.0f);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        }
                        if (!this.savedata.FlagList[0])
                        {
                            this._rect = new Rectangle(240, 264, 16, 16);
                            this._position = new Vector2(16f, 8f);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        }
                    }
                    Vector2 vector2_1 = new Vector2(240 - this.savedata.plase.Length * 8, 146f)
                    {
                        X = 2f
                    };
                    this._position = new Vector2(vector2_1.X - 1f, vector2_1.Y - 1f);
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(vector2_1.X - 1f, vector2_1.Y);
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(vector2_1.X - 1f, vector2_1.Y + 1f);
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(vector2_1.X, vector2_1.Y - 1f);
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(vector2_1.X, vector2_1.Y + 1f);
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(vector2_1.X + 1f, vector2_1.Y - 1f);
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(vector2_1.X + 1f, vector2_1.Y);
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.FromArgb(32, 32, 32));
                    this._position = new Vector2(vector2_1.X + 1f, vector2_1.Y + 1f);
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.FromArgb(32, 32, 32));
                    this._position = vector2_1;
                    dg.DrawMicroText(this.savedata.plase, this._position, Color.White);
                    if (this.savedata.FlagList[4] || this.savedata.FlagList[69])
                    {
                        string str1 = (this.savedata.ValList[40] / 10000).ToString();
                        if (str1.Length < 2)
                            str1 = "0" + str1;
                        int num = this.savedata.ValList[40] % 10000 / 100;
                        string str2 = num.ToString();
                        if (str2.Length < 2)
                            str2 = "0" + str2;
                        num = this.savedata.ValList[40] % 100;
                        string str3 = num.ToString();
                        if (str3.Length < 2)
                            str3 = "0" + str3;
                        string txt = str1 + "：" + str2 + "：" + str3;
                        Color color = Color.White;
                        if (this.savedata.ValList[40] <= 300)
                            color = Color.Orange;
                        else if (this.savedata.ValList[40] <= 500)
                            color = Color.Yellow;
                        if (this.savedata.ValList[40] <= 1000 && this.savedata.ValList[40] % 100 == 0 && (!this.savedata.FlagList[69] && this.savedata.ValList[40] != this.savedata.ValList[41]) && !this.eventmanager.playevent)
                            this.sound.PlaySE(SoundEffect.search);
                        this.TextRender(dg, txt, false, new Vector2(176f, 0.0f), false, color);
                    }
                }
                if (alpha > 0.0)
                {
                    if (alpha > (double)byte.MaxValue)
                        this.alpha = byte.MaxValue;
                    Color color = Color.FromArgb((int)this.alpha, this.fadeColor);
                    this._rect = new Rectangle(0, 0, 240, 160);
                    this._position = new Vector2(0.0f, 0.0f);
                    dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
                }
            }
            if (this.eventmanager.playevent)
			{
				this.eventmanager.Render(dg);
			}
            if (this.eventmanagerParallel.playevent)
			{
				this.eventmanagerParallel.Render(dg);
			}
            // Temporary copy made in case of race condition modifying collection
			foreach (var persistentEvent in this.persistentEvents.ToArray())
			{
				if (persistentEvent.IsActive)
				{
					persistentEvent.PersistentRender(dg);
				}
			}
            if (this.DebugOn)
			{
				this.debugmode.Render(dg);
			}
        }

        private void MailAnime()
        {
            if (this.mailflame % 4 == 0)
            {
                ++this.phoneAnime;
                if (this.phoneAnime >= 4)
                    this.phoneAnime = 0;
            }
            switch (this.mailflame)
            {
                case 0:
                    if (this.mailsound)
                    {
                        this.sound.PlaySE(SoundEffect.mail);
                        break;
                    }
                    break;
                case 40:
                    this.mailflame = -1;
                    ++this.mailloop;
                    break;
            }
            ++this.mailflame;
            if (this.mailloop != 3)
                return;
            this.mailflag = false;
            this.mailloop = 0;
            this.mailflame = 0;
            this.phoneAnime = 0;
        }

        public void MailOn(bool sound)
        {
            this.mailloop = 0;
            this.mailflag = true;
            this.mailsound = sound;
        }

        public void FadeStart(Color fadecolor, int fadetime)
        {
            if (fadetime > 0)
            {
                this.R = fadeColor.R;
                this.G = fadeColor.G;
                this.B = fadeColor.B;
                this.plusAlpha = (fadecolor.A - this.alpha) / fadetime;
                this.plusR = (fadecolor.R - (float)this.fadeColor.R) / fadetime;
                this.plusG = (fadecolor.G - (float)this.fadeColor.G) / fadetime;
                this.plusB = (fadecolor.B - (float)this.fadeColor.B) / fadetime;
                this.fadeFlame = 0;
                this.fadeTime = fadetime;
                this.endAlpha = fadecolor.A;
                this.endR = fadecolor.R;
                this.endG = fadecolor.G;
                this.endB = fadecolor.B;
                if (alpha == 0.0)
                {
                    this.R = fadecolor.R;
                    this.G = fadecolor.G;
                    this.B = fadecolor.B;
                    this.plusR = 0.0f;
                    this.plusG = 0.0f;
                    this.plusB = 0.0f;
                }
                this.fadeflug = true;
            }
            else
            {
                this.alpha = fadecolor.A;
                this.R = fadecolor.R;
                this.G = fadecolor.G;
                this.B = fadecolor.B;
                this.endR = fadecolor.R;
                this.endG = fadecolor.G;
                this.endB = fadecolor.B;
                this.fadeColor = fadecolor;
                this.fadeflug = false;
            }
        }

        private void Fadeing()
        {
            if (this.fadeFlame >= this.fadeTime)
            {
                this.fadeflug = false;
                this.alpha = endAlpha;
                this.R = endR;
                this.G = endG;
                this.B = endB;
                this.fadeFlame = 0;
                if (alpha == 0.0)
                {
                    this.R = 0.0f;
                    this.G = 0.0f;
                    this.B = 0.0f;
                }
            }
            else
            {
                ++this.fadeFlame;
                this.alpha += this.plusAlpha;
                this.R += this.plusR;
                this.G += this.plusG;
                this.B += this.plusB;
                if (R > (double)byte.MaxValue)
                    this.R = byte.MaxValue;
                if (G > (double)byte.MaxValue)
                    this.G = byte.MaxValue;
                if (B > (double)byte.MaxValue)
                    this.B = byte.MaxValue;
                if (alpha > (double)byte.MaxValue)
                    this.alpha = byte.MaxValue;
            }
            this.fadeColor = Color.FromArgb((int)this.R, (int)this.G, (int)this.B);
        }

        public enum STEPS
        {
            normal,
            rightstep,
            leftstepp,
        }
    }
}
