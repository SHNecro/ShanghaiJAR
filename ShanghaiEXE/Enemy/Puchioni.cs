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
    internal class Puchioni : EnemyBase
    {
        private Puchioni.MOTION motion = Puchioni.MOTION.neutral;
        private readonly int nspeed;
        private int roopneutral;
        private int roopmove;

        public Puchioni(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.PuchioniName1");
                    this.power = 10;
                    this.hp = 50;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.PuchioniName2");
                    this.power = 30;
                    this.hp = 80;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.PuchioniName3");
                    this.power = 50;
                    this.hp = 120;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.PuchioniName4");
                    this.power = 80;
                    this.hp = 150;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.PuchioniName5") + (version - 3).ToString();
                    this.power = 80 + (version - 4) * 20;
                    this.hp = 150 + (version - 4) * 20;
                    break;
            }
            this.picturename = "putioni";
            this.race = EnemyBase.ENEMY.virus;
            this.element = ChipBase.ELEMENT.eleki;
            this.Flying = true;
            this.wide = 40;
            this.height = 48;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = n * 3;
            this.nspeed = 12 - version * 2;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.PuchioniName6");
                    this.hp = 900;
                    this.hpmax = this.hp;
                    this.power = 150;
                    this.hpprint = this.hp;
                    this.speed = 1;
                    this.printNumber = false;
                    this.dropchips[0].chip = new ElekiChain3(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ElekiChain3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ElekiChain3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ElekiChain3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ElekiChain3(this.sound);
                    this.dropchips[4].codeNo = 3;
                    break;
                case 1:
                    this.dropchips[0].chip = new ElekiChain1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ElekiChain1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ElekiChain1(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ElekiChain1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ElekiChain1(this.sound);
                    if (this.Random.Next(4) < 3)
                        this.dropchips[4].codeNo = 2;
                    else
                        this.dropchips[4].codeNo = 3;
                    this.havezenny = 300;
                    break;
                case 2:
                    this.dropchips[0].chip = new ElekiChain2(this.sound);
                    this.dropchips[0].codeNo = 2;
                    this.dropchips[1].chip = new ElekiChain2(this.sound);
                    this.dropchips[1].codeNo = 3;
                    this.dropchips[2].chip = new ElekiChain2(this.sound);
                    this.dropchips[2].codeNo = 3;
                    this.dropchips[3].chip = new ElekiChain2(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new ElekiChain2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 600;
                    break;
                case 3:
                    this.dropchips[0].chip = new ElekiChain3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new ElekiChain3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ElekiChain3(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ElekiChain3(this.sound);
                    this.dropchips[3].codeNo = 3;
                    this.dropchips[4].chip = new ElekiChain3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 800;
                    break;
                default:
                    this.dropchips[0].chip = new ElekiChain1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new ElekiChain1(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new ElekiChain2(this.sound);
                    this.dropchips[2].codeNo = 2;
                    this.dropchips[3].chip = new ElekiChain3(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new ElekiChain1(this.sound);
                    this.dropchips[4].codeNo = 3;
                    this.havezenny = 800;
                    if (this.version < 8)
                        break;
                    this.dropchips[4].chip = new ElekiChainX(this.sound);
                    this.dropchips[4].codeNo = this.Random.Next(4);
                    this.havezenny = 8000;
                    break;
            }
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 62.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.motion == Puchioni.MOTION.neutral;
            switch (this.motion)
            {
                case Puchioni.MOTION.neutral:
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.frame);
                        int num;
                        if (this.roopmove > 3 || this.version == 0)
                        {
                            num = 0;
                            this.speed = 3;
                        }
                        else
                        {
                            num = 3 - version;
                            this.speed = this.nspeed;
                        }
                        if (this.frame >= 4 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.frame = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= num)
                            {
                                this.roopneutral = 0;
                                this.motion = Puchioni.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case Puchioni.MOTION.move:
                    ++this.roopmove;
                    if ((this.version > 0 && this.roopmove > 8 || this.version == 0 && this.roopmove > 3) && !this.badstatus[4])
                    {
                        this.MoveRandom(true, false);
                        this.motion = Puchioni.MOTION.attack;
                        this.counterTiming = true;
                    }
                    else
                    {
                        this.MoveRandom(false, false);
                        this.motion = Puchioni.MOTION.neutral;
                    }
                    if (this.position == this.positionre)
                    {
                        this.motion = Puchioni.MOTION.neutral;
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
                case Puchioni.MOTION.attack:
                    if (this.moveflame)
                    {
                        if (this.frame == 1 && this.version > 0)
                            this.speed = 4;
                        if (this.version > 0)
                            this.animationpoint = this.AnimeAttack(this.frame);
                        else
                            this.animationpoint = this.AnimeAttack2(this.frame);
                        if (this.version > 0 && this.frame == 10 || this.version == 0 && this.frame == 6)
                        {
                            this.parent.attacks.Add(new ChainAttack(this.sound, this.parent, this.union == Panel.COLOR.blue ? this.position.X - 1 : this.position.X + 1, this.position.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.speed, this.version > 0 ? Math.Min(version - 1, 2) : 0, false, this.element));
                            this.counterTiming = false;
                        }
                        if (this.frame >= 23 || this.version == 0 && this.frame >= 17)
                        {
                            if (this.version == 0)
                                this.parent.attacks.Add(new EriaSteel(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, 10, ChipBase.ELEMENT.normal));
                            this.frame = 0;
                            this.roopmove = 0;
                            this.roopneutral = 0;
                            this.speed = this.nspeed;
                            this.motion = Puchioni.MOTION.neutral;
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
                this._rect.Y = 0;
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
            }, new int[8] { 1, 2, 3, 4, 3, 2, 1, 0 }, 0, waitflame);
        }

        private Point AnimeAttack(int waitflame)
        {
            return this.Return(new int[14]
            {
        0,
        1,
        2,
        3,
        8,
        9,
        10,
        17,
        18,
        19,
        20,
        21,
        22,
        23
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

        private Point AnimeAttack2(int waitflame)
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

