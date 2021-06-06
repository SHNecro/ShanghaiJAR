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
    internal class FireCat : EnemyBase
    {
        private FireCat.MOTION motion = FireCat.MOTION.neutral;
        private readonly int nspeed;
        private int attackcount;
        private readonly int moveroop;
        private bool attackanimation;
        private int roopneutral;
        private int roopmove;

        public FireCat(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "firecat";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.heat;
            this.wide = 48;
            this.height = 24;
            this.nspeed = 8 - version * 2;
            this.speed = this.nspeed;
            this.printhp = true;
            this.printNumber = true;
            this.name = ShanghaiEXE.Translate("Enemy.FireCatName1");
            switch (this.version)
            {
                case 0:
                    this.power = 200;
                    this.hp = 1000;
                    this.nspeed = 1;
                    this.speed = this.nspeed;
                    this.moveroop = 5;
                    this.name = ShanghaiEXE.Translate("Enemy.FireCatName2");
                    this.printNumber = false;
                    break;
                case 1:
                    this.power = 30;
                    this.hp = 90;
                    this.moveroop = 4;
                    break;
                case 2:
                    this.power = 60;
                    this.hp = 140;
                    this.moveroop = 4;
                    break;
                case 3:
                    this.power = 80;
                    this.hp = 200;
                    this.moveroop = 4;
                    break;
                case 4:
                    this.power = 100;
                    this.hp = 250;
                    this.moveroop = 5;
                    this.name = ShanghaiEXE.Translate("Enemy.FireCatName3");
                    this.printNumber = false;
                    break;
                default:
                    this.power = 100 + (version - 4) * 20;
                    this.hp = 250 + (version - 4) * 50;
                    this.moveroop = 6;
                    this.name = ShanghaiEXE.Translate("Enemy.FireCatName4") + (version - 3).ToString();
                    this.printNumber = false;
                    break;
            }
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.effecting = false;
            this.Flying = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new FireArm1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new FireArm1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new FireArm1(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new FireArm1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new FireArm1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new FireArm2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new FireArm2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new FireArm2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new FireArm2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new FireArm2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new FireArm3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new FireArm3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new FireArm3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new FireArm3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new FireArm3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                case 8:
                    this.dropchips[0].chip = new FireArm1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new FireArm2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new FireArm2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new FireArm3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new FireArmX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
                default:
                    this.dropchips[0].chip = new FireArm1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new FireArm2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new FireArm2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new FireArm3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new FireArm1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0) + 2 * this.UnionRebirth, (float)(position.Y * 24.0 + 76.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == FireCat.MOTION.neutral;
            switch (this.motion)
            {
                case FireCat.MOTION.neutral:
                    if (this.moveflame)
                    {
                        if (this.roopmove < this.moveroop / 2)
                            this.animationpoint = this.AnimeNeutral(this.frame);
                        else
                            this.animationpoint = this.AnimeNeutral2(this.frame);
                        if (this.frame >= 6)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                this.motion = FireCat.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case FireCat.MOTION.move:
                    ++this.roopmove;
                    if (this.roopmove >= this.moveroop && !this.badstatus[4])
                    {
                        this.attackcount = 1;
                        this.speed = 3;
                        this.motion = FireCat.MOTION.attack;
                        this.counterTiming = true;
                    }
                    else
                        this.motion = FireCat.MOTION.neutral;
                    this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
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
                case FireCat.MOTION.attack:
                    if (this.moveflame)
                    {
                        this.animationpoint.X = this.attackanimation ? 9 : 8;
                        this.attackanimation = !this.attackanimation;
                        if (this.frame == 12 || this.frame == 15 || this.frame == 18)
                        {
                            this.counterTiming = false;
                            this.sound.PlaySE(SoundEffect.fire);
                            if (this.version == 0)
                            {
                                AttackBase attackBase1 = new ElementFire(this.sound, this.parent, this.position.X + this.attackcount * this.UnionRebirth, this.position.Y, this.union, this.Power, 4, this.element, false, 1);
                                attackBase1.positionDirect.Y += 16f;
                                attackBase1.breaking = true;
                                this.parent.attacks.Add(attackBase1);
                                if (this.frame >= 15)
                                {
                                    AttackBase attackBase2 = new ElementFire(this.sound, this.parent, this.position.X + this.attackcount * this.UnionRebirth, this.position.Y - 1, this.union, this.Power, 4, this.element, false, 1);
                                    attackBase2.positionDirect.Y += 16f;
                                    attackBase2.breaking = true;
                                    this.parent.attacks.Add(attackBase2);
                                    AttackBase attackBase3 = new ElementFire(this.sound, this.parent, this.position.X + this.attackcount * this.UnionRebirth, this.position.Y + 1, this.union, this.Power, 4, this.element, false, 1);
                                    attackBase3.positionDirect.Y += 16f;
                                    attackBase3.breaking = true;
                                    this.parent.attacks.Add(attackBase3);
                                }
                            }
                            else
                            {
                                AttackBase attackBase = new ElementFire(this.sound, this.parent, this.position.X + this.attackcount * this.UnionRebirth, this.position.Y, this.union, this.Power, 18, this.element, false, 0);
                                attackBase.positionDirect.X -= 48 * this.UnionRebirth;
                                attackBase.positionDirect.Y += 16f;
                                attackBase.breaking = true;
                                this.parent.attacks.Add(attackBase);
                            }
                            ++this.attackcount;
                        }
                        if (this.frame == 6)
                        {
                            this.counterTiming = true;
                            int num = this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1;
                        }
                        if (this.frame >= 45)
                        {
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = FireCat.MOTION.neutral;
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
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y - this.height / 2 - 8);
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
        2,
        1
            }, 1, waitflame);
        }

        private Point AnimeNeutral2(int waitflame)
        {
            return this.Return(new int[6] { 0, 1, 2, 3, 4, 5 }, new int[6]
            {
        4,
        5,
        6,
        7,
        6,
        5
            }, 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[2] { 4, 4 }, new int[2]
            {
        8,
        9
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

