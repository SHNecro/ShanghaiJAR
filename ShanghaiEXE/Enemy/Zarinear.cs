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
    internal class Zarinear : EnemyBase
    {
        private Zarinear.MOTION motion = Zarinear.MOTION.neutral;
        private readonly ClossBomb.TYPE type;
        private int targetX;
        private int roopneutral;

        public Zarinear(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            this.helpPosition.X = -8;
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = "zarinear";
            this.race = EnemyBase.ENEMY.virus;
            this.Flying = false;
            this.wide = 48;
            this.height = 40;
            this.printNumber = false;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.ZarinearName1");
                    this.power = 150;
                    this.hp = 1000;
                    this.speed = 3;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.ZarinearName2");
                    this.power = 20;
                    this.hp = 70;
                    this.speed = 7;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.ZarinearName3");
                    this.power = 60;
                    this.hp = 140;
                    this.speed = 5;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.ZarinearName4");
                    this.power = 120;
                    this.hp = 230;
                    this.speed = 4;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.ZarinearName5");
                    this.power = 150;
                    this.hp = 260;
                    this.speed = 3;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.ZarinearName6") + (version - 3).ToString();
                    this.power = 150 + (version - 4) * 20;
                    this.hp = 260 + (version - 4) * 20;
                    this.speed = 3;
                    break;
            }
            this.roop = this.number;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.PositionDirectSet();
            this.element = ChipBase.ELEMENT.aqua;
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new BubbleBlust1(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new BubbleBlust1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new BubbleBlust1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new BubbleBlust1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BubbleBlust1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 0;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    this.type = ClossBomb.TYPE.single;
                    break;
                case 2:
                    this.dropchips[0].chip = new BubbleBlust2(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new BubbleBlust2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new BubbleBlust2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new BubbleBlust2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new BubbleBlust2(this.sound);
                    this.dropchips[4].codeNo = 2;
                    this.havezenny = 600;
                    this.type = ClossBomb.TYPE.closs;
                    break;
                case 3:
                    this.dropchips[0].chip = new BubbleBlust3(this.sound);
                    this.dropchips[0].codeNo = 3;
                    this.dropchips[1].chip = new BubbleBlust3(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new BubbleBlust3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new BubbleBlust3(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new BubbleBlust3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    this.type = ClossBomb.TYPE.big;
                    break;
                default:
                    this.dropchips[0].chip = new BubbleBlust1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new BubbleBlust1(this.sound);
                    this.dropchips[1].codeNo = 2;
                    this.dropchips[2].chip = new BubbleBlust2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new BubbleBlust3(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new BubbleBlust1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    this.type = ClossBomb.TYPE.big;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new BubbleBlustX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 8000;
                        break;
                    }
                    break;
            }
            this.neutlal = true;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 14.0) - 4 * this.UnionRebirth, (float)(position.Y * 24.0 + 68.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Zarinear.MOTION.neutral;
            if (this.moveflame)
            {
                this.positionre = this.position;
                switch (this.motion)
                {
                    case Zarinear.MOTION.neutral:
                        if (this.moveflame)
                        {
                            this.animationpoint = this.AnimeNeutral(this.frame);
                            if (this.frame >= 20)
                            {
                                this.frame = 0;
                                ++this.roopneutral;
                                if ((this.roopneutral >= 2 || this.version == 0) && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                                {
                                    this.roopneutral = 0;
                                    if (!this.badstatus[4])
                                        this.motion = Zarinear.MOTION.up;
                                }
                            }
                            break;
                        }
                        break;
                    case Zarinear.MOTION.move:
                        this.motion = Zarinear.MOTION.neutral;
                        this.MoveRandom(false, false);
                        if (this.position == this.positionre)
                        {
                            this.frame = 0;
                            this.motion = Zarinear.MOTION.neutral;
                            this.roopneutral = 0;
                            break;
                        }
                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                        this.position = this.positionre;
                        this.PositionDirectSet();
                        this.frame = 0;
                        this.roopneutral = 0;
                        break;
                    case Zarinear.MOTION.attack:
                        this.animationpoint.X = this.AnimeAttack(this.frame).X;
                        switch (this.frame)
                        {
                            case 1:
                                this.sound.PlaySE(SoundEffect.water);
                                if (this.version == 0)
                                {
                                    Point end = new Point(this.targetX, this.positionre.Y);
                                    Vector2 v = new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 32f : -16f), this.positionDirect.Y - 6f);
                                    for (int y = 0; y < 3; ++y)
                                    {
                                        end = new Point(this.targetX, y);
                                        this.parent.attacks.Add(new BubbleBomb(this.sound, this.parent, this.targetX, this.positionre.Y, this.union, this.Power, 1, v, end, 40));
                                    }
                                    break;
                                }
                                Point end1 = new Point(this.targetX, this.positionre.Y);
                                this.parent.attacks.Add(new BubbleBomb(this.sound, this.parent, this.targetX, this.positionre.Y, this.union, this.Power, 1, new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 32f : -16f), this.positionDirect.Y - 6f), end1, 40));
                                break;
                            case 2:
                                this.targetX += this.union == Panel.COLOR.red ? 1 : -1;
                                this.frame = 0;
                                if (this.targetX < 0 || this.targetX > 5)
                                {
                                    this.animationpoint.X = 0;
                                    this.roop = 0;
                                    this.motion = Zarinear.MOTION.down;
                                    this.animationpoint = this.AnimeDown(this.frame);
                                    this.counterTiming = false;
                                    break;
                                }
                                break;
                        }
                        break;
                    case Zarinear.MOTION.up:
                        if (this.moveflame)
                        {
                            this.animationpoint = this.AnimeUP(this.frame);
                            if (this.frame >= 7)
                            {
                                this.targetX = this.position.X + (this.union == Panel.COLOR.red ? 1 : -1);
                                this.frame = 0;
                                this.motion = Zarinear.MOTION.attack;
                                this.counterTiming = true;
                            }
                            break;
                        }
                        break;
                    case Zarinear.MOTION.down:
                        if (this.moveflame)
                        {
                            this.animationpoint = this.AnimeDown(this.frame);
                            if (this.frame >= 7)
                            {
                                this.frame = 0;
                                this.motion = Zarinear.MOTION.move;
                            }
                            break;
                        }
                        break;
                }
            }
            this.MoveAftar();
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + 6 + this.Shake.X, (int)this.positionDirect.Y + 2 + this.Shake.Y);
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
                this._rect.Y = 0;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2(this.positionDirect.X + 6f, this.positionDirect.Y + 2f - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[14]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        9,
        10,
        11,
        12,
        13,
        14,
        15,
        19
            }, new int[14]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        5,
        4,
        3,
        2,
        1,
        0,
        0
            }, 1, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[3] { 0, 1, 2 }, new int[3]
            {
        13,
        14,
        13
            }, 1, waitflame);
        }

        private Point AnimeUP(int waitflame)
        {
            return this.Return(new int[8]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7
            }, new int[8] { 7, 7, 8, 9, 10, 11, 12, 13 }, 1, waitflame);
        }

        private Point AnimeDown(int waitflame)
        {
            return this.Return(new int[8]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7
            }, new int[8] { 13, 12, 11, 10, 9, 8, 7, 7 }, 1, waitflame);
        }

        private enum MOTION
        {
            neutral,
            move,
            attack,
            up,
            down,
        }
    }
}

