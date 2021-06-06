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
    internal class BibityBat : EnemyBase
    {
        private BibityBat.MOTION motion = BibityBat.MOTION.neutral;
        private readonly int nspeed;
        private readonly int moveroop;
        private bool attackanimation;
        private int roopneutral;
        private int roopmove;

        public BibityBat(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "bibitybat";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.earth;
            this.wide = 56;
            this.height = 48;
            this.nspeed = Math.Max(12 - version * 2, 3);
            this.speed = this.nspeed;
            this.printhp = true;
            this.printNumber = true;
            this.name = ShanghaiEXE.Translate("Enemy.BibityBatName1");
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.BibityBatName2");
                    this.nspeed = 6;
                    this.speed = this.nspeed;
                    this.power = 100;
                    this.hp = 1800;
                    this.moveroop = 0;
                    break;
                case 1:
                    this.power = 20;
                    this.hp = 90;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.power = 50;
                    this.hp = 140;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.power = 80;
                    this.hp = 200;
                    this.moveroop = 4;
                    break;
                case 4:
                    this.power = 100;
                    this.hp = 250;
                    this.moveroop = 4;
                    this.name = ShanghaiEXE.Translate("Enemy.BibityBatName3");
                    this.printNumber = false;
                    break;
                default:
                    this.power = 100 + (version - 4) * 20;
                    this.hp = 250 + (version - 4) * 50;
                    this.moveroop = 4;
                    this.name = ShanghaiEXE.Translate("Enemy.BibityBatName4") + (version - 3).ToString();
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
                    this.dropchips[0].chip = new GraviBall1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new GraviBall1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new GraviBall1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new GraviBall1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new GraviBall1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new GraviBall2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new GraviBall2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new GraviBall2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new GraviBall2(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new GraviBall2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new GraviBall3(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GraviBall3(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new GraviBall3(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new GraviBall3(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new GraviBall3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new GraviBall1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GraviBall2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new GraviBall2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new GraviBall3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new GraviBallX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new GraviBall1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GraviBall2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new GraviBall2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new GraviBall3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new GraviBall1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 62.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == BibityBat.MOTION.neutral;
            switch (this.motion)
            {
                case BibityBat.MOTION.neutral:
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
                                    this.motion = BibityBat.MOTION.attack;
                                    this.counterTiming = true;
                                }
                                else
                                    this.motion = BibityBat.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case BibityBat.MOTION.move:
                    ++this.roopmove;
                    this.motion = BibityBat.MOTION.neutral;
                    if (this.version == 0)
                    {
                        this.positionre = new Point(this.union == Panel.COLOR.blue ? 5 : 0, this.Random.Next(3));
                        this.motion = BibityBat.MOTION.attack;
                    }
                    else
                        this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
                        this.motion = BibityBat.MOTION.neutral;
                        this.frame = 0;
                        this.roopneutral = 0;
                        break;
                    }
                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                    this.position = this.positionre;
                    this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 62.0));
                    this.frame = 0;
                    this.roopneutral = 0;
                    break;
                case BibityBat.MOTION.attack:
                    this.animationpoint.X = this.attackanimation ? 4 : 3;
                    this.attackanimation = !this.attackanimation;
                    if (this.moveflame)
                    {
                        if (this.frame == 1)
                        {
                            GravityBallAttack gravityBallAttack = new GravityBallAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, 2, this.positionDirect, this.element);
                            if (this.version == 0)
                            {
                                gravityBallAttack.Xspeed = 8f;
                                gravityBallAttack.Yspeed = 2f;
                                gravityBallAttack.breaking = true;
                            }
                            this.parent.attacks.Add(gravityBallAttack);
                        }
                        if (this.frame == 7)
                        {
                            this.counterTiming = false;
                            int num = this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1;
                        }
                        if (this.frame >= 14)
                        {
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = BibityBat.MOTION.neutral;
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
            int num1 = (int)this.positionDirect.X + (this.union == Panel.COLOR.blue ? 5 : -5);
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

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[4] { 0, 1, 2, 3 }, new int[4]
            {
        0,
        1,
        2,
        1
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
            attack,
        }
    }
}

