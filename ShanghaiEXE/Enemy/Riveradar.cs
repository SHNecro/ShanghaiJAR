using NSAttack;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    internal class Riveradar : EnemyBase
    {
        private Riveradar.MOTION motion = Riveradar.MOTION.neutral;
        private readonly List<RiveradarCrosshair> target = new List<RiveradarCrosshair>();
        private readonly int nspeed;
        private readonly int manytarget;
        private bool attackanimation;
        private int roopneutral;
        private int roopmove;

        public Riveradar(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.name = ShanghaiEXE.Translate("Enemy.RiveradarName1");
            this.printNumber = true;
            switch (this.version)
            {
                case 1:
                    this.power = 50;
                    this.hp = 100;
                    this.manytarget = 2;
                    break;
                case 2:
                    this.power = 80;
                    this.hp = 160;
                    this.manytarget = 3;
                    break;
                case 3:
                    this.power = 120;
                    this.hp = 240;
                    this.manytarget = 4;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.RiveradarName2");
                    this.printNumber = false;
                    this.power = 150;
                    this.hp = 270;
                    this.manytarget = 5;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.RiveradarName3") + (version - 3).ToString();
                    this.printNumber = false;
                    this.power = 150 + (version - 4) * 20;
                    this.hp = 270 + (version - 4) * 30;
                    this.manytarget = 5;
                    break;
            }
            this.picturename = "riveradar";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.normal;
            this.wide = 56;
            this.height = 40;
            this.hpmax = this.hp;
            this.nspeed = 6 - Math.Min(5, version * 2);
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = true;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.RiveradarName4");
                    this.printNumber = false;
                    this.manytarget = 18;
                    this.nspeed = 1;
                    this.speed = this.nspeed;
                    this.hp = 1700;
                    this.hpmax = this.hp;
                    this.hpprint = this.hp;
                    this.power = 160;
                    this.dropchips[0].chip = new ChainGun1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ChainGun1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ChainGun1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ChainGun1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ChainGun1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 1:
                    this.dropchips[0].chip = new ChainGun1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ChainGun1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ChainGun1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ChainGun1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ChainGun1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new ChainGun2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ChainGun2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ChainGun2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ChainGun2(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new ChainGun2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new ChainGun3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ChainGun3(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new ChainGun3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ChainGun3(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ChainGun3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new ChainGun1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ChainGun2(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new ChainGun2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new ChainGun3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new ChainGun1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new ChainGunX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                        break;
                    }
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 17.0) - 3 * this.UnionRebirth, (float)(position.Y * 24.0 + 66.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Riveradar.MOTION.neutral;
            switch (this.motion)
            {
                case Riveradar.MOTION.neutral:
                    if (this.moveflame)
                    {
                        for (int index = 0; index < this.target.Count; ++index)
                        {
                            if (this.target[index].ManualFrame < 2)
                            {
                                ++this.target[index].ManualFrame;
                                if (this.target[index].ManualFrame == 2)
                                    this.sound.PlaySE(SoundEffect.search);
                            }
                        }
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 6)
                        {
                            if (this.target.Count < this.manytarget)
                            {
                                var newTarget = this.RandomPanel(this.UnionEnemy);
                                var newTargetEffect = new RiveradarCrosshair(this.sound, this.parent, newTarget.X, newTarget.Y, this.picturename, this.version);
                                this.target.Add(newTargetEffect);
                                this.parent.effects.Add(newTargetEffect);
                            }
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.target.Count == this.manytarget && this.target[this.manytarget - 1].ManualFrame >= 2 && !this.badstatus[4])
                                {
                                    this.speed = 4;
                                    this.motion = Riveradar.MOTION.attackStart;
                                    this.counterTiming = true;
                                }
                                else
                                    this.motion = Riveradar.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case Riveradar.MOTION.move:
                    ++this.roopmove;
                    this.motion = Riveradar.MOTION.neutral;
                    this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
                        this.motion = Riveradar.MOTION.neutral;
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
                case Riveradar.MOTION.attackStart:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeMove(this.frame);
                        if (this.frame >= 2)
                        {
                            this.counterTiming = false;
                            this.motion = Riveradar.MOTION.attack;
                            this.frame = 0;
                        }
                        break;
                    }
                    break;
                case Riveradar.MOTION.attack:
                    this.animationpoint.X = this.attackanimation ? 8 : 7;
                    if (this.moveflame)
                    {
                        this.attackanimation = !this.attackanimation;
                        if (this.attackanimation)
                        {
                            this.sound.PlaySE(SoundEffect.vulcan);
                            List<AttackBase> attacks = this.parent.attacks;
                            IAudioEngine sound1 = this.sound;
                            SceneBattle parent1 = this.parent;
                            Point point = this.target[0].position;
                            int x1 = point.X;
                            point = this.target[0].position;
                            int y1 = point.Y;
                            int union1 = (int)this.union;
                            int power = this.Power;
                            int speed = this.speed;
                            int element = (int)this.element;
                            BombAttack bombAttack = new BombAttack(sound1, parent1, x1, y1, (Panel.COLOR)union1, power, speed, (ChipBase.ELEMENT)element);
                            attacks.Add(bombAttack);
                            List<EffectBase> effects = this.parent.effects;
                            IAudioEngine sound2 = this.sound;
                            SceneBattle parent2 = this.parent;
                            point = this.target[0].position;
                            int x2 = point.X;
                            point = this.target[0].position;
                            int y2 = point.Y;
                            int union2 = (int)this.union;
                            GunHit gunHit = new GunHit(sound2, parent2, x2, y2, (Panel.COLOR)union2);
                            effects.Add(gunHit);
                            this.parent.effects.Add(new BulletShells(this.sound, this.parent, this.position, this.positionDirect.X - 8 * this.UnionRebirth, this.positionDirect.Y - 24f, 32, this.union, 20 + this.Random.Next(20), 2, 0));
                            this.target[0].flag = false;
                            this.target.RemoveAt(0);
                        }
                        if (this.target.Count <= 0 && !this.attackanimation)
                        {
                            foreach (var crosshair in this.target)
                            {
                                crosshair.ManualFrame = 0;
                            }
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = Riveradar.MOTION.attackEnd;
                        }
                        break;
                    }
                    break;
                case Riveradar.MOTION.attackEnd:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeMoveEND(this.frame);
                        if (this.frame >= 2)
                        {
                            this.motion = Riveradar.MOTION.neutral;
                            this.frame = 0;
                            this.speed = this.nspeed;
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
            int num1 = this.union == Panel.COLOR.blue ? 5 : -5;
            int num2 = (int)this.positionDirect.X + num1;
            Point shake1 = this.Shake;
            int x1 = shake1.X;
            double num3 = num2 + x1;
            int num4 = (int)this.positionDirect.Y - 16;
            shake1 = this.Shake;
            int y1 = shake1.Y;
            double num5 = num4 + y1;
            this._position = new Vector2((float)num3, (float)num5);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = 5 * this.height;
            if (this.Hp <= 0)
            {
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
                foreach (var crosshair in this.target)
                {
                    crosshair.flag = false;
                }

                this.target.Clear();
            }
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
            int num6 = (int)this.positionDirect.X + num1 + 4;
            Point shake2 = this.Shake;
            int x2 = shake2.X;
            double num7 = num6 + x2;
            int y2 = (int)this.positionDirect.Y;
            shake2 = this.Shake;
            int y3 = shake2.Y;
            double num8 = y2 + y3;
            this._position = new Vector2((float)num7, (float)num8);
            this._rect = new Rectangle(10 * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide - 8, this.height);
            if (this.version == 0)
                this._rect.Y = 5 * this.height;
            dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 + 36);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[6] { 0, 1, 2, 3, 4, 5 }, new int[6]
            {
        0,
        1,
        2,
        3,
        4,
        5
            }, 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[2] { 0, 1 }, new int[2]
            {
        0,
        6
            }, 0, waitflame);
        }

        private Point AnimeMoveEND(int waitflame)
        {
            return this.Return(new int[2] { 0, 1 }, new int[2]
            {
        9,
        0
            }, 0, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[14]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        13,
        14,
        15,
        16,
        17,
        18,
        19
            }, new int[14]
            {
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        9,
        8,
        7,
        6,
        5,
        4
            }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attackStart,
            attack,
            attackEnd,
        }
    }
}

