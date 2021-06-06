using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    internal class BakeBake : EnemyBase
    {
        private BakeBake.MOTION motion = BakeBake.MOTION.neutral;
        private readonly List<BakeBake> bakes = new List<BakeBake>();
        private readonly int nspeed;
        private readonly int moveroop;
        private int hpspeed;
        private int roopneutral;
        private readonly int roopmove;
        private bool attackflag;

        public BakeBake(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "bakebake";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.normal;
            this.wide = 32;
            this.height = 48;
            this.nspeed = Math.Max(12 - version * 2, 3);
            this.speed = this.nspeed;
            this.printhp = true;
            this.printNumber = false;
            switch (this.version)
            {
                case 0:
                    this.power = 150;
                    this.hp = 250;
                    this.moveroop = 8;
                    this.nspeed = 8;
                    this.speed = 8;
                    this.name = ShanghaiEXE.Translate("Enemy.BakeBakeName1");
                    break;
                case 1:
                    this.power = 20;
                    this.hp = 100;
                    this.moveroop = 8;
                    this.name = ShanghaiEXE.Translate("Enemy.BakeBakeName2");
                    break;
                case 2:
                    this.power = 60;
                    this.hp = 130;
                    this.moveroop = 6;
                    this.name = ShanghaiEXE.Translate("Enemy.BakeBakeName3");
                    break;
                case 3:
                    this.power = 80;
                    this.hp = 160;
                    this.moveroop = 4;
                    this.name = ShanghaiEXE.Translate("Enemy.BakeBakeName4");
                    break;
                case 4:
                    this.power = 100;
                    this.hp = 200;
                    this.moveroop = 5;
                    this.name = ShanghaiEXE.Translate("Enemy.BakeBakeName5");
                    this.printNumber = false;
                    break;
                default:
                    this.power = 200;
                    this.hp = 250 + (version - 4) * 50;
                    this.moveroop = 6;
                    this.name = ShanghaiEXE.Translate("Enemy.BakeBakeName6") + (version - 3).ToString();
                    this.printNumber = false;
                    break;
            }
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.effecting = false;
            this.Flying = true;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new GhostBody(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GhostBody(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new GhostBody(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new GhostBody(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new GhostBody(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new ShadowBody(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ShadowBody(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ShadowBody(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ShadowBody(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ShadowBody(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new SynchroBody(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new SynchroBody(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SynchroBody(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new SynchroBody(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SynchroBody(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new GhostBody(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new GhostBody(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new ShadowBody(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new SynchroBody(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new GhostBody(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
            }
        }

        public override void InitAfter()
        {
            for (int index = 0; index < this.Parent.enemys.Count; ++index)
            {
                if (index != this.number && (this.Parent.enemys[index] is BakeBake && this.Parent.enemys[index].version == 0))
                    this.bakes.Add((BakeBake)this.Parent.enemys[index]);
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 62.0));
        }

        public override void Updata()
        {
            base.Updata();

            ++this.hpspeed;
            if (this.hpspeed > (8 - Math.Min(7, (int)this.version)) * (this.version > 2 || this.version <= 0 ? 1 : 2))
            {
                this.hpspeed = 0;
                ++this.Hp;
            }
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == BakeBake.MOTION.neutral;

            switch (this.motion)
            {
                case BakeBake.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 4)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= this.moveroop && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                this.nohit = true;
                                this.attackflag = !this.badstatus[4];
                                this.speed /= 2;
                                this.motion = BakeBake.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case BakeBake.MOTION.move:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeMove(this.frame);
                        if (this.frame == 4)
                        {
                            if (this.attackflag && this.parent.nowscene != SceneBattle.BATTLESCENE.end && !this.HeviSand)
                            {
                                Point point = this.RandomTarget();
                                this.positionre = point;
                                this.positionre.X -= this.UnionRebirth;
                                if (!this.NoObject(new Point(point.X - this.UnionRebirth, point.Y), this.number))
                                    this.MoveRandom(true, false);
                            }
                            else
                                this.MoveRandom(false, false);
                            this.position = this.positionre;
                            this.PositionDirectSet();
                        }
                        if (this.frame == 8)
                        {
                            this.frame = 0;
                            this.nohit = false;
                            if (this.attackflag)
                            {
                                this.effecting = true;
                                this.sound.PlaySE(SoundEffect.bound);
                                this.motion = BakeBake.MOTION.attack;
                                this.counterTiming = true;
                            }
                            else
                            {
                                this.motion = BakeBake.MOTION.neutral;
                                this.speed = this.nspeed;
                            }
                        }
                        break;
                    }
                    break;
                case BakeBake.MOTION.attack:
                    this.animationpoint = this.AnimeAttack(this.frame);
                    if (this.moveflame)
                    {
                        if (this.frame == 3)
                        {
                            AttackBase attackBase = new BombAttack(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.Power, 2, 0, this.element);
                            attackBase.badstatus[4] = true;
                            attackBase.badstatustime[4] = 300;
                            this.parent.attacks.Add(attackBase);
                        }
                        if (this.frame == 7)
                        {
                            this.nohit = true;
                            this.attackflag = false;
                            this.counterTiming = false;
                            this.effecting = false;
                            this.frame = 0;
                            this.motion = BakeBake.MOTION.move;
                        }
                        break;
                    }
                    break;
            }
            if (this.effecting && !this.nohit)
                this.AttackMake(this.Power, 0, 0);
            this.FlameControl();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            int num1 = (int)this.positionDirect.X + (this.union == Panel.COLOR.blue ? 5 : 5);
            Point shake = this.Shake;
            int x = shake.X;
            double num2 = num1 + x;
            int y1 = (int)this.positionDirect.Y;
            shake = this.Shake;
            int y2 = shake.Y;
            double num3 = y1 + y2;
            this._position = new Vector2((float)num2, (float)num3);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, Math.Min(this.version, (byte)4) * this.height, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y = 5 * this.height;
            if (this.Hp <= 0)
            {
                bool flag = true;
                if (this.version == 0)
                {
                    foreach (CharacterBase bake in this.bakes)
                    {
                        if (bake.Hp > 0)
                            flag = false;
                    }
                }
                if (flag)
                    this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
                else
                    this.hp = 0;
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
            this.printhp = !this.nohit;
            this.HPposition = new Vector2((int)this.positionDirect.X + 4, (int)this.positionDirect.Y - this.height / 2 - 3);
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

        private Point AnimeMove(int waitflame)
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
            }, new int[11] { 0, 6, 7, 8, 9, 10, 9, 8, 7, 6, 0 }, 0, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 4, 5, 6 }, new int[7]
            {
        0,
        3,
        4,
        5,
        4,
        3,
        0
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

