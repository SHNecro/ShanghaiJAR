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
    internal class Furjirn : EnemyBase
    {
        private Furjirn.MOTION motion = Furjirn.MOTION.neutral;
        private readonly int nspeed;
        private readonly int moveroop;
        private int roopneutral;
        private int roopmove;
        private bool spAttack;

        public Furjirn(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.nspeed = 14 - version * 2;
            this.speed = this.nspeed;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.FurjirnName1");
                    this.element = ChipBase.ELEMENT.normal;
                    this.power = 100;
                    this.hp = 1600;
                    this.moveroop = 1;
                    this.nspeed = 2;
                    this.speed = this.nspeed;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.FurjirnName2");
                    this.element = ChipBase.ELEMENT.normal;
                    this.power = 20;
                    this.hp = 60;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.FurjirnName3");
                    this.element = ChipBase.ELEMENT.heat;
                    this.power = 30;
                    this.hp = 100;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.FurjirnName4");
                    this.element = ChipBase.ELEMENT.eleki;
                    this.power = 50;
                    this.hp = 170;
                    this.moveroop = 1;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.FurjirnName5");
                    this.element = ChipBase.ELEMENT.poison;
                    this.power = 70;
                    this.hp = 250;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.FurjirnName6") + (version - 3).ToString();
                    this.element = ChipBase.ELEMENT.poison;
                    this.power = 70 + (version - 4) * 20;
                    this.hp = 250 + (version - 4) * 40;
                    break;
            }
            this.picturename = "furjirn";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 48;
            this.height = 48;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = true;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new NSChip.Storm(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new NSChip.Storm(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new NSChip.Storm(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new NSChip.Storm(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new NSChip.Storm(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new HellStorm(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new HellStorm(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new HellStorm(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new HellStorm(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new HellStorm(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new ElekiStorm(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new ElekiStorm(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new ElekiStorm(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ElekiStorm(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ElekiStorm(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new BioStorm(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new BioStorm(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new BioStorm(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new BioStorm(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new StormX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new BioStorm(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new BioStorm(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new BioStorm(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new BioStorm(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new BioStorm(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 1500;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 64.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Furjirn.MOTION.neutral;
            switch (this.motion)
            {
                case Furjirn.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 4)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if ((this.roopneutral >= 2 || this.version == 0) && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.speed = 4;
                                    this.motion = Furjirn.MOTION.attack;
                                    this.counterTiming = true;
                                }
                                else
                                    this.motion = Furjirn.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case Furjirn.MOTION.move:
                    ++this.roopmove;
                    this.motion = Furjirn.MOTION.neutral;
                    this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
                        this.motion = Furjirn.MOTION.neutral;
                        this.frame = 0;
                        this.roopneutral = 0;
                        break;
                    }
                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                    this.position = this.positionre;
                    this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 64.0));
                    this.frame = 0;
                    this.roopneutral = 0;
                    break;
                case Furjirn.MOTION.attack:
                    this.animationpoint = this.AnimeAttack(this.frame);
                    if (this.moveflame)
                    {
                        if (this.frame == 7)
                        {
                            if (this.version == 0)
                            {
                                if (this.spAttack)
                                {
                                    this.parent.attacks.Add(new Tornado(this.sound, this.parent, this.position.X, this.position.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.element, 8));
                                }
                                else
                                {
                                    switch (this.position.Y)
                                    {
                                        case 0:
                                            this.parent.attacks.Add(new Tornado(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.element, 8));
                                            this.parent.attacks.Add(new Tornado(this.sound, this.parent, this.position.X, this.position.Y + 2, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.element, 8));
                                            break;
                                        case 1:
                                            this.parent.attacks.Add(new Tornado(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.element, 8));
                                            this.parent.attacks.Add(new Tornado(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.element, 8));
                                            break;
                                        case 2:
                                            this.parent.attacks.Add(new Tornado(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.element, 8));
                                            this.parent.attacks.Add(new Tornado(this.sound, this.parent, this.position.X, this.position.Y - 2, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.element, 8));
                                            break;
                                    }
                                }
                            }
                            else
                                this.parent.attacks.Add(new Tornado(this.sound, this.parent, this.position.X, this.position.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.element, version * 2));
                            this.spAttack = !this.spAttack;
                            this.counterTiming = false;
                        }
                        if (this.frame >= 13)
                        {
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = Furjirn.MOTION.neutral;
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
            return this.Return(new int[5] { 0, 1, 2, 3, 4 }, new int[5]
            {
        0,
        1,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[5] { 0, 6, 7, 9, 13 }, new int[5]
            {
        3,
        4,
        5,
        6,
        6
            }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

