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
    internal class Cirno : NaviBase
    {
        private readonly int[] powers = new int[5] { 20, 0, 30, 0, 40 };
        private readonly int nspeed = 8;
        private readonly int moveroop;
        private Cirno.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private int atackroop;
        private bool atack;
        private int time;
        private int flyflame;
        private Point target;
        private Vector2 endposition;
        private float movex;
        private float movey;
        private float plusy;
        private float speedy;
        private float plusing;
        private const int startspeed = 6;

        public Cirno(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.aqua;
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.CirnoName1");
                    this.power = 200;
                    this.hp = 2400;
                    this.moveroop = 1;
                    this.version = 3;
                    this.nspeed = 4;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.CirnoName2");
                    this.power = 20;
                    this.hp = 500;
                    this.moveroop = 2;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.CirnoName3");
                    this.power = 60;
                    this.hp = 900;
                    this.moveroop = 2;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.CirnoName4");
                    this.power = 120;
                    this.hp = 1200;
                    this.moveroop = 1;
                    this.nspeed = 6;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.CirnoName5");
                    this.power = 200;
                    this.hp = 1900;
                    this.moveroop = 1;
                    this.nspeed = 4;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.CirnoName6") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 2000 + (version - 4) * 500;
                    this.moveroop = 1;
                    this.nspeed = 4;
                    break;
            }
            this.picturename = "cirno";
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = true;
            this.wide = 40;
            this.height = 56;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.Flying = false;
                    this.dropchips[0].chip = new CirnoV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new CirnoV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new CirnoV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new CirnoV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new CirnoV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new CirnoV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new CirnoV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new CirnoV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new CirnoV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new CirnoV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new CirnoV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new CirnoV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new CirnoV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new CirnoV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new CirnoV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 2500;
                    break;
                default:
                    this.dropchips[0].chip = new CirnoV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new CirnoV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new CirnoV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new CirnoV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new CirnoV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new CirnoX(this.sound);
                        this.dropchips[4].codeNo = this.Random.Next(4);
                        this.havezenny = 18000;
                        break;
                    }
                    break;
            }
            this.motion = NaviBase.MOTION.move;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 58.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
//                    if (this.flying)
//                        this.flying = false;
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame)
                    {
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.waittime >= 4 || this.atack)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    ++this.atackroop;
                                    this.roopneutral = 0;
                                    this.roopmove = 0;
                                    this.speed = 4;
                                    if (!this.atack)
                                    {
                                        int index = this.Random.Next(this.version > 1 ? 3 : 3);
                                        this.attack = (Cirno.ATTACK)index;
                                        this.powerPlus = this.powers[index];
                                    }
                                    if (this.atackroop >= 3)
                                    {
                                        this.atackroop = 0;
                                        if (this.atack)
                                        {
                                            int index = 4;
                                            this.attack = (Cirno.ATTACK)index;
                                            this.powerPlus = this.powers[index];
                                        }
                                        this.atack = !this.atack;
                                    }
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.counterTiming = true;
                                }
                                else
                                {
                                    this.roopneutral = 0;
                                    this.waittime = 0;
                                    if (this.atack)
                                        this.roopmove = this.moveroop + 1;
                                    this.Motion = NaviBase.MOTION.move;
                                }
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame)
                    {
                        switch (this.attack)
                        {
                            case Cirno.ATTACK.iceTower:
                                this.animationpoint = this.AnimeIceTower(this.waittime);
                                switch (this.waittime)
                                {
                                    case 6:
                                        this.counterTiming = false;
                                        int s = 7 - (version - 1) * 3;
                                        if (s <= 0)
                                            s = 1;
                                        this.parent.attacks.Add(new Tower(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, s, this.element));
                                        break;
                                    case 13:
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        if (!this.atack)
                                        {
                                            this.speed = this.nspeed;
                                            break;
                                        }
                                        break;
                                }
                                break;
                            case Cirno.ATTACK.coldShot:
                                this.animationpoint = this.AnimeColdShot(this.waittime);
                                switch (this.waittime)
                                {
                                    case 6:
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.chain);
                                        Point end = this.RandomTarget();
                                        Vector2 v = new Vector2(this.positionDirect.X + (this.union == Panel.COLOR.red ? 0.0f : 0.0f), this.positionDirect.Y - 8f);
                                        this.parent.attacks.Add(new NSAttack.PoisonShot(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, v, ChipBase.ELEMENT.aqua, end, Math.Max(80 / (version / 2 + 1), 30), false, NSAttack.PoisonShot.TYPE.ice));
                                        for (int seed = 0; seed < this.Random.Next(Math.Min((int)this.version, 7)); ++seed)
                                        {
                                            this.MoveRandom(false, false, this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red, seed);
                                            Point positionre = this.positionre;
                                            this.parent.attacks.Add(new NSAttack.PoisonShot(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, this.Power, 1, v, ChipBase.ELEMENT.aqua, positionre, Math.Max(80 / (version / 2 + 1), 30), false, NSAttack.PoisonShot.TYPE.ice));
                                        }
                                        this.positionre = this.position;
                                        break;
                                    case 13:
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        if (!this.atack)
                                        {
                                            this.speed = this.nspeed;
                                            break;
                                        }
                                        break;
                                }
                                break;
                            case Cirno.ATTACK.iceCharge:
                                if (this.waittime <= 4)
                                    this.animationpoint = this.AnimeCharge(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X, this.position.Y, this.union, new Point(6, 0), 10, true));
                                        this.counterTiming = true;
                                        this.sound.PlaySE(SoundEffect.water);
                                        break;
                                    case 4:
                                        this.effecting = true;
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.sand);
                                        break;
                                }
                                break;
                            case Cirno.ATTACK.iceChange:
                                this.animationpoint = this.AnimeColdPress1(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.counterTiming = true;
                                        this.sound.PlaySE(SoundEffect.warp);
//                                        this.flying = true;
                                        break;
                                    case 5:
                                        this.Noslip = true;
                                        this.effecting = true;
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.throwbomb);
                                        this.attack = Cirno.ATTACK.icePress;
                                        this.target = this.RandomTarget();
                                        this.positionre = this.target;
                                        this.time = 80 - Math.Min(10 * version, 50);
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y, this.union, new Point(), this.time, true));
                                        this.Throw();
                                        this.positionre = this.position;
                                        break;
                                }
                                break;
                        }
                    }
                    if (this.attack == Cirno.ATTACK.icePress && this.motion == NaviBase.MOTION.attack)
                    {
                        this.animationpoint = this.AnimeColdPress2(this.waittime % 4);
                        if (this.flyflame >= this.time + 30)
                        {
                            this.effecting = false;
                            this.superArmor = false;
                            this.animationpoint = new Point();
                            this.position = this.positionre;
                            this.PositionDirectSet();
                            this.Noslip = false;
                            this.motion = NaviBase.MOTION.neutral;
                            this.roopneutral = 0;
                            if (!this.atack)
                                this.speed = this.nspeed;
                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.target.X, this.target.Y));
                        }
                        else if (this.flyflame == this.time)
                        {
                            if (!this.parent.panel[this.target.X, this.target.Y].Hole)
                            {
                                this.parent.panel[this.target.X, this.target.Y].Crack();
                                this.ShakeStart(8, 30);
                                this.sound.PlaySE(SoundEffect.clincher);
                                this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 1, this.element));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
                                if (this.version >= 2)
                                {
                                    Point[] pointArray = this.RandomMultiPanel(3, this.UnionEnemy, true);
                                    for (int index = 0; index < 3; ++index)
                                        this.parent.attacks.Add(new IceSpike(this.sound, this.parent, pointArray[index].X, pointArray[index].Y, this.union, this.Power, 1, this.Element));
                                }
                            }
                            else
                            {
                                this.PositionDirectSet();
                                this.motion = NaviBase.MOTION.neutral;
                                this.roopneutral = 0;
                                if (!this.atack)
                                    this.speed = this.nspeed;
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                            }
                        }
                        else if (this.flyflame < this.time)
                        {
                            this.positionDirect.X -= this.movex;
                            this.positionDirect.Y -= this.movey;
                            this.plusy -= this.speedy;
                            this.speedy -= this.plusing;
                            this.nohit = speedy * (double)this.speedy < 25.0;
                            if (speedy < 0.0)
                            {
                                this.position = this.target;
                                this.superArmor = true;
                            }
                        }
                        ++this.flyflame;
                    }
                    if (this.attack == Cirno.ATTACK.iceCharge && this.motion == NaviBase.MOTION.attack)
                    {
                        this.AttackMake(this.Power, 0, 0);
                        if (this.waittime > 4)
                        {
                            this.positionDirect.X += Math.Min(3 + version, 8) * this.UnionRebirth(this.union);
                            this.position.X = this.Calcposition(this.positionDirect, this.height, false).X;
                            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                            {
                                this.HitFlagReset();
                                this.effecting = false;
                                this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                this.motion = NaviBase.MOTION.neutral;
                                this.animationpoint = new Point();
                                this.roopneutral = 0;
                                if (!this.atack)
                                    this.speed = this.nspeed;
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    if (this.moveflame)
                        ++this.waittime;
                    ++this.roopmove;
                    this.Motion = NaviBase.MOTION.neutral;
                    this.MoveRandom(false, false);
                    if (this.position == this.positionre)
                    {
                        this.Motion = NaviBase.MOTION.neutral;
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
                case NaviBase.MOTION.knockback:
                    switch (this.waittime)
                    {
                        case 2:
//                            this.flying = false;
                            this.animationpoint = new Point(6, 0);
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.animationpoint = new Point(6, 0);
                            break;
                        case 15:
                            this.animationpoint = new Point(3, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.Motion = NaviBase.MOTION.neutral;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= this.UnionRebirth(this.union);
                    ++this.waittime;
                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        private void Throw()
        {
            this.endposition = new Vector2(this.positionre.X * 40 + 16, this.positionre.Y * 24 + 72);
            this.movex = (this.positionDirect.X - this.endposition.X) / time;
            this.movey = (this.positionDirect.Y - this.endposition.Y) / time;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.flyflame = 0;
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(6, 0);
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            Point shake;
            if (this.attack == Cirno.ATTACK.icePress && this.motion == NaviBase.MOTION.attack)
            {
                this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y + 16);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height + 48, this.wide, 8);
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                int x1 = (int)this.positionDirect.X;
                shake = this.Shake;
                int x2 = shake.X;
                double num1 = x1 + x2;
                double num2 = (int)this.positionDirect.Y + (double)this.plusy;
                shake = this.Shake;
                double y = shake.Y;
                double num3 = num2 + y + 8.0;
                this._position = new Vector2((float)num1, (float)num3);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height - 8);
            }
            else
            {
                this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
            }
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num + x1;
                int y1 = (this.version < 4 ? 0 : 2) * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y2 = shake.Y;
                int height2 = height1 + y2;
                this._rect = new Rectangle(x2, y1, wide, height2);
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height), this._position, this.picturename);
            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            if (this.attack == Cirno.ATTACK.icePress && this.motion == NaviBase.MOTION.attack)
                this.HPposition = new Vector2((int)this.positionDirect.X, (float)((int)this.positionDirect.Y - this.height / 2 - 3 + (double)this.plusy + 16.0));
            else
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

        private Point AnimeColdShot(int waitflame)
        {
            return this.Return(new int[4] { 5, 6, 7, 13 }, new int[4]
            {
        3,
        4,
        5,
        5
            }, 0, waitflame);
        }

        private Point AnimeIceTower(int waitflame)
        {
            return this.Return(new int[4] { 5, 6, 7, 13 }, new int[4]
            {
        5,
        4,
        3,
        3
            }, 0, waitflame);
        }

        private Point AnimeColdPress1(int waitflame)
        {
            return this.Return(new int[5] { 1, 2, 3, 4, 5 }, new int[5]
            {
        0,
        3,
        7,
        8,
        9
            }, 0, waitflame);
        }

        private Point AnimeColdPress2(int waitflame)
        {
            return this.Return(new int[4] { 1, 2, 3, 4 }, new int[4]
            {
        9,
        10,
        11,
        12
            }, 0, waitflame);
        }

        private Point AnimeCharge(int waitflame)
        {
            return this.Return(new int[4] { 1, 2, 3, 4 }, new int[4]
            {
        3,
        7,
        13,
        14
            }, 0, waitflame);
        }

        private enum ATTACK
        {
            iceTower,
            coldShot,
            iceCharge,
            freezeSword,
            iceChange,
            icePress,
        }
    }
}

