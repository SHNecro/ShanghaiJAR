using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using NSObject;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class Barlizard : EnemyBase
    {
        private Barlizard.MOTION motion = Barlizard.MOTION.neutral;
        private readonly int nspeed;
        private readonly int moveroop;
        private int roopneutral;
        private int roopmove;

        public Barlizard(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.BarlizardName1");
                    this.power = 200;
                    this.hp = 1500;
                    this.moveroop = 1;
                    this.nspeed = 2;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.BarlizardName2");
                    this.power = 40;
                    this.hp = 60;
                    this.moveroop = 1;
                    this.nspeed = 6;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.BarlizardName3");
                    this.power = 80;
                    this.hp = 100;
                    this.moveroop = 1;
                    this.nspeed = 5;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.BarlizardName4");
                    this.power = 150;
                    this.hp = 190;
                    this.moveroop = 1;
                    this.nspeed = 4;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.BarlizardName5");
                    this.power = 180;
                    this.hp = 240;
                    this.nspeed = 3;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.BarlizardName6") + (version - 3).ToString();
                    this.power = 180 + (version - 4) * 20;
                    this.hp = 240 + (version - 4) * 40;
                    this.nspeed = 2;
                    break;
            }
            this.picturename = nameof(Barlizard);
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 48;
            this.height = 40;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new RebirthShield(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new RebirthShield(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new RebirthShield(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new RebirthShield(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new RebirthShield(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new LifeShield(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new LifeShield(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new LifeShield(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new LifeShield(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new LifeShield(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new ReflectShield(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ReflectShield(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new ReflectShield(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new ReflectShield(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ReflectShield(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new RebirthShield(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new LifeShield(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new LifeShield(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ReflectShield(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new RebirthShieldX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new RebirthShield(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new LifeShield(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new LifeShield(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ReflectShield(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new RebirthShield(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 1500;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 64.0 + 8.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Barlizard.MOTION.neutral;
            switch (this.motion)
            {
                case Barlizard.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 4)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (!this.badstatus[4])
                                {
                                    this.motion = Barlizard.MOTION.attack;
                                    this.counterTiming = true;
                                }
                            }
                        }
                        break;
                    }
                    break;
                case Barlizard.MOTION.move:
                    ++this.roopmove;
                    this.motion = Barlizard.MOTION.neutral;
                    this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
                        this.motion = Barlizard.MOTION.neutral;
                        this.frame = 0;
                        this.roopneutral = 0;
                        break;
                    }
                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                    this.position = this.positionre;
                    this.PositionDirectSet();
                    this.frame = 0;
                    this.roopneutral = 0;
                    break;
                case Barlizard.MOTION.attack:
                    this.animationpoint = this.AnimeAttack1(this.frame);
                    if (this.moveflame && this.frame == 10)
                    {
                        this.sound.PlaySE(SoundEffect.rockopen);
                        Point point = this.RandomTarget(this.UnionEnemy);
                        point.X += this.UnionRebirth;
                        int color = version - 1;
                        if (color > 3)
                            color = 3;
                        else if (color == -1)
                            color = 4;
                        if (this.version > 0)
                        {
                            this.parent.objects.Add(new BarrierObject(this.sound, this.parent, point.X, point.Y, this.union, this.Power, color));
                        }
                        else
                        {
                            for (int index = 0; index < 9; ++index)
                            {
                                point.X = this.Random.Next(6);
                                point.Y = this.Random.Next(3);
                                this.parent.objects.Add(new BarrierObject(this.sound, this.parent, point.X, point.Y, this.union, this.Power, color));
                            }
                        }
                        this.counterTiming = false;
                        this.frame = 0;
                        this.roopmove = 0;
                        this.roopneutral = 0;
                        this.speed = this.nspeed;
                        this.motion = Barlizard.MOTION.attack2;
                        break;
                    }
                    break;
                case Barlizard.MOTION.attack2:
                    this.animationpoint = this.AnimeAttack2(this.frame);
                    if (this.moveflame && this.frame == 10)
                    {
                        this.motion = Barlizard.MOTION.move;
                        break;
                    }
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
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
                this._rect.Y = this.animationpoint.Y;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 - 3);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        1,
        2,
        3
            }, 1, waitflame);
        }

        private Point AnimeAttack1(int waitflame)
        {
            return this.Return(new int[11]
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
        10
            }, new int[11] { 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, 0, waitflame);
        }

        private Point AnimeAttack2(int waitflame)
        {
            return this.Return(new int[11]
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
        10
            }, new int[11] { 13, 12, 11, 10, 9, 8, 8, 8, 5, 4, 0 }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
            attack2,
        }
    }
}

