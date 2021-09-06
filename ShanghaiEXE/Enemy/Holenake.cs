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
    internal class Holenake : EnemyBase
    {
        private Holenake.MOTION motion = Holenake.MOTION.neutral;
        private int animepls;
        private readonly int nspeed;
        private readonly int moveroop;
        private bool attackanimation;
        private int roopneutral;
        private int roopmove;

        private int attackFrames;
        private double remainingPerTickDamage;

        public Holenake(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -16;
            this.wantedPosition.X = -8;
            this.wantedPosition.Y = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.HolenakeName1");
                    this.attackFrames = 60;
                    this.power = 270;
                    this.hp = 1200;
                    this.moveroop = 1;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.HolenakeName2");
                    this.attackFrames = 30;
                    this.power = 120;
                    this.hp = 80;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.HolenakeName3");
                    this.attackFrames = 35;
                    this.power = 160;
                    this.hp = 150;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.HolenakeName4");
                    this.attackFrames = 40;
                    this.power = 200;
                    this.hp = 210;
                    this.moveroop = 1;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.HolenakeName5");
                    this.attackFrames = 45;
                    this.power = 240;
                    this.hp = 260;
                    this.moveroop = 1;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.HolenakeName6") + (version - 3).ToString();
                    this.attackFrames = 50;
                    this.power = 240 + (version - 4) * 40;
                    this.hp = 260 + (version - 4) * 40;
                    this.moveroop = 1;
                    break;
            }
            this.picturename = "holenake";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.poison;
            this.Flying = false;
            this.wide = 40;
            this.height = 40;
            this.hpmax = this.hp;
            this.nspeed = 6 - Math.Min(5, (int)this.version);
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.printNumber = false;
            this.effecting = false;
            this.Noslip = true;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.nspeed = 1;
                    this.speed = this.nspeed;
                    this.dropchips[0].chip = new BioSpray1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new GraviBall2(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new GraviBall2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new GraviBall2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new GraviBall2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    break;
                case 1:
                    this.dropchips[0].chip = new BioSpray1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new BioSpray1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new BioSpray1(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new BioSpray1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BioSpray1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new BioSpray2(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new BioSpray2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new BioSpray2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new BioSpray2(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new BioSpray2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new BioSpray3(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new BioSpray3(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new BioSpray3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new BioSpray3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BioSpray3(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new BioSpray1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new BioSpray1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BioSpray2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new BioSpray3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BioSpray1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new BioSprayX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 8.0), (float)(position.Y * 24.0 + 66.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Holenake.MOTION.neutral;
            switch (this.motion)
            {
                case Holenake.MOTION.neutral:
                    if (this.moveflame && this.frame >= 4)
                    {
                        this.frame = 0;
                        ++this.roopneutral;
                        if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove > this.moveroop && !this.badstatus[4])
                            {
                                this.motion = Holenake.MOTION.attack;
                                this.counterTiming = true;
                            }
                            else
                            {
                                this.speed = 4 - Math.Min(3, (int)this.version);
                                if (this.version == 0)
                                    this.speed = 1;
                                this.motion = Holenake.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case Holenake.MOTION.move:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeMove(this.frame);
                        switch (this.frame)
                        {
                            case 8:
                                this.nohit = true;
                                break;
                            case 16:
                                this.MoveRandom(false, false);
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                break;
                            case 24:
                                this.nohit = false;
                                break;
                            case 32:
                                ++this.roopmove;
                                this.frame = 0;
                                this.speed = this.nspeed;
                                this.motion = Holenake.MOTION.neutral;
                                break;
                        }
                        break;
                    }
                    break;
                case Holenake.MOTION.attack:
                    if (this.frame >= 7)
                    {
                        var actualPoisonGasAttacks = (this.speed * this.attackFrames) - ((this.speed * 7) - 1);
                        var actualPerTickDamage = (double)this.Power / actualPoisonGasAttacks;
                        this.remainingPerTickDamage += actualPerTickDamage;
                        var damageThisTick = (this.frame >= this.attackFrames) ? (int)Math.Round(this.remainingPerTickDamage) : (int)Math.Floor(this.remainingPerTickDamage);
                        this.remainingPerTickDamage -= damageThisTick;
                        var po = damageThisTick;

                        this.parent.attacks.Add(new PoisonGas(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth, this.position.Y - 1, this.union, po, this.moveflame, this.Element));
                        this.parent.attacks.Add(new PoisonGas(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth, this.position.Y, this.union, po, this.moveflame, this.Element));
                        this.parent.attacks.Add(new PoisonGas(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth, this.position.Y + 1, this.union, po, this.moveflame, this.Element));
                        if (this.version == 0)
                        {
                            this.parent.attacks.Add(new PoisonGas(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y - 1, this.union, po, this.moveflame, this.Element));
                            this.parent.attacks.Add(new PoisonGas(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y, this.union, po, this.moveflame, this.Element));
                            this.parent.attacks.Add(new PoisonGas(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth, this.position.Y + 1, this.union, po, this.moveflame, this.Element));
                        }
                        else if (this.version > 1)
                            this.parent.attacks.Add(new PoisonGas(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, po, this.moveflame, this.Element));
                    }
                    if (this.moveflame)
                    {
                        this.animationpoint.X = this.attackanimation ? this.animepls : this.animepls + 1;
                        this.attackanimation = !this.attackanimation;
                        if (this.frame == 7)
                        {
                            this.counterTiming = false;
                            this.animepls = 1;
                        }
                        if (this.frame >= 7)
                            this.sound.PlaySE(SoundEffect.lance);
                        if (this.frame >= this.attackFrames)
                        {
                            this.animationpoint.X = 0;
                            this.animepls = 0;
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = Holenake.MOTION.neutral;
                        }
                        break;
                    }
                    break;
            }
            if (this.motion != Holenake.MOTION.attack) { }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void Render(IRenderer dg)
        {
            int num1 = (int)this.positionDirect.X + (this.union == Panel.COLOR.blue ? 5 : 22);
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
                this._rect.Y = this.animationpoint.Y;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y - this.height / 2 + 12);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.ReturnKai(new int[36]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1
            }, new int[32]
            {
        0,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        15,
        16,
        17,
        17,
        16,
        15,
        14,
        13,
        12,
        11,
        10,
        9,
        8,
        7,
        6,
        5,
        4,
        3,
        0
            }, 1, waitflame);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 0, waitflame);
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

