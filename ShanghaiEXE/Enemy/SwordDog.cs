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
    internal class SwordDog : EnemyBase
    {
        private SwordDog.MOTION motion = SwordDog.MOTION.neutral;
        private readonly int nspeed;
        private readonly int moveroop;
        private readonly bool attackanimation;
        private int roopneutral;
        private int roopmove;
        private Point t;

        public SwordDog(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.Y = -16;
            this.wantedPosition.X = -4;
            this.wantedPosition.Y = -16;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "swordog";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.normal;
            this.wide = 64;
            this.height = 80;
            this.nspeed = 12 - version * 2;
            if (this.nspeed < 4)
                this.nspeed = 4;
            this.speed = this.nspeed;
            this.printhp = true;
            this.printNumber = false;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.SwordDogName1");
                    this.power = 200;
                    this.hp = 1000;
                    this.moveroop = 1;
                    this.nspeed = 8;
                    this.speed = this.nspeed;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.SwordDogName2");
                    this.power = 40;
                    this.hp = 90;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.SwordDogName3");
                    this.power = 100;
                    this.hp = 140;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.SwordDogName4");
                    this.power = 100;
                    this.hp = 260;
                    this.moveroop = 4;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.SwordDogName5");
                    this.power = 140;
                    this.hp = 300;
                    this.moveroop = 5;
                    this.printNumber = false;
                    break;
                default:
                    this.power = 140 + (version - 4) * 20;
                    this.hp = 300 + (version - 4) * 50;
                    this.moveroop = 6;
                    this.name = ShanghaiEXE.Translate("Enemy.SwordDogName6") + (version - 3).ToString();
                    this.printNumber = false;
                    break;
            }
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = 1 - n % 3 * 2;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new CrossSword(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new CrossSword(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new CrossSword(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new CrossSword(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new CrossSword(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new ThujigiriSword(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ThujigiriSword(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new ThujigiriSword(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ThujigiriSword(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new ThujigiriSword(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new ThujigiriCross(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new ThujigiriCross(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new ThujigiriCross(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ThujigiriCross(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ThujigiriCross(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new CrossSword(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ThujigiriSword(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ThujigiriSword(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new ThujigiriCross(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new CrossSword(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new ThujigiriSwordX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 48.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == SwordDog.MOTION.neutral;
            switch (this.motion)
            {
                case SwordDog.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        if (this.frame >= 4)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end || this.version == 0)
                            {
                                this.roopneutral = 0;
                                this.motion = SwordDog.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case SwordDog.MOTION.move:
                    ++this.roopmove;
                    this.motion = SwordDog.MOTION.neutral;
                    if (this.roopmove > this.moveroop && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                    {
                        Point point = this.RandomTarget();
                        this.positionre = point;
                        this.positionre.X -= this.UnionRebirth;
                        if (!this.Canmove(new Point(point.X - this.UnionRebirth, point.Y), this.number, this.UnionEnemy) || this.version <= 1)
                            this.MoveRandom(this.version > 0, false);
                        this.speed = this.nspeed / 2;
                        this.counterTiming = true;
                        this.motion = SwordDog.MOTION.attack;
                        if (this.version == 0)
                        {
                            this.t = this.RandomPanel(this.UnionEnemy);
                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.t.X, this.t.Y, this.union, new Point(), 8 * this.speed, true));
                        }
                    }
                    else
                    {
                        this.MoveRandom(false, false);
                        this.motion = SwordDog.MOTION.neutral;
                    }
                    this.PositionDirectSet();
                    if (this.position == this.positionre)
                    {
                        this.frame = 0;
                        this.roopneutral = 0;
                        this.position = this.positionre;
                        break;
                    }
                    this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                    this.position = this.positionre;
                    this.PositionDirectSet();
                    this.frame = 0;
                    this.roopneutral = 0;
                    break;
                case SwordDog.MOTION.attack:
                    this.animationpoint.X = this.AnimeAttack(this.frame).X;
                    if (this.moveflame)
                    {
                        if (this.frame == 8)
                        {
                            this.counterTiming = false;
                            this.sound.PlaySE(SoundEffect.sword);
                            int pX = this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1;
                            int y = this.position.Y;
                            if (this.version == 0)
                            {
                                pX = this.t.X;
                                y = this.t.Y;
                            }
                            if (version % 2 == 1)
                                this.parent.attacks.Add(new SwordCloss(this.sound, this.parent, pX, y, this.union, this.Power, 2, this.element, false));
                            else if (this.version > 0)
                            {
                                this.parent.attacks.Add(new SwordAttack(this.sound, this.parent, pX, y, this.union, this.Power, 2, this.element, false, false));
                            }
                            else
                            {
                                this.parent.attacks.Add(new SwordAttack(this.sound, this.parent, pX, y, this.union, this.Power, 2, this.element, false, false));
                                this.parent.attacks.Add(new SwordCloss(this.sound, this.parent, pX, y, this.union, this.Power, 2, this.element, false));
                            }
                        }
                        if (this.frame >= 20)
                        {
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.animationpoint.X = 0;
                            this.motion = SwordDog.MOTION.move;
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
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 + 32);
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
            return this.ReturnKai(new int[6] { 8, 1, 1, 1, 1, 10 }, new int[6]
            {
        3,
        4,
        5,
        6,
        7,
        8
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

