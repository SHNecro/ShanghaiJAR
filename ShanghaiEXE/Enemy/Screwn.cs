using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class Screwn : EnemyBase
    {
        private Screwn.MOTION motion = Screwn.MOTION.neutral;
        private int angle;
        private bool back;
        private int fire;
        private int realFlame;

        public Screwn(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.ScrewnName1");
            this.picturename = "screwn";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.aqua;
            this.Flying = false;
            this.hp = 30 + 50 * version;
            this.power = 15 + 15 * ((version - 1) * 2);
            this.wide = 40;
            this.height = 64;
            this.version = v;
            this.frame = 20 * (n - 1);
            this.speed = 7 - version;
            this.printhp = true;
            this.printNumber = true;
            this.effecting = false;
            if (this.parent != null)
                this.roop = (byte)(parent.manyenemys - (uint)this.number);
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.ScrewnName2");
                    this.speed = 2;
                    this.hp = 1500;
                    this.power = 200;
                    this.printNumber = false;
                    this.dropchips[0].chip = new ColdAir1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ColdAir1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ColdAir2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ColdAir3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ColdAir1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                case 1:
                    this.dropchips[0].chip = new ColdAir1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ColdAir1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new ColdAir1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ColdAir1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ColdAir1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 350;
                    break;
                case 2:
                    this.dropchips[0].chip = new ColdAir2(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ColdAir2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ColdAir2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new ColdAir2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ColdAir2(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new ColdAir3(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ColdAir3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ColdAir3(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new ColdAir3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new ColdAir3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.ScrewnName3");
                    this.printNumber = false;
                    this.dropchips[0].chip = new ColdAir1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ColdAir1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ColdAir2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ColdAir3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ColdAir1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.ScrewnName4") + (version - 3).ToString();
                    this.printNumber = false;
                    this.dropchips[0].chip = new ColdAir1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ColdAir1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ColdAir2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ColdAir3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ColdAir1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new ColdAirX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                        break;
                    }
                    break;
            }
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0) + 4 * this.UnionRebirth, (float)(position.Y * 24.0 + 54.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Screwn.MOTION.neutral;
            switch (this.motion)
            {
                case Screwn.MOTION.neutral:
                    this.FlameControl(this.frame > 30 ? this.speed / 2 : this.speed);
                    this.animationpoint = this.AnimeNeutral(this.frame % 5);
                    if ((this.frame > 60 || this.version == 0) && this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.badstatus[4])
                    {
                        this.motion = Screwn.MOTION.boost;
                        this.frame = 0;
                        break;
                    }
                    break;
                case Screwn.MOTION.boost:
                    this.FlameControl(this.speed / 2);
                    this.animationpoint = this.Animeboost(this.frame % 2);
                    this.animationpoint.X += this.angle * 2;
                    if (this.frame >= 4)
                    {
                        this.frame = 0;
                        if (!this.back)
                        {
                            ++this.angle;
                            if (this.angle >= 3)
                            {
                                this.realFlame = 60;
                                this.motion = Screwn.MOTION.attack;
                                if (this.version == 0)
                                {
                                    this.parent.effects.RemoveAll(e => e is NSEffect.BackWind);
                                    this.parent.effects.RemoveAll(e => e is NSEffect.PushWind);
                                    this.parent.effects.Add(new NSEffect.PushWind(this.sound, this.parent, new Vector2(), new Point(), this.union));
                                }
                            }
                        }
                        else
                        {
                            --this.angle;
                            if (this.angle <= 0)
                            {
                                this.motion = Screwn.MOTION.neutral;
                                this.back = false;
                            }
                        }
                        break;
                    }
                    break;
                case Screwn.MOTION.attack:
                    this.FlameControl(2);
                    this.animationpoint = this.AnimeAttack(this.frame % 2);
                    if (this.realFlame >= 120)
                    {
                        this.counterTiming = false;
                        this.realFlame = 0;
                        ++this.fire;
                        if (this.version == 0)
                        {
                            this.realFlame = 30;
                            this.sound.PlaySE(SoundEffect.sand);
                            int pX = this.position.X + (this.union == Panel.COLOR.red ? 1 : -1);
                            int num = this.Random.Next(3);
                            int pY1 = 0;
                            int pY2 = 0;
                            switch (num)
                            {
                                case 0:
                                    pY1 = 1;
                                    pY2 = 2;
                                    break;
                                case 1:
                                    pY1 = 0;
                                    pY2 = 2;
                                    break;
                                case 2:
                                    pY1 = 0;
                                    pY2 = 1;
                                    break;
                            }
                            this.parent.attacks.Add(new PushTornado(this.sound, this.parent, pX, pY1, this.union, this.Power, ChipBase.ELEMENT.aqua, 1, 1, true));
                            this.parent.attacks.Add(new PushTornado(this.sound, this.parent, pX, pY2, this.union, this.Power, ChipBase.ELEMENT.aqua, 1, 1, true));
                            this.parent.effects.RemoveAll(e => e is NSEffect.BackWind);
                            this.parent.effects.RemoveAll(e => e is NSEffect.PushWind);
                            this.parent.effects.Add(new NSEffect.PushWind(this.sound, this.parent, new Vector2(), new Point(), this.union));
                            break;
                        }
                        if (this.fire > version)
                        {
                            this.fire = 0;
                            this.back = true;
                            this.motion = Screwn.MOTION.boost;
                        }
                        else
                        {
                            this.sound.PlaySE(SoundEffect.sand);
                            this.parent.attacks.Add(new PushTornado(this.sound, this.parent, this.position.X + (this.union == Panel.COLOR.red ? 1 : -1), this.position.Y, this.union, this.Power, ChipBase.ELEMENT.aqua, 1, 1, true));
                        }
                        break;
                    }
                    if (this.realFlame >= 100 && this.fire <= version)
                        this.counterTiming = true;
                    ++this.realFlame;
                    break;
            }
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = this.height * 5;
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
            this.HPposition = new Vector2(this.positionDirect.X + 4f, this.positionDirect.Y - 32f);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 3, 4 }, new int[5]
            {
        0,
        1,
        2,
        3,
        4
            }, 1, waitflame);
        }

        private Point Animeboost(int waitflame)
        {
            return this.Return(new int[2] { 0, 1 }, new int[2]
            {
        5,
        6
            }, 0, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[2] { 0, 1 }, new int[2]
            {
        11,
        12
            }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            boost,
            attack,
        }
    }
}

