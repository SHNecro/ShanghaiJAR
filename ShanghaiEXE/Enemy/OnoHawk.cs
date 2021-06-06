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
    internal class OnoHawk : EnemyBase
    {
        private OnoHawk.MOTION motion = OnoHawk.MOTION.neutral;
        private int attackProcess;
        private bool attackUP;
        private readonly int nspeed;
        private readonly int moveroop;
        private readonly int attackSpeed;
        private int roopneutral;
        private int roopmove;
        private int animeflame;
        private Shadow shadow;

        public OnoHawk(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "onohawk";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.leaf;
            this.wide = 56;
            this.height = 48;
            this.nspeed = 12 - Math.Min(version * 2, 10);
            this.speed = this.nspeed;
            this.printhp = true;
            this.printNumber = true;
            this.name = ShanghaiEXE.Translate("Enemy.OnoHawkName1");
            switch (this.version)
            {
                case 0:
                    this.power = 200;
                    this.hp = 1400;
                    this.moveroop = 1;
                    this.nspeed = 6;
                    this.speed = this.nspeed;
                    this.name = ShanghaiEXE.Translate("Enemy.OnoHawkName2");
                    this.printNumber = false;
                    this.attackSpeed = 8;
                    break;
                case 1:
                    this.power = 40;
                    this.hp = 90;
                    this.moveroop = 2;
                    this.attackSpeed = 4;
                    break;
                case 2:
                    this.power = 60;
                    this.hp = 140;
                    this.moveroop = 2;
                    this.attackSpeed = 8;
                    break;
                case 3:
                    this.power = 80;
                    this.hp = 200;
                    this.moveroop = 4;
                    this.attackSpeed = 12;
                    break;
                case 4:
                    this.power = 100;
                    this.hp = 250;
                    this.moveroop = 5;
                    this.name = ShanghaiEXE.Translate("Enemy.OnoHawkName3");
                    this.printNumber = false;
                    this.attackSpeed = 15;
                    break;
                default:
                    this.power = 100 + (version - 4) * 20;
                    this.hp = 250 + (version - 4) * 50;
                    this.moveroop = 6;
                    this.attackSpeed = 15;
                    this.name = ShanghaiEXE.Translate("Enemy.OnoHawkName4") + (version - 3).ToString();
                    this.printNumber = false;
                    break;
            }
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.effecting = false;
            this.Flying = true;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new Tomahawk(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new Tomahawk(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new Tomahawk(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new Tomahawk(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new Tomahawk(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new MegaTomahawk(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new MegaTomahawk(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MegaTomahawk(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new MegaTomahawk(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new MegaTomahawk(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new GigaTomahawk(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new GigaTomahawk(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new GigaTomahawk(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new GigaTomahawk(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new GigaTomahawk(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new Tomahawk(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new MegaTomahawk(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new MegaTomahawk(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new GigaTomahawk(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new Tomahawk(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new TomahawkX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0) + 4 * this.UnionRebirth, (float)(position.Y * 24.0 + 60.0));
        }

        public override void InitAfter()
        {
            if (this.parent == null)
                return;
            this.shadow = new Shadow(this.sound, this.parent, this.position.X, this.position.Y, this);
            this.shadow.slide.X = this.union == Panel.COLOR.red ? -8 : 0;
            this.parent.effects.Add(shadow);
            this.shadow.PositionDirectSet(this.position);
        }

        protected override void Moving()
        {
            if (this.shadow != null)
                this.shadow.positionDirect = new Vector2(this.positionDirect.X + 4f, this.positionDirect.Y + 24f);
            this.neutlal = this.motion == OnoHawk.MOTION.neutral;
            switch (this.motion)
            {
                case OnoHawk.MOTION.neutral:
                    if (this.moveflame)
                    {
                        if (this.roopmove <= this.moveroop)
                            this.animationpoint = this.AnimeNeutral(this.frame);
                        else
                            this.animationpoint.X = 4;
                        if (this.frame >= 4)
                        {
                            this.frame = 0;
                            if (this.roopmove <= this.moveroop)
                                this.animationpoint = this.AnimeNeutral(this.frame);
                            ++this.roopneutral;
                            if (this.roopneutral >= 2 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.motion = OnoHawk.MOTION.attack;
                                    this.sound.PlaySE(SoundEffect.knife);
                                    this.effecting = true;
                                    this.counterTiming = false;
                                    this.HitFlagReset();
                                }
                                else
                                    this.motion = OnoHawk.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case OnoHawk.MOTION.move:
                    ++this.roopmove;
                    this.motion = OnoHawk.MOTION.neutral;
                    this.MoveRandom(false, false, this.union, true);
                    if (this.position == this.positionre)
                    {
                        this.motion = OnoHawk.MOTION.neutral;
                        this.frame = 0;
                        this.roopneutral = 0;
                    }
                    else
                    {
                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                        this.position = this.positionre;
                        this.PositionDirectSet();
                        this.frame = 0;
                        this.roopneutral = 0;
                    }
                    if (this.roopmove > this.moveroop)
                    {
                        this.counterTiming = true;
                        this.sound.PlaySE(SoundEffect.chain);
                        break;
                    }
                    break;
                case OnoHawk.MOTION.attack:
                    if (!this.moveflame) { }
                    ++this.animeflame;
                    if (this.animeflame > 8)
                        this.animeflame = 0;
                    this.animationpoint = this.AnimeAttack(this.animeflame);
                    if (this.attackProcess == 0)
                    {
                        if (this.SlideMove(attackSpeed, 0))
                        {
                            this.SlideMoveEnd();
                            if (!this.InAreaCheck(new Point(this.position.X + this.UnionRebirth, this.position.Y)))
                            {
                                switch (this.position.Y)
                                {
                                    case 0:
                                        this.attackUP = false;
                                        this.attackProcess = 1;
                                        break;
                                    case 2:
                                        this.attackUP = true;
                                        this.attackProcess = 1;
                                        break;
                                    default:
                                        this.attackProcess = 2;
                                        break;
                                }
                            }
                            break;
                        }
                        break;
                    }
                    if (this.attackProcess == 1)
                    {
                        if (this.SlideMove(attackSpeed, this.attackUP ? 2 : 3))
                        {
                            this.SlideMoveEnd();
                            if (this.version == 0 && this.position.X == 5 && this.position.Y == 1 && this.Random.Next(2) == 1)
                            {
                                this.HitFlagReset();
                                this.attackProcess = 0;
                            }
                            else if (this.version == 0 && this.position.X == 5)
                            {
                                this.HitFlagReset();
                                this.attackProcess = 0;
                            }
                            else if (!this.InAreaCheck(new Point(this.position.X, this.position.Y + (this.attackUP ? -1 : 1))))
                            {
                                this.HitFlagReset();
                                this.attackProcess = 2;
                            }
                            break;
                        }
                        break;
                    }
                    if (this.SlideMove(attackSpeed, 1))
                    {
                        this.SlideMoveEnd();
                        if (!this.InAreaCheck(new Point(this.position.X - this.UnionRebirth, this.position.Y)))
                        {
                            this.HitFlagReset();
                            this.PositionDirectSet();
                            if (this.version == 0)
                            {
                                switch (this.position.Y)
                                {
                                    case 0:
                                        this.attackUP = false;
                                        this.attackProcess = 1;
                                        break;
                                    case 2:
                                        this.attackUP = true;
                                        this.attackProcess = 1;
                                        break;
                                    default:
                                        switch (this.Random.Next(3))
                                        {
                                            case 0:
                                                this.attackUP = false;
                                                this.attackProcess = 1;
                                                break;
                                            case 2:
                                                this.attackUP = true;
                                                this.attackProcess = 1;
                                                break;
                                            default:
                                                this.attackProcess = 0;
                                                break;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                this.roopmove = 0;
                                this.attackProcess = 0;
                                this.effecting = false;
                                this.motion = OnoHawk.MOTION.move;
                            }
                        }
                    }
                    break;
            }
            this.AttackMake(this.Power, 0, 0);
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
            {
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, 0, this.wide, this.height), this._position, this.picturename);
                this.shadow.flag = false;
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
            this.HPposition = new Vector2((int)this.positionDirect.X - 2 - 2 * this.UnionRebirth, (int)this.positionDirect.Y - this.height / 2 + 48);
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
            return this.Return(new int[9]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        100
            }, new int[8] { 5, 6, 7, 8, 9, 10, 11, 12 }, 0, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
        }
    }
}

