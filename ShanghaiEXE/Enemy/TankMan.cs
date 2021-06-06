using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    internal class TankMan : NaviBase
    {
        private readonly int[] pattern = new int[5] { 2, 0, 0, 1, 0 };
        private readonly int[] powers = new int[4] { 40, 0, 20, 0 };
        private readonly int nspeed = 2;
        private readonly int aspeed = 4;
        private readonly Point[,] target = new Point[2, 9];
        private int action;
        private bool ready;
        private int attackCount;
        private readonly int moveroop;
        private TankMan.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private int attackroop;

        private int Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
                if (this.action < this.pattern.Length)
                    return;
                this.action = 0;
            }
        }

        public TankMan(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.TankManName1");
                    this.power = 40;
                    this.hp = 600;
                    this.moveroop = 2;
                    this.nspeed = 4;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.TankManName2");
                    this.power = 80;
                    this.hp = 1000;
                    this.moveroop = 2;
                    this.nspeed = 4;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.TankManName3");
                    this.power = 100;
                    this.hp = 1600;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.aspeed = 3;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.TankManName4");
                    this.power = 200;
                    this.hp = 1800;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.aspeed = 3;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.TankManName5") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 1500 + (version - 4) * 500;
                    this.moveroop = 3;
                    this.nspeed = 4;
                    this.aspeed = 3;
                    break;
            }
            this.speed = this.nspeed;
            this.picturename = "tankman";
            this.race = EnemyBase.ENEMY.navi;
            this.wide = 88;
            this.height = 64;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.superArmor = this.version >= 3;
            this.roopmove = 0;
            this.PositionDirectSet();
            if (this.version >= 3)
                this.pattern = new int[7] { 3, 0, 0, 1, 2, 0, 0 };
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new TankmanV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new TankmanV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new TankmanV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new TankmanV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new TankmanV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new TankmanV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new TankmanV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new TankmanV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new TankmanV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new TankmanV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new TankmanV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new TankmanV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new TankmanV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new TankmanV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new TankmanV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 8800;
                    break;
                default:
                    this.dropchips[0].chip = new TankmanV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new TankmanV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new TankmanV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new TankmanV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new TankmanV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new TankmanX(this.sound);
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
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 8.0), (float)(position.Y * 24.0 + 58.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame && this.waittime >= 16 / version)
                    {
                        this.waittime = 0;
                        ++this.roopneutral;
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove > this.moveroop && !this.badstatus[4])
                            {
                                this.roopmove = 0;
                                this.speed = 1;
                                ++this.attackroop;
                                this.waittime = 0;
                                this.ready = false;
                                this.attack = (TankMan.ATTACK)this.pattern[this.action];
                                this.powerPlus = this.powers[this.pattern[this.action]];
                                ++this.action;
                                if (this.action >= this.pattern.Length)
                                    this.action = 0;
                                this.Motion = NaviBase.MOTION.attack;
                                this.counterTiming = true;
                            }
                            else
                            {
                                this.waittime = 0;
                                this.roopmove = this.moveroop + 1;
                                this.Motion = NaviBase.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    switch (this.attack)
                    {
                        case TankMan.ATTACK.TankCanon:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                if (!this.ready)
                                {
                                    this.animationpoint = this.AnimeCanonReady(this.waittime);
                                    if (this.waittime >= this.aspeed * 7)
                                    {
                                        this.ready = true;
                                        this.counterTiming = false;
                                        this.waittime = 0;
                                    }
                                }
                                else
                                {
                                    this.animationpoint = this.AnimeCanon(this.waittime);
                                    if (this.waittime == this.aspeed)
                                    {
                                        this.sound.PlaySE(SoundEffect.canon);
                                        this.parent.attacks.Add(new CanonBullet(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, new Vector2(this.positionDirect.X + 32 * this.UnionRebirth(this.union), this.positionDirect.Y), this.union, this.Power, this.element, false));
                                        this.parent.effects.Add(new BulletBigShells(this.sound, this.parent, this.position, this.positionDirect.X - 16 * this.UnionRebirth(this.union), this.positionDirect.Y - 16f, 32, this.union, 40 + this.Random.Next(20), 2, 0));
                                    }
                                    else if (this.waittime == this.aspeed * 6)
                                    {
                                        ++this.attackCount;
                                        this.waittime = 0;
                                        if (this.attackCount >= Math.Min((int)this.version, 3))
                                        {
                                            this.attackCount = 0;
                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            this.speed = this.nspeed;
                                        }
                                    }
                                }
                                break;
                            }
                            break;
                        case TankMan.ATTACK.GatlingGun:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                if (!this.ready)
                                {
                                    this.animationpoint = this.AnimeGatlingReady(this.waittime);
                                    if (this.waittime >= this.aspeed * 7)
                                    {
                                        int num1 = Eriabash.SteelX(this, this.parent);
                                        int num2 = 0;
                                        int y = 0;
                                        int num3 = 0;
                                        bool flag = false;
                                        for (int index = 0; index < this.target.GetLength(1); ++index)
                                        {
                                            this.target[0, index] = new Point(num1 + num3 * this.UnionRebirth(this.union), y);
                                            this.target[1, index] = new Point(num2 - num3 * this.UnionRebirth(this.union), 2 - y);
                                            if (flag)
                                            {
                                                if (y <= 0)
                                                {
                                                    ++num3;
                                                    flag = !flag;
                                                }
                                                else
                                                    --y;
                                            }
                                            else if (y >= 2)
                                            {
                                                ++num3;
                                                flag = !flag;
                                            }
                                            else
                                                ++y;
                                        }
                                        this.sound.PlaySE(SoundEffect.machineRunning);
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target[0, 0].X, this.target[0, 0].Y, this.union, new Point(0, 0), 40, true));
                                        if (this.version >= 3)
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target[1, 0].X, this.target[1, 0].Y, this.union, new Point(0, 0), 40, true));
                                        this.ready = true;
                                        this.counterTiming = false;
                                        this.speed = 1;
                                        this.waittime = 0;
                                        break;
                                    }
                                    break;
                                }
                                if (this.attackCount >= 4 && this.attackCount < this.target.GetLength(1) + 4)
                                    this.animationpoint = this.AnimeGatling2(this.waittime);
                                else
                                    this.animationpoint = this.AnimeGatling1(this.waittime);
                                if (this.waittime == this.aspeed)
                                {
                                    if (this.attackCount >= 4 && this.attackCount < this.target.GetLength(1) + 4)
                                    {
                                        if (this.version >= 3)
                                        {
                                            this.sound.PlaySE(SoundEffect.vulcan);
                                            Point point = this.target[0, this.attackCount - 4];
                                            this.parent.effects.Add(new GunHit(this.sound, this.parent, point.X, point.Y, this.union));
                                            this.parent.attacks.Add(new BombAttack(this.sound, this.parent, point.X, point.Y, this.union, this.Power, 1, this.element));
                                            point = this.target[1, this.attackCount - 4];
                                            this.parent.effects.Add(new GunHit(this.sound, this.parent, point.X, point.Y, this.union));
                                            this.parent.attacks.Add(new BombAttack(this.sound, this.parent, point.X, point.Y, this.union, this.Power, 1, this.element));
                                        }
                                        else
                                        {
                                            this.sound.PlaySE(SoundEffect.vulcan);
                                            Point point = this.target[0, this.attackCount - 4];
                                            this.parent.effects.Add(new GunHit(this.sound, this.parent, point.X, point.Y, this.union));
                                            this.parent.attacks.Add(new BombAttack(this.sound, this.parent, point.X, point.Y, this.union, this.Power, 1, this.element));
                                        }
                                        List<EffectBase> effects = this.parent.effects;
                                        IAudioEngine sound = this.sound;
                                        SceneBattle parent = this.parent;
                                        Point position = this.position;
                                        double x = positionDirect.X;
                                        this.UnionRebirth(this.union);
                                        double num1 = x - 0.0;
                                        double num2 = positionDirect.Y - 4.0;
                                        int union = (int)this.union;
                                        int time = 40 + this.Random.Next(20);
                                        BulletShells bulletShells = new BulletShells(sound, parent, position, (float)num1, (float)num2, 32, (Panel.COLOR)union, time, 2, 0);
                                        effects.Add(bulletShells);
                                        this.parent.effects.Add(new BulletShells(this.sound, this.parent, this.position, this.positionDirect.X - 16 * this.UnionRebirth(this.union), this.positionDirect.Y - 4f, 32, this.union, 40 + this.Random.Next(20), 2, 0));
                                    }
                                }
                                else if (this.waittime == this.aspeed * 3)
                                {
                                    ++this.attackCount;
                                    this.waittime = 0;
                                    if (this.attackCount > this.target.GetLength(1) + 6)
                                    {
                                        this.attackCount = 0;
                                        this.roopneutral = 0;
                                        this.Motion = NaviBase.MOTION.neutral;
                                        this.speed = this.nspeed;
                                    }
                                }
                                break;
                            }
                            break;
                        case TankMan.ATTACK.MissilePod:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                if (!this.ready)
                                {
                                    this.animationpoint = this.AnimeMissileReady(this.waittime);
                                    if (this.waittime >= this.aspeed * 6)
                                    {
                                        this.ready = true;
                                        this.counterTiming = false;
                                        this.waittime = 0;
                                    }
                                }
                                else
                                {
                                    this.animationpoint = this.AnimeMissile(this.waittime);
                                    if (this.waittime == this.aspeed * 3)
                                    {
                                        this.sound.PlaySE(SoundEffect.shoot);
                                        Point point = this.RandomPanel(Panel.COLOR.red);
                                        this.parent.attacks.Add(new DelayMissile(this.sound, this.parent, point.X, point.Y, this.union, this.Power, 300, this.element));
                                    }
                                    else if (this.waittime == this.aspeed * 5)
                                    {
                                        ++this.attackCount;
                                        this.waittime = 0;
                                        if (this.attackCount >= version + 2)
                                        {
                                            this.attackCount = 0;
                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            this.speed = this.nspeed;
                                        }
                                    }
                                }
                                break;
                            }
                            break;
                        case TankMan.ATTACK.NapalmPod:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                if (!this.ready)
                                {
                                    this.animationpoint = this.AnimeMissileReady(this.waittime);
                                    if (this.waittime >= this.aspeed * 6)
                                    {
                                        this.ready = true;
                                        this.counterTiming = false;
                                        this.waittime = 0;
                                    }
                                }
                                else
                                {
                                    this.animationpoint = this.AnimeMissile(this.waittime);
                                    if (this.waittime == this.aspeed * 3)
                                    {
                                        this.sound.PlaySE(SoundEffect.canon);
                                        Point point = this.RandomPanel(Panel.COLOR.red);
                                        this.parent.attacks.Add(new NapalmBomb(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 1, new Vector2(this.positionDirect.X - 32 * this.UnionRebirth(this.union), this.positionDirect.Y - 16f), new Point(point.X, point.Y), 40, NapalmBomb.TYPE.single, 300));
                                    }
                                    else if (this.waittime == this.aspeed * 5)
                                    {
                                        ++this.attackCount;
                                        this.waittime = 0;
                                        if (this.attackCount >= 2)
                                        {
                                            this.attackCount = 0;
                                            this.roopneutral = 0;
                                            this.Motion = NaviBase.MOTION.neutral;
                                            this.speed = this.nspeed;
                                        }
                                    }
                                }
                                break;
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
                    this.speed = this.nspeed;
                    if (this.attackroop > 4)
                        this.attackroop = 0;
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
                case NaviBase.MOTION.knockback:
                    switch (this.waittime)
                    {
                        case 2:
                            this.rebirth = this.union == Panel.COLOR.red;
                            this.ready = false;
                            this.attackCount = 0;
                            this.speed = this.nspeed;
                            this.effecting = false;
                            this.NockMotion();
                            this.counterTiming = false;
                            this.effecting = false;
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(9, 0);
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
            if (this.effecting)
                this.AttackMake(this.Power, 0, 0);
            this.FlameControl();
            this.MoveAftar();
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(1, 0);
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            double num1 = (int)this.positionDirect.X + this.Shake.X;
            int y1 = (int)this.positionDirect.Y;
            Point shake = this.Shake;
            int y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num3 = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num3 + x1;
                int y3 = (this.version < 4 ? 0 : 2) * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y4 = shake.Y;
                int height2 = height1 + y4;
                this._rect = new Rectangle(x2, y3, wide, height2);
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
            this.HPposition = new Vector2((int)this.positionDirect.X + 20, (int)this.positionDirect.Y - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 5, 6, 7 }, new int[7]
            {
        0,
        1,
        2,
        3,
        2,
        1,
        0
            }, 1, waitflame);
        }

        private Point AnimeCanonReady(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[7] { 2, 3, 20, 21, 22, 23, 24 }, 0, waittime);
        }

        private Point AnimeCanon(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[6] { 24, 25, 26, 27, 28, 24 }, 0, waittime);
        }

        private Point AnimeGatlingReady(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[7] { 2, 3, 4, 5, 6, 7, 8 }, 0, waittime);
        }

        private Point AnimeGatling1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[3] { 8, 9, 10 }, 0, waittime);
        }

        private Point AnimeGatling2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[4] { 8, 11, 12, 8 }, 0, waittime);
        }

        private Point AnimeMissileReady(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[6] { 2, 3, 13, 14, 15, 16 }, 0, waittime);
        }

        private Point AnimeMissile(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed,
        this.aspeed
            }, new int[5] { 16, 17, 18, 19, 15 }, 0, waittime);
        }

        private enum ATTACK
        {
            TankCanon,
            GatlingGun,
            MissilePod,
            NapalmPod,
        }
    }
}

