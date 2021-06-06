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
    internal class Doripper : EnemyBase
    {
        private Doripper.MOTION motion = Doripper.MOTION.neutral;
        private EnemyShadow shadow;
        private int Y;
        private readonly int yspeed;
        private int roopneutoral;
        private DammyEnemy dammy;

        public Doripper(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "doripper";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 24;
            this.height = 56;
            this.name = ShanghaiEXE.Translate("Enemy.DoripperName1");
            this.printNumber = true;
            this.yspeed = version * 2 + 2;
            this.speed = 7 - Math.Min(6, (int)this.version);
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.DoripperName2");
                    this.printNumber = false;
                    this.power = 150;
                    this.hp = 900;
                    this.yspeed = 8;
                    this.speed = 4;
                    break;
                case 1:
                    this.power = 80;
                    this.hp = 120;
                    break;
                case 2:
                    this.power = 100;
                    this.hp = 150;
                    break;
                case 3:
                    this.power = 120;
                    this.hp = 220;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.DoripperName3");
                    this.printNumber = false;
                    this.power = 150;
                    this.hp = 270;
                    break;
                default:
                    this.printNumber = false;
                    this.name = ShanghaiEXE.Translate("Enemy.DoripperName4") + (version - 3).ToString();
                    this.power = 150 + (version - 4) * 40;
                    this.hp = 270 + (version - 4) * 40;
                    break;
            }
            this.roop = this.number;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            this.element = ChipBase.ELEMENT.earth;
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new DigDrill1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DigDrill1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new DigDrill1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new DigDrill1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new DigDrill1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new DigDrill2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DigDrill2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new DigDrill2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new DigDrill2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new DigDrill2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new DigDrill3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new DigDrill3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new DigDrill3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new DigDrill3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new DigDrill3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new DigDrill1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DigDrill1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new DigDrill2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new DigDrill3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new DigDrillX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new DigDrill1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new DigDrill1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new DigDrill2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new DigDrill3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new DigDrill1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1600;
                    break;
            }
            this.neutlal = true;
        }

        public override void Init()
        {
            if (this.parent == null)
                return;
            this.shadow = new EnemyShadow(this.sound, this.parent, this, this.union == Panel.COLOR.red);
            this.shadow.slide.X = this.union == Panel.COLOR.red ? -16 : 0;
            this.parent.effects.Add(shadow);
            this.dammy = new DammyEnemy(this.sound, this.parent, this.position.X, this.position.Y, this, true)
            {
                nohit = true
            };
        }

        private void DammySet()
        {
            this.dammy.position = this.position;
            this.dammy.flag = true;
            this.dammy.nomove = true;
            this.dammy.effecting = true;
            this.parent.enemys.Add(dammy);
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0) + 2 * this.UnionRebirth, (float)(position.Y * 24.0 + 56.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Doripper.MOTION.neutral;
            this.positionre = this.position;
            if (this.Y < -32)
                this.nohit = true;
            else
                this.nohit = false;
            switch (this.motion)
            {
                case Doripper.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.version == 0)
                            this.frame = 8;
                        if (this.frame == 8)
                        {
                            this.frame = 0;
                            ++this.roopneutoral;
                            if (this.roopneutoral >= 3 && this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.badstatus[4])
                            {
                                this.counterTiming = true;
                                this.motion = Doripper.MOTION.attack;
                                this.roopneutoral = 0;
                            }
                        }
                        break;
                    }
                    break;
                case Doripper.MOTION.attack:
                    if (this.frame <= 6)
                    {
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        break;
                    }
                    if (this.moveflame && this.frame == 7)
                    {
                        this.counterTiming = false;
                        this.sound.PlaySE(SoundEffect.drill1);
                        this.DammySet();
                    }
                    this.Y -= this.yspeed;
                    if (this.Y <= -160)
                    {
                        if (this.Random.Next(2) == 0)
                            this.MoveRandom(false, false, this.UnionEnemy, false);
                        else
                            this.positionre = this.RandomTarget();
                        this.position = this.positionre;
                        this.effecting = true;
                        this.shadow.print = false;
                        this.noslip = true;
                        this.flying = true;
                        this.motion = Doripper.MOTION.attack2;
                        if (this.version == 0)
                        {
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, new Point(), 60, true));
                            this.waittime = 30;
                        }
                        else
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, new Point(), 90, true));
                        this.frame = 0;
                    }
                    break;
                case Doripper.MOTION.attack2:
                    if (this.waittime == 90)
                    {
                        this.Y = 16;
                        this.PositionDirectSet();
                        this.sound.PlaySE(SoundEffect.breakObject);
                        this.sound.PlaySE(SoundEffect.drill1);
                        this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 1, this.element));
                        this.StandPanel.Crack();
                        this.StandPanel.Crack();
                    }
                    if (this.effecting && this.waittime > 90)
                    {
                        if (this.Y > -240)
                        {
                            this.Y -= this.yspeed;
                            break;
                        }
                        ++this.waittime;
                        if (this.waittime >= 180)
                        {
                            this.counterTiming = false;
                            this.effecting = false;
                            if (this.version == 0)
                            {
                                if (this.Random.Next(2) == 0)
                                    this.MoveRandom(false, false, this.UnionEnemy, false);
                                else
                                    this.positionre = this.RandomTarget();
                                this.position = this.positionre;
                                this.effecting = true;
                                this.shadow.print = false;
                                this.noslip = true;
                                this.flying = true;
                                this.motion = Doripper.MOTION.attack2;
                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, new Point(), 60, true));
                                this.frame = 0;
                                this.waittime = 30;
                            }
                            else
                            {
                                this.MoveRandom(false, false, this.union, false);
                                this.position = this.positionre;
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                this.animationpoint.X = 0;
                                this.PositionDirectSet();
                                this.Y = 0;
                                this.shadow.print = true;
                                this.motion = Doripper.MOTION.neutral;
                                this.frame = 0;
                                this.waittime = 0;
                                this.flying = false;
                                this.noslip = false;
                                this.dammy.flag = false;
                            }
                        }
                        break;
                    }
                    ++this.waittime;
                    break;
            }
            this.MoveAftar();
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + 6 + this.Shake.X, (int)this.positionDirect.Y + 2 + this.Shake.Y + this.Y);
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
            }
            else
            {
                this._rect.Y = 0;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2(this.positionDirect.X + 6f, (float)(positionDirect.Y + 2.0 - this.height / 2 + 60.0) + Y);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
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
            }, new int[8] { 0, 1, 2, 1, 0, 3, 4, 3 }, 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 4, 5, 6 }, new int[7]
            {
        0,
        5,
        6,
        7,
        8,
        9,
        10
            }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            attack,
            attack2,
        }
    }
}

