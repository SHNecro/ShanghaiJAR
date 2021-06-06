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
    internal class Mantiser : EnemyBase
    {
        private Mantiser.MOTION motion = Mantiser.MOTION.neutral;
        private readonly int nspeed;
        private readonly int moveroop;
        private readonly bool attackanimation;
        private int roopneutral;
        private int roopmove;
        private Point t;

        public Mantiser(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "mantiser";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.poison;
            this.wide = 48;
            this.height = 48;
            this.nspeed = 12 - version * 2;
            this.speed = this.nspeed;
            this.printhp = true;
            this.printNumber = true;
            this.name = ShanghaiEXE.Translate("Enemy.MantiserName1");
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.MantiserName2");
                    this.power = 200;
                    this.hp = 3000;
                    this.moveroop = 1;
                    this.nspeed = 6;
                    this.speed = this.nspeed;
                    this.printNumber = false;
                    break;
                case 1:
                    this.power = 80;
                    this.hp = 140;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.power = 100;
                    this.hp = 220;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.power = 120;
                    this.hp = 330;
                    this.moveroop = 4;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.MantiserName3");
                    this.power = 200;
                    this.hp = 370;
                    this.moveroop = 5;
                    this.printNumber = false;
                    break;
                default:
                    this.power = 200 + (version - 4) * 20;
                    this.hp = 370 + (version - 4) * 50;
                    this.moveroop = 6;
                    this.name = ShanghaiEXE.Translate("Enemy.MantiserName4") + (version - 3).ToString();
                    this.printNumber = false;
                    break;
            }
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.effecting = false;
            this.Flying = true;
            this.roopmove = 1 - n % 3 * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new DeathWiper1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new DeathWiper1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new DeathWiper1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new DeathWiper1(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new DeathWiper1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 1;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new DeathWiper2(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new DeathWiper2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new DeathWiper2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new DeathWiper2(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new DeathWiper2(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new DeathWiper3(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new DeathWiper3(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new DeathWiper3(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new DeathWiper3(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new DeathWiper3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new DeathWiper1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new DeathWiper2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new DeathWiper2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new DeathWiper3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new DeathWiper1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 12.0), (float)(position.Y * 24.0 + 66.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Mantiser.MOTION.neutral;
            switch (this.motion)
            {
                case Mantiser.MOTION.neutral:
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
                                this.motion = Mantiser.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case Mantiser.MOTION.move:
                    ++this.roopmove;
                    this.motion = Mantiser.MOTION.neutral;
                    if (this.roopmove > this.moveroop)
                    {
                        this.MoveRandom(this.version > 0, false, this.union, this.version == 0);
                        this.speed = this.nspeed / 2;
                        this.counterTiming = true;
                        this.motion = Mantiser.MOTION.attack;
                    }
                    else
                    {
                        this.MoveRandom(false, false);
                        this.motion = Mantiser.MOTION.neutral;
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
                case Mantiser.MOTION.attack:
                    this.animationpoint.X = this.AnimeAttack(this.frame).X;
                    if (this.moveflame)
                    {
                        if (this.frame == 8)
                        {
                            this.counterTiming = false;
                            if (this.version == 0)
                            {
                                this.sound.PlaySE(SoundEffect.shoot);
                                this.parent.attacks.Add(new SonicBoom(this.sound, this.parent, this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1, this.position.Y, this.union, this.Power, 8, this.element, true));
                            }
                            else
                            {
                                this.sound.PlaySE(SoundEffect.sword);
                                int pX = this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1;
                                int y = this.position.Y;
                                if (this.version == 0)
                                {
                                    pX = this.t.X;
                                    y = this.t.Y;
                                }
                                this.parent.attacks.Add(new Halberd(this.sound, this.parent, pX, y, this.union, this.Power, 2, this.element, false));
                            }
                        }
                        if (this.frame >= 20)
                        {
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.animationpoint.X = 0;
                            this.motion = Mantiser.MOTION.move;
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
            int num1 = (int)this.positionDirect.X + (this.union == Panel.COLOR.blue ? 5 : -5) + 8 + 8 * this.UnionRebirth;
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
            this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y - this.height / 2 + 54);
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

        private Point AnimeAttack(int waitflame)
        {
            return this.ReturnKai(new int[5] { 8, 1, 1, 1, 10 }, new int[5]
            {
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

