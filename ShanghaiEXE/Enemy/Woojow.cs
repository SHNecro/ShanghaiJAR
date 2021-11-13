using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class Woojow : EnemyBase
    {
        private int Y = 0;
        private Woojow.MOTION motion = Woojow.MOTION.neutral;
        private int updowntime;
        private readonly int nspeed;
        private MonkeyPole monkeypole;
        private int roopneutral;
        private int roopmove;
        private EnemyShadow shadow;

        public Woojow(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.WoojowName1");
            this.flying = true;
            this.printNumber = true;
            this.nspeed = 12 - version * 2;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.WoojowName2");
                    this.nspeed = 1;
                    this.power = 300;
                    this.hp = 1600;
                    this.printNumber = false;
                    break;
                case 1:
                    this.power = 50;
                    this.hp = 100;
                    break;
                case 2:
                    this.power = 80;
                    this.hp = 150;
                    break;
                case 3:
                    this.power = 100;
                    this.hp = 260;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.WoojowName3");
                    this.power = 130;
                    this.hp = 290;
                    this.printNumber = false;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.WoojowName4") + (version - 3).ToString();
                    this.power = 150 + (version - 4) * 20;
                    this.hp = 290 + (version - 4) * 20;
                    this.printNumber = false;
                    break;
            }
            this.picturename = "woojow";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.leaf;
            this.Flying = true;
            this.wide = 40;
            this.height = 64;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = 0;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.WoojowName5");
                    this.hp = 900;
                    this.hpmax = this.hp;
                    this.power = 150;
                    this.hpprint = this.hp;
                    this.speed = 1;
                    this.printNumber = false;
                    this.dropchips[0].chip = new MonkeyPole3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MonkeyPole3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MonkeyPole3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new MonkeyPole3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MonkeyPole3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    break;
                case 1:
                    this.dropchips[0].chip = new MonkeyPole1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MonkeyPole1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new MonkeyPole1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new MonkeyPole1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new MonkeyPole1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new MonkeyPole2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new MonkeyPole2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MonkeyPole2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MonkeyPole2(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new MonkeyPole2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new MonkeyPole3(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new MonkeyPole3(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new MonkeyPole3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new MonkeyPole3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new MonkeyPole3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new MonkeyPole1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new MonkeyPole1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MonkeyPole2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new MonkeyPole3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MonkeyPole1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new MonkeyPoleX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                        break;
                    }
                    break;
            }
            this.speed = this.nspeed;
        }

        public override void Init()
        {
            if (this.parent == null)
                return;
            this.shadow = new EnemyShadow(this.sound, this.parent, this, this.union == Panel.COLOR.red);
            this.parent.effects.Add(shadow);
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 12.0), (float)(position.Y * 24.0 + 52.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Woojow.MOTION.neutral;
            if (this.Y < -32)
                this.nohit = true;
            else
                this.nohit = false;
            switch (this.motion)
            {
                case Woojow.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 8 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            this.roopneutral = 0;
                            this.updowntime = 0;
                            this.motion = Woojow.MOTION.move;
                        }
                        break;
                    }
                    break;
                case Woojow.MOTION.move:
                    if (this.updowntime < 18)
                        this.Y -= 8;
                    else if (this.updowntime == 33)
                    {
                        if (this.version == 0)
                        {
                            this.roopmove = 3;
                            this.MoveRandom(true, false);
                            this.shadow.print = false;
                        }
                        else
                            this.MoveRandom(false, false);
                        this.position = this.positionre;
                        this.PositionDirectSet();
                        if (this.version == 0)
                        {
                            this.sound.PlaySE(SoundEffect.chain);
                            int pX = this.union == Panel.COLOR.red ? 5 : 0;
                            for (int pY = 0; pY < 3; ++pY)
                                this.parent.attacks.Add(new MonkeyPoleChip(this.sound, this.parent, pX, pY, this.union, this.Power, 1, this.element, 4));
                        }
                    }
                    else if (this.updowntime > 48 && this.updowntime <= 66)
                        this.Y += 8;
                    else if (this.updowntime > 66)
                    {
                        ++this.roopmove;
                        this.frame = 0;
                        this.Y = 0;
                        if (this.roopmove > 3 && !this.badstatus[4])
                        {
                            this.roopmove = 0;
                            this.shadow.print = true;
                            this.motion = Woojow.MOTION.attack;
                        }
                        else
                            this.motion = Woojow.MOTION.neutral;
                        this.roopneutral = 0;
                    }
                    ++this.updowntime;
                    break;
                case Woojow.MOTION.attack:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeAttack(this.frame);
                        if (this.frame == 1)
                        {
                            this.counterTiming = true;
                            if (this.version == 0)
                                this.speed = 1;
                            else
                                this.speed = 3;
                        }
                        if (this.frame == 11)
                        {
                            this.sound.PlaySE(SoundEffect.chain);
                            this.monkeypole = new MonkeyPole(this.sound, this.parent, this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1, this.position.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, 4, this.version > 0 ? Math.Min(version - 1, 2) : 4, false, this.element);
                            this.parent.attacks.Add(monkeypole);
                            this.counterTiming = false;
                        }
                        if (this.frame >= 16 && !this.monkeypole.flag)
                        {
                            this.animationpoint = this.AnimeNeutral(this.frame);
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = Woojow.MOTION.neutral;
                        }
                        break;
                    }
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X + 8, (int)this.positionDirect.Y + this.Shake.Y + this.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = 5 * this.height;
            if (this.Hp <= 0)
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else
                this.color = this.mastorcolor;
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                for (int index = 0; index < 18; ++index)
                {
                    int x1 = (int)this.positionDirect.X;
                    Point shake = this.Shake;
                    int x2 = shake.X;
                    double num1 = x1 + x2 + 8;
                    int y1 = (int)this.positionDirect.Y;
                    shake = this.Shake;
                    int y2 = shake.Y;
                    double num2 = y1 + y2 + this.Y - 32 - 8 * index;
                    this._position = new Vector2((float)num1, (float)num2);
                    this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, 8);
                    if (this.version == 0)
                        this._rect.Y = 5 * this.height;
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }
            }
            else
            {
                this._rect.Y = 0;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                for (int index = 0; index < 18; ++index)
                {
                    int x1 = (int)this.positionDirect.X;
                    Point shake = this.Shake;
                    int x2 = shake.X;
                    double num1 = x1 + x2 + 8;
                    int y1 = (int)this.positionDirect.Y;
                    shake = this.Shake;
                    int y2 = shake.Y;
                    double num2 = y1 + y2 + this.Y - 32 - 8 * index;
                    this._position = new Vector2((float)num1, (float)num2);
                    this._rect = new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, 8);
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y - this.height / 2 + 12 + this.Y);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[8]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7
            }, new int[8] { 1, 2, 3, 4, 3, 2, 1, 0 }, 0, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[12]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        100
            }, new int[12] { 0, 1, 2, 3, 4, 5, 4, 3, 2, 1, 0, 6 }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

