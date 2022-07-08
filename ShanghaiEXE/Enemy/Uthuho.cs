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
using System.Drawing;

namespace NSEnemy
{
    internal class Uthuho : NaviBase
    {
        private readonly int[] pattern = new int[11]
        {
      0,
      0,
      1,
      0,
      0,
      1,
      0,
      1,
      1,
      0,
      1
        };
        private readonly int[] pattern2 = new int[11]
        {
      0,
      0,
      1,
      0,
      3,
      0,
      0,
      1,
      0,
      3,
      3
        };
        private readonly int[] powers = new int[4] { 30, 0, 50, 250 };
        private readonly int nspeed = 2;
        private readonly int aspeed = 4;
        private Shadow shadow;
        private int action;
        private readonly bool attackUP;
        private readonly int attackSpeed;
        private bool ready;
        private readonly bool end;
        private int attackCount;
        private readonly int moveroop;
        private Uthuho.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private Point target;
        private NSEffect.PushWind windmaker;
        private int attackroop;
        private int wait;
        private bool notimeMove;
        private readonly int waveX;
        private readonly int waveY;

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

        public Uthuho(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            switch (this.version)
            {
                case 0:
                    this.name = ShanghaiEXE.Translate("Enemy.UthuhoName1");
                    this.barierPower = 200;
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierTime = -1;
                    this.power = 200;
                    this.hp = 3000;
                    this.moveroop = 6;
                    this.nspeed = 2;
                    this.aspeed = 2;
                    this.attackSpeed = 8;
                    break;
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.UthuhoName2");
                    this.power = 50;
                    this.hp = 2200;
                    this.moveroop = 4;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 7;
                    break;
                case 2:
                    this.name = ShanghaiEXE.Translate("Enemy.UthuhoName3");
                    this.power = 130;
                    this.hp = 2500;
                    this.moveroop = 4;
                    this.nspeed = 3;
                    this.aspeed = 3;
                    this.attackSpeed = 8;
                    break;
                case 3:
                    this.name = ShanghaiEXE.Translate("Enemy.UthuhoName4");
                    this.power = 160;
                    this.hp = 2700;
                    this.moveroop = 7;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 9;
                    break;
                case 4:
                    this.name = ShanghaiEXE.Translate("Enemy.UthuhoName5");
                    this.power = 180;
                    this.hp = 2900;
                    this.moveroop = 8;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
                default:
                    this.name = ShanghaiEXE.Translate("Enemy.UthuhoName6") + (version - 3).ToString();
                    this.power = 250;
                    this.hp = 2900 + (version - 4) * 500;
                    this.moveroop = 8;
                    this.nspeed = 3;
                    this.aspeed = 2;
                    this.attackSpeed = 10;
                    break;
            }
            this.element = ChipBase.ELEMENT.heat;
            this.speed = this.nspeed;
            this.picturename = nameof(Uthuho);
            this.race = EnemyBase.ENEMY.navi;
            this.wide = 120;
            this.height = 144;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = true;
            this.roopmove = 0;
            this.PositionDirectSet();
            switch (this.version)
            {
                case 0:
                    this.dropchips[0].chip = new UthuhoDS(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new UthuhoDS(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new UthuhoDS(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new UthuhoDS(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new UthuhoDS(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 1:
                    this.dropchips[0].chip = new UthuhoV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new UthuhoV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new UthuhoV1(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new UthuhoV1(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new UthuhoV1(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 2:
                    this.dropchips[0].chip = new UthuhoV2(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new UthuhoV2(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new UthuhoV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new UthuhoV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new UthuhoV2(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 0;
                    break;
                case 3:
                    this.dropchips[0].chip = new UthuhoV1(this.sound);
                    this.dropchips[0].codeNo = 0;
                    this.dropchips[1].chip = new UthuhoV1(this.sound);
                    this.dropchips[1].codeNo = 0;
                    this.dropchips[2].chip = new UthuhoV2(this.sound);
                    this.dropchips[2].codeNo = 0;
                    this.dropchips[3].chip = new UthuhoV2(this.sound);
                    this.dropchips[3].codeNo = 0;
                    this.dropchips[4].chip = new UthuhoV3(this.sound);
                    this.dropchips[4].codeNo = 0;
                    this.havezenny = 8800;
                    break;
                default:
                    this.dropchips[0].chip = new UthuhoV1(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new UthuhoV2(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new UthuhoV2(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new UthuhoV1(this.sound);
                    this.dropchips[3].codeNo = 2;
                    this.dropchips[4].chip = new UthuhoV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 9000;
                    if (this.version >= 8)
                    {
                        this.dropchips[4].chip = new UthuhoV3(this.sound);
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
            this.positionDirect = new Vector2(position.X * 40f, (float)(position.Y * 24.0 + 22.0));
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
            {
                if (!this.nohit)
                    this.shadow.PositionDirectSet(this.position);
                else
                    this.shadow.positionDirect.X = -120f;
            }
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    if (this.moveflame)
                        ++this.waittime;
                    if (this.moveflame && this.waittime >= (this.notimeMove ? 4 : 16) / Math.Max(2, Math.Min((int)this.version, 4)) + this.wait)
                    {
                        this.wait = 0;
                        this.waittime = 0;
                        ++this.roopneutral;
                        this.animationpoint = new Point();
                        if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                        {
                            this.roopneutral = 0;
                            if (this.roopmove >= this.moveroop && !this.badstatus[4])
                            {
                                this.roopmove = 0;
                                this.speed = this.aspeed;
                                ++this.attackroop;
                                this.waittime = 0;
                                this.ready = false;
                                if (this.hp <= this.hpmax / 2)
                                {
                                    this.attack = (Uthuho.ATTACK)this.pattern2[this.action];
                                    this.powerPlus = this.powers[this.pattern2[this.action]];
                                }
                                else
                                {
                                    this.attack = (Uthuho.ATTACK)this.pattern[this.action];
                                    this.powerPlus = this.powers[this.pattern[this.action]];
                                }
                                ++this.action;
                                if (this.action >= this.pattern.Length)
                                    this.action = 0;
                                this.attackCount = 0;
                                this.ready = false;
                                this.Motion = NaviBase.MOTION.attack;
                                this.counterTiming = true;
                            }
                            else
                            {
                                this.waittime = 0;
                                ++this.roopmove;
                                this.Motion = NaviBase.MOTION.move;
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                    {
                        switch (this.attack)
                        {
                            case Uthuho.ATTACK.FlearShot:
                                this.animationpoint = this.AnimeFlearShot(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.counterTiming = true;
                                        break;
                                    case 5:
                                        this.parent.attacks.Add(new FlaeBall(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, new Vector2((this.position.X + this.UnionRebirth(this.union)) * 40 + 20, this.position.Y * 24 + 70), this.element, 8, true, false));
                                        break;
                                    case 7:
                                        this.counterTiming = false;
                                        break;
                                    case 10:
                                        ++this.attackCount;
                                        if (this.attackCount < 3)
                                        {
                                            this.motion = NaviBase.MOTION.move;
                                            this.waittime = 0;
                                            this.animationpoint = new Point(3, 1);
                                            break;
                                        }
                                        break;
                                    case 20:
                                        this.frame = 0;
                                        this.waittime = -15;
                                        this.attackCount = 0;
                                        this.animationpoint = new Point();
                                        this.motion = NaviBase.MOTION.move;
                                        break;
                                }
                                break;
                            case Uthuho.ATTACK.MeteorStorm:
                                if (!this.ready)
                                {
                                    this.animationpoint = this.AnimeMeteor1(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                                            this.BackMove();
                                            this.speed = this.nspeed;
                                            this.position = this.positionre;
                                            this.PositionDirectSet();
                                            this.speed = 2;
                                            this.counterTiming = true;
                                            this.sound.PlaySE(SoundEffect.futon);
                                            break;
                                        case 3:
                                            this.counterTiming = false;
                                            this.ready = true;
                                            this.windmaker = new NSEffect.PushWind(this.sound, this.parent, new Vector2(), new Point(), this.union);
                                            this.parent.effects.Add(windmaker);
                                            this.waittime = 0;
                                            break;
                                    }
                                }
                                else
                                {
                                    int waittime = this.waittime / 2 % 5;
                                    this.animationpoint = this.AnimeMeteor2(waittime);
                                    if (this.waittime % 2 == 0 && this.waittime < 60 && waittime == 2)
                                    {
                                        this.sound.PlaySE(SoundEffect.futon);
                                        this.target = this.RandomTarget();
                                        this.parent.attacks.Add(new DelayMeteor(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, 90, this.Element));
                                    }
                                    if (this.waittime == 80)
                                    {
                                        this.windmaker.flag = false;
                                        this.frame = 0;
                                        this.waittime = 0;
                                        this.attackCount = 0;
                                        this.ready = false;
                                        this.animationpoint = new Point();
                                        if (this.position.Y != 1)
                                        {
                                            this.attack = Uthuho.ATTACK.PhoenixDive;
                                            this.powerPlus = this.powers[2];
                                        }
                                        else
                                        {
                                            this.frame = 0;
                                            this.waittime = 0;
                                            this.attackCount = 0;
                                            this.attack = Uthuho.ATTACK.FlearShot;
                                            this.powerPlus = this.powers[0];
                                        }
                                    }
                                    break;
                                }
                                break;
                            case Uthuho.ATTACK.PhoenixDive:
                                if (!this.ready)
                                {
                                    this.animationpoint = this.AnimePhoenix1(this.waittime);
                                    switch (this.waittime)
                                    {
                                        case 1:
                                            this.counterTiming = true;
                                            this.sound.PlaySE(SoundEffect.futon);
                                            this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, new Point(9, 2), 30, true));
                                            break;
                                        case 15:
                                            this.superArmor = true;
                                            this.overMove = true;
                                            this.counterTiming = false;
                                            this.ready = true;
                                            this.effecting = true;
                                            this.waittime = 0;
                                            this.sound.PlaySE(SoundEffect.shoot);
                                            break;
                                    }
                                }
                                else
                                {
                                    this.animationpoint = this.AnimePhoenix2(this.waittime % 2);
                                    break;
                                }
                                break;
                            case Uthuho.ATTACK.GigantFlear:
                                this.animationpoint = this.AnimeGigantFlear(this.waittime);
                                switch (this.waittime)
                                {
                                    case 1:
                                        this.superArmor = true;
                                        this.speed = 4;
                                        this.counterTiming = true;
                                        this.sound.PlaySE(SoundEffect.dark);
                                        this.sound.PlaySE(SoundEffect.charge);
                                        break;
                                    case 7:
                                        this.counterTiming = false;
                                        break;
                                    case 10:
                                        this.parent.attacks.Add(new GigantFlear(this.sound, this.parent, this.position.X + this.UnionRebirth(this.union), this.position.Y, this.union, this.Power, 2, false));
                                        break;
                                    case 50:
                                        this.superArmor = false;
                                        this.frame = 0;
                                        this.waittime = -15;
                                        this.attackCount = 0;
                                        this.animationpoint = new Point();
                                        this.motion = NaviBase.MOTION.move;
                                        break;
                                }
                                break;
                        }
                        ++this.waittime;
                    }
                    if (this.attack == Uthuho.ATTACK.PhoenixDive && this.ready)
                    {
                        if (positionDirect.X >= 0.0 && positionDirect.X <= 240.0)
                        {
                            this.nohit = false;
                            this.AttackMake(this.Power, 0, 0, true);
                            this.AttackMake(this.Power, 0, -1, false);
                            this.AttackMake(this.Power, 0, 1, false);
                        }
                        else
                            this.nohit = true;
                        this.positionDirect.X += Math.Min(8 + version, 12) * (!this.rebirth ? -1 : 1);
                        this.position.X = this.Calcposition(this.positionDirect, this.height, false).X;
                        int num = 240;
                        if (positionDirect.X < (double)-num || positionDirect.X > (double)(240 + num))
                        {
                            ++this.attackCount;
                            if (this.attackCount >= 2)
                            {
                                this.rebirth = false;
                                this.effecting = false;
                                this.superArmor = false;
                                this.motion = NaviBase.MOTION.move;
                                this.overMove = false;
                                this.nohit = false;
                                this.animationpoint = new Point();
                                this.roopneutral = 0;
                                this.frame = 0;
                                this.waittime = 0;
                                this.attackCount = 0;
                                this.speed = this.nspeed;
                            }
                            else
                            {
                                this.rebirth = true;
                                this.sound.PlaySE(SoundEffect.shoot);
                                this.HitFlagReset();
                                this.positionre = new Point(this.position.X, this.position.Y == 0 ? 2 : 0);
                                this.position = this.positionre;
                                this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.union == Panel.COLOR.blue ? 5 : 0, this.position.Y - 1, this.union, new Point(9, 2), 30, true));
                                this.PositionDirectSet();
                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    if (this.moveflame)
                        ++this.waittime;
                    this.notimeMove = this.roopmove % 2 == 1;
                    if (this.attackCount == 0)
                    {
                        this.Motion = NaviBase.MOTION.neutral;
                    }
                    else
                    {
                        this.Motion = NaviBase.MOTION.attack;
                        if (this.attack == Uthuho.ATTACK.FlearShot)
                            this.waittime = 3;
                    }
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
                            this.superArmor = false;
                            if (this.windmaker != null)
                                this.windmaker.flag = false;
                            this.superArmor = false;
                            this.rebirth = this.union == Panel.COLOR.red;
                            this.guard = CharacterBase.GUARD.none;
                            this.ready = false;
                            this.Noslip = false;
                            this.attackCount = 0;
                            this.speed = this.nspeed;
                            this.overMove = false;
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
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.wait = 0;
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
            double num1 = (int)this.positionDirect.X + this.Shake.X + 12;
            int y1 = (int)this.positionDirect.Y;
            Point shake = this.Shake;
            int y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, 720 * (this.version < 4 ? 0 : 2) + this.animationpoint.Y * this.height, this.wide, this.height);
            if (this.version == 0)
                this._rect.Y += 2160;
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num3 = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num3 + x1;
                int y3 = 720 * (this.version < 4 ? 0 : 2) + this.animationpoint.Y * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y4 = shake.Y;
                int height2 = height1 + y4;
                this._rect = new Rectangle(x2, y3, wide, height2);
                if (this.version == 0)
                    this._rect.Y += 2160;
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height)
                {
                    Y = 720 + this.animationpoint.Y * this.height
                }, this._position, this.picturename);
            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                this._rect.Y = 720 + this.animationpoint.Y * this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            if (!this.nohit)
                this.HPposition = new Vector2((int)this.positionDirect.X + 24, (int)this.positionDirect.Y + this.height / 2);
            else
                this.HPposition = new Vector2(-180f, (int)this.positionDirect.Y + this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        public override void BarrierRend(IRenderer dg)
        {
            this.BarrierRender(dg, new Vector2(this.positionDirect.X + 28f, this.positionDirect.Y + 24f));
        }

        public override void BarrierPowerRend(IRenderer dg)
        {
            this.BarrierPowerRender(dg, new Vector2(this.positionDirect.X + 28f, this.positionDirect.Y + 24f));
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[1], new int[1], 1, waitflame);
        }

        private Point AnimeFlearShot(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[8]
            {
        1,
        1,
        4,
        1,
        1,
        1,
        1,
        100
            }, new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 }, 1, waittime);
        }

        private Point AnimeMeteor1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        1,
        1,
        100
            }, new int[3] { 0, 1, 2 }, 2, waittime);
        }

        private Point AnimeMeteor2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[5]
            {
        1,
        1,
        1,
        1,
        100
            }, new int[5] { 2, 3, 4, 5, 6 }, 2, waittime);
        }

        private Point AnimePhoenix1(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[3]
            {
        3,
        3,
        100
            }, new int[3] { 0, 1, 2 }, 3, waittime);
        }

        private Point AnimePhoenix2(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[2]
            {
        0,
        100
            }, new int[2] { 3, 4 }, 3, waittime);
        }

        private Point AnimeGigantFlear(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[16]
            {
        1,
        1,
        1,
        1,
        1,
        4,
        1,
        1,
        1,
        1,
        15,
        1,
        1,
        1,
        1,
        100
            }, new int[16]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        9,
        8,
        7,
        6,
        5
            }, 4, waittime);
        }

        private enum ATTACK
        {
            FlearShot,
            MeteorStorm,
            PhoenixDive,
            GigantFlear,
        }
    }
}

