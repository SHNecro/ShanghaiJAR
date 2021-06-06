using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSEnemy
{
    internal class SpannerMan : NaviBase
    {
        private readonly int[] pattern = new int[6] { 1, 1, 1, 3, 0, 1 };
        private readonly int[] powers = new int[4] { 40, 60, 0, 20 };
        private readonly int nspeed = 2;
        private readonly int aspeed = 4;
        private readonly Point[,] target = new Point[2, 9];
        private readonly List<SpinSpanner> spanners = new List<SpinSpanner>();
        private int action;
        private bool ready;
        private int attackCount;
        private readonly int moveroop;
        private SpannerMan.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private bool clack;
        private bool spark;
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

        public SpannerMan(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.SpannerManName1");
                    this.power = 60;
                    this.hp = 1300;
                    this.moveroop = 2;
                    this.nspeed = 3;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.SpannerManName2");
                    this.power = 80;
                    this.hp = 1500;
                    this.moveroop = 2;
                    this.nspeed = 3;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.SpannerManName3");
                    this.power = 100;
                    this.hp = 1800;
                    this.moveroop = 3;
                    this.nspeed = 2;
                    this.aspeed = 3;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.SpannerManName4");
                    this.power = 200;
                    this.hp = 1800;
                    this.moveroop = 3;
                    this.nspeed = 2;
                    this.aspeed = 3;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.SpannerManName5") + (version - 3).ToString();
                    this.power = 200;
                    this.hp = 1500 + (version - 4) * 500;
                    this.moveroop = 3;
                    this.nspeed = 2;
                    this.aspeed = 3;
                    break;
            }
            this.speed = this.nspeed;
            this.picturename = "spannerman";
            this.race = EnemyBase.ENEMY.navi;
            this.wide = 112;
            this.height = 96;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.superArmor = this.version >= 4;
            this.roopmove = 0;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 1:
                    this.dropchips[0].chip = new SpannerManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new SpannerManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new SpannerManV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SpannerManV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SpannerManV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new SpannerManV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new SpannerManV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new SpannerManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SpannerManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SpannerManV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new SpannerManV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new SpannerManV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new SpannerManV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new SpannerManV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new SpannerManV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 8800;
                    break;
                default:
                    this.dropchips[0].chip = new SpannerManV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new SpannerManV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new SpannerManV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new SpannerManV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new SpannerManV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 8800;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new SpannerManX(this.sound);
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
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 20.0), (float)(position.Y * 24.0 + 46.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame && (this.waittime >= 16 / version || this.spark))
                    {
                        this.waittime = 0;
                        ++this.roopneutral;
                        this.animationpoint = this.AnimeNeutral(this.waittime);
                        if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove > this.moveroop && !this.badstatus[4])
                            {
                                this.frame = 0;
                                this.roopmove = 0;
                                ++this.attackroop;
                                this.waittime = 0;
                                this.ready = false;
                                if (this.spark)
                                {
                                    this.attack = SpannerMan.ATTACK.SparkArm;
                                    this.speed = this.nspeed;
                                }
                                else
                                    this.attack = (SpannerMan.ATTACK)this.pattern[this.action];
                                this.powerPlus = this.powers[this.pattern[this.action]];
                                ++this.action;
                                if (this.action >= this.pattern.Length)
                                    this.action = 0;
                                this.Motion = NaviBase.MOTION.attack;
                                this.counterTiming = true;
                            }
                            else
                            {
                                this.frame = 0;
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
                        case SpannerMan.ATTACK.SpannerThrow:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeThrow(this.waittime);
                                switch (this.frame)
                                {
                                    case 4:
                                        this.counterTiming = false;
                                        this.sound.PlaySE(SoundEffect.knife);
                                        SpinSpanner spinSpanner = new SpinSpanner(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.Power, this.union)
                                        {
                                            Hp = version * 10
                                        };
                                        this.spanners.Add(spinSpanner);
                                        this.parent.objects.Add(spinSpanner);
                                        break;
                                    case 14:
                                        this.motion = NaviBase.MOTION.move;
                                        this.waittime = 0;
                                        this.frame = 0;
                                        break;
                                }
                                break;
                            }
                            break;
                        case SpannerMan.ATTACK.MetalImpact:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeImpact(this.waittime);
                                switch (this.frame)
                                {
                                    case 8:
                                        this.counterTiming = false;
                                        if (!this.parent.panel[this.position.X + this.UnionRebirth(this.union), this.position.Y].Hole)
                                        {
                                            this.sound.PlaySE(SoundEffect.quake);
                                            if (!this.clack)
                                            {
                                                Point point = this.RandomPanel(this.UnionEnemy);
                                                this.parent.panel[point.X, point.Y].Crack();
                                                if (this.version >= 3)
                                                    this.clack = true;
                                            }
                                            else
                                                this.clack = false;
                                            this.ShakeStart(1, 80);
                                            int s = 1;
                                            switch (this.version)
                                            {
                                                case 1:
                                                case 2:
                                                    s = 3;
                                                    break;
                                                case 3:
                                                    s = 2;
                                                    break;
                                            }
                                            this.parent.attacks.Add(new WaveAttsck(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, s, 0, this.element));
                                            break;
                                        }
                                        break;
                                    case 20:
                                        this.motion = NaviBase.MOTION.move;
                                        this.waittime = 0;
                                        this.frame = 0;
                                        break;
                                }
                            }
                            break;
                        case SpannerMan.ATTACK.SparkArm:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                this.animationpoint = this.AnimeSpark(this.waittime);
                                switch (this.waittime)
                                {
                                    case 7:
                                        this.counterTiming = false;
                                        this.Sound.PlaySE(SoundEffect.thunder);
                                        AttackBase attackBase = new BombAttack(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 1, 1, ChipBase.ELEMENT.eleki);
                                        attackBase.badstatus[3] = true;
                                        attackBase.badstatustime[3] = 120;
                                        attackBase.invincibility = false;
                                        this.parent.attacks.Add(attackBase);
                                        break;
                                    case 9:
                                        this.Sound.PlaySE(SoundEffect.thunder);
                                        break;
                                    case 11:
                                        this.attack = (SpannerMan.ATTACK)this.pattern[this.action];
                                        this.powerPlus = this.powers[this.pattern[this.action]];
                                        ++this.action;
                                        if (this.action >= this.pattern.Length)
                                            this.action = 0;
                                        this.Motion = NaviBase.MOTION.attack;
                                        this.counterTiming = true;
                                        this.waittime = 0;
                                        this.frame = 0;
                                        break;
                                }
                                break;
                            }
                            break;
                        case SpannerMan.ATTACK.BurnerBlast:
                            if (this.moveflame)
                                ++this.waittime;
                            if (this.moveflame)
                            {
                                if (!this.ready)
                                {
                                    this.animationpoint = this.AnimeBurner1(this.waittime);
                                    if (this.frame == 1)
                                    {
                                        this.speed = this.nspeed + 2;
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), this.position.Y, this.union, new Point(), 80, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth(this.union), this.position.Y, this.union, new Point(), 80, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth(this.union), this.position.Y - 1, this.union, new Point(), 80, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 3 * this.UnionRebirth(this.union), this.position.Y + 1, this.union, new Point(), 80, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 4 * this.UnionRebirth(this.union), this.position.Y, this.union, new Point(), 80, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 4 * this.UnionRebirth(this.union), this.position.Y - 1, this.union, new Point(), 80, true));
                                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X + 4 * this.UnionRebirth(this.union), this.position.Y + 1, this.union, new Point(), 80, true));
                                    }
                                    if (this.frame >= 8)
                                    {
                                        this.ready = true;
                                        this.counterTiming = false;
                                        this.frame = 0;
                                    }
                                }
                                else
                                {
                                    this.animationpoint = this.AnimeBurner2(this.frame % 2);
                                    int num1 = 0;
                                    int roop = 9;
                                    switch (this.frame)
                                    {
                                        case 1:
                                            this.sound.PlaySE(SoundEffect.quake);
                                            AttackBase attackBase1 = new ElementFire(this.sound, this.parent, this.position.X + 2 * this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, roop, ChipBase.ELEMENT.heat, false, 1);
                                            attackBase1.positionDirect.Y += num1;
                                            this.parent.attacks.Add(attackBase1);
                                            break;
                                        case 4:
                                            this.sound.PlaySE(SoundEffect.quake);
                                            int num2 = 3;
                                            AttackBase attackBase2 = new ElementFire(this.sound, this.parent, this.position.X + num2 * this.UnionRebirth(this.union), this.position.Y - 1, this.union, this.Power, roop, ChipBase.ELEMENT.heat, false, 1);
                                            attackBase2.positionDirect.Y += num1;
                                            this.parent.attacks.Add(attackBase2);
                                            AttackBase attackBase3 = new ElementFire(this.sound, this.parent, this.position.X + num2 * this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, roop, ChipBase.ELEMENT.heat, false, 1);
                                            attackBase3.positionDirect.Y += num1;
                                            this.parent.attacks.Add(attackBase3);
                                            AttackBase attackBase4 = new ElementFire(this.sound, this.parent, this.position.X + num2 * this.UnionRebirth(this.union), this.position.Y + 1, this.union, this.Power, roop, ChipBase.ELEMENT.heat, false, 1);
                                            attackBase4.positionDirect.Y += num1;
                                            this.parent.attacks.Add(attackBase4);
                                            break;
                                        case 7:
                                            this.sound.PlaySE(SoundEffect.quake);
                                            int num3 = 4;
                                            AttackBase attackBase5 = new ElementFire(this.sound, this.parent, this.position.X + num3 * this.UnionRebirth(this.union), this.position.Y - 1, this.union, this.Power, roop, ChipBase.ELEMENT.heat, false, 1);
                                            attackBase5.positionDirect.Y += num1;
                                            this.parent.attacks.Add(attackBase5);
                                            AttackBase attackBase6 = new ElementFire(this.sound, this.parent, this.position.X + num3 * this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, roop, ChipBase.ELEMENT.heat, false, 1);
                                            attackBase6.positionDirect.Y += num1;
                                            this.parent.attacks.Add(attackBase6);
                                            AttackBase attackBase7 = new ElementFire(this.sound, this.parent, this.position.X + num3 * this.UnionRebirth(this.union), this.position.Y + 1, this.union, this.Power, roop, ChipBase.ELEMENT.heat, false, 1);
                                            attackBase7.positionDirect.Y += num1;
                                            this.parent.attacks.Add(attackBase7);
                                            break;
                                        case 26:
                                            this.ready = false;
                                            this.motion = NaviBase.MOTION.move;
                                            this.waittime = 0;
                                            this.frame = 0;
                                            this.speed = this.nspeed;
                                            break;
                                    }
                                }
                                break;
                            }
                            break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    this.animationpoint = this.AnimeMove(this.waittime);
                    if (this.moveflame)
                    {
                        switch (this.waittime)
                        {
                            case 0:
                                bool flag = false;
                                if (!this.spark)
                                {
                                    foreach (CharacterBase characterBase in this.parent.AllChara())
                                    {
                                        if (characterBase.union == this.UnionEnemy && characterBase.position.X == Eriabash.SteelX(this, this.parent))
                                        {
                                            flag = true;
                                            this.positionre = characterBase.position;
                                            this.positionre.X -= this.UnionRebirth(this.union);
                                            break;
                                        }
                                    }
                                }
                                else
                                    this.spark = false;
                                if (flag && this.Canmove(this.positionre, this.number))
                                {
                                    this.spark = true;
                                    this.roopmove = this.moveroop + 1;
                                    this.speed = 2;
                                }
                                else
                                    this.MoveRandom(false, false);
                                if (this.position == this.positionre)
                                {
                                    this.Motion = NaviBase.MOTION.neutral;
                                    this.frame = 0;
                                    this.roopneutral = 0;
                                    ++this.roopmove;
                                    break;
                                }
                                break;
                            case 4:
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                break;
                            case 7:
                                this.Motion = NaviBase.MOTION.neutral;
                                this.frame = 0;
                                this.roopneutral = 0;
                                ++this.roopmove;
                                break;
                        }
                        ++this.waittime;
                        break;
                    }
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
                            this.animationpoint = new Point(17, 0);
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
            this.spanners.RemoveAll(s => !s.flag);
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
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                foreach (ObjectBase spanner in this.spanners)
                    spanner.Break();
                this._rect = new Rectangle(this.animationpoint.X * this.wide + this.Shake.X, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height + this.Shake.Y);
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
            this.HPposition = new Vector2((int)this.positionDirect.X, (int)this.positionDirect.Y + this.height / 2);
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
        2,
        3,
        4,
        3,
        2,
        0
            }, 1, waitflame);
        }

        private Point AnimeThrow(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[6] { 0, 5, 6, 7, 8, 9 }, 0, waittime);
        }

        private Point AnimeImpact(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[8]
            {
        1,
        1,
        1,
        2,
        1,
        1,
        1,
        100
            }, new int[8] { 0, 10, 11, 12, 13, 14, 15, 16 }, 0, waittime);
        }

        private Point AnimeSpark(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        1,
        1,
        1,
        2,
        1,
        1,
        1,
        1,
        1,
        100
            }, new int[10]
            {
        0,
        17,
        18,
        19,
        20,
        21,
        22,
        23,
        24,
        25
            }, 0, waittime);
        }

        private Point AnimeBurner1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        1,
        1,
        1,
        3,
        1,
        1
            }, new int[6] { 0, 26, 27, 28, 29, 30 }, 0, waittime);
        }

        private Point AnimeBurner2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[2]
            {
        0,
        1
            }, new int[2] { 31, 30 }, 0, waittime);
        }

        private enum ATTACK
        {
            SpannerThrow,
            MetalImpact,
            SparkArm,
            BurnerBlast,
        }
    }
}

