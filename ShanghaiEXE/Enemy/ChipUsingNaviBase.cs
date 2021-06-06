using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEnemy
{
    internal class ChipUsingNaviBase : NaviBase
    {
        protected readonly int movewaittime = 10;
        protected int moveend = 4;
        protected ChipFolder[] chips;
        protected int usechip;
        protected bool pain;
        protected int animeFlame;
        public int waittimeSub;
        private bool attack;

        public override NaviBase.MOTION Motion
        {
            get
            {
                return this.motion;
            }
            set
            {
                this.motion = value;
            }
        }

        public ChipUsingNaviBase(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          byte n,
          Panel.COLOR u,
          byte v,
          int h,
          string names,
          string picturename)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.picturename = picturename;
            this.element = ChipBase.ELEMENT.normal;
            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 120;
            this.height = 120;
            this.speed = 2;
            this.animationpoint = new Point(0, 0);
            this.hp = 9999;
            this.hpmax = this.hp;
            this.printhp = true;
            this.effecting = false;
            if (this.parent != null)
                this.roop = (byte)(parent.manyenemys - (uint)this.number);
            this.frame = this.number * 3;
            this.neutlal = true;
            this.hp = h;
            this.hpmax = this.hp;
            this.hpprint = this.hp;
            this.name = names;
            this.havezenny = 0;
        }

        public override void Init()
        {
            base.Init();

            this.rebirth = !this.rebirth;
        }

        public override void InitAfter()
        {
            if (version - 16U <= 2U)
            {
                this.parent.illegal = true;
            }

            this.InitializeChips();
            this.PositionDirectSet();
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            if (this.Motion != NaviBase.MOTION.knockback && this.Motion != NaviBase.MOTION.attack)
            {
                if (this.moveflame)
                {
                    this.MovementIdle();
                    this.positionre = this.position;
                    switch (this.Motion)
                    {
                        case NaviBase.MOTION.neutral:
                            this.chips[this.usechip].chip.chipUseEnd = true;
                            if (this.waittime >= (60 - (Math.Min(65, 5 * (this.HpMax / 100)) - 5)) / 2)
                            {
                                if (this.attack && !this.badstatus[4])
                                {
                                    this.waittimeSub = 0;
                                    this.waittime = 0;
                                    if (this.body == CharacterBase.BODY.Shadow && this.chips[this.usechip].chip.shadow)
                                        this.Step();
                                    this.OnAttack();
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.attack = false;
                                    this.chips[this.usechip].chip.chipUseEnd = false;
                                    this.roop = 0;
                                    this.moveend = this.Random.Next(5);
                                }
                                else
                                {
                                    this.waittimeSub = 0;
                                    this.waittime = 0;
                                    this.Motion = NaviBase.MOTION.move;
                                }
                                break;
                            }
                            break;
                        case NaviBase.MOTION.attack:
                            this.chips[this.usechip].chip.Action(this, this.parent);
                            switch (this.waittime)
                            {
                                case 1:
                                    this.counterTiming = true;
                                    break;
                                case 12:
                                    this.counterTiming = false;
                                    break;
                            }
                            break;
                        case NaviBase.MOTION.move:
                            this.chips[this.usechip].chip.chipUseEnd = true;
                            if (this.waittime == 3)
                            {
                                ++this.roop;
                                if (this.roop >= this.moveend)
                                {
                                    this.SetUsedChip();
                                    this.chips[this.usechip].chip.chipUseEnd = true;
                                    this.chips[this.usechip].SettingChip(this.chips[this.usechip].chip.number);
                                    this.MoveRandom(this.chips[this.usechip].chip.infight, true, this.union, this.chips[this.usechip].chip.sideOnly);
                                    this.attack = true;
                                }
                                else
                                {
                                    this.MoveRandom(false, false);
                                }
                                this.MovementEffect();
                                this.position = this.positionre;
                                this.PositionDirectSet();
                            }
                            if (this.waittime == this.movewaittime)
                            {
                                if (this.pain)
                                {
                                    this.Hp -= 20;
                                    this.sound.PlaySE(SoundEffect.damageenemy);
                                }
                                this.Motion = NaviBase.MOTION.neutral;
                            }
                            this.MovementAnimation();
                            break;
                        case NaviBase.MOTION.knockback:
                            this.chips[this.usechip].chip.chipUseEnd = true;
                            switch (this.waittime)
                            {
                                case 1:
                                    this.animationpoint = new Point(5, 2);
                                    this.counterTiming = false;
                                    this.PositionDirectSet();
                                    break;
                                case 2:
                                    this.animationpoint = new Point(5, 2);
                                    break;
                                case 15:
                                    this.animationpoint = new Point(4, 0);
                                    this.PositionDirectSet();
                                    break;
                                case 21:
                                    this.animationpoint = new Point(0, 0);
                                    this.waittime = 0;
                                    this.waittimeSub = 0;
                                    this.Motion = NaviBase.MOTION.neutral;
                                    break;
                            }
                            if (this.waittime >= 2 && this.waittime <= 6)
                            {
                                this.positionDirect.X -= this.UnionRebirth(this.union);
                                break;
                            }
                            break;
                    }
                    ++this.waittimeSub;
                    this.waittime = this.waittimeSub;
                    if (this.waittime >= 2147483646)
                        this.waittime = 0;
                }
            }
            else if (this.Motion == NaviBase.MOTION.attack)
            {
                if (this.chips[this.usechip].chip.timeStopper)
                {
                    if (this.chips[this.usechip].chip.BlackOut(this, this.parent, this.chips[this.usechip].chip.name, this.chips[this.usechip].chip.Power(this).ToString()))
                    {
                        if (!this.chips[this.usechip].chip.chipUseEnd)
                            this.chips[this.usechip].chip.Action(this, this.parent);
                        if (this.chips[this.usechip].chip.chipUseEnd)
                            this.chips[this.usechip].chip.ActionEnd(this, this.parent);
                    }
                }
                else
                {
                    this.chips[this.usechip].chip.Action(this, this.parent);
                    if (this.waittime == 0)
                        this.waittimeSub = 0;
                    if (this.chips[this.usechip].chip.chipUseEnd)
                        this.chips[this.usechip].chip.ActionEnd(this, this.parent);
                }
                switch (this.waittime)
                {
                    case 1:
                        this.counterTiming = true;
                        break;
                    case 30:
                        this.counterTiming = false;
                        break;
                }
                ++this.waittimeSub;
                this.waittime = this.waittimeSub;
            }
            else if (this.Motion == NaviBase.MOTION.knockback)
            {
                this.chips[this.usechip].chip.chipUseEnd = true;
                switch (this.waittime)
                {
                    case 2:
                        this.counterTiming = false;
                        this.animationpoint = new Point(5, 2);
                        this.PositionDirectSet();
                        break;
                    case 3:
                        this.animationpoint = new Point(5, 2);
                        break;
                    case 15:
                        this.animationpoint = new Point(4, 0);
                        this.PositionDirectSet();
                        break;
                    default:
                        if (this.waittime >= 21)
                        {
                            this.counterTiming = false;
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.Motion = NaviBase.MOTION.neutral;
                        }
                        break;
                }
                if (this.waittime >= 2 && this.waittime <= 6)
                {
                    this.positionDirect.X -= this.UnionRebirth(this.union);
                }
                ++this.waittimeSub;
                this.waittime = this.waittimeSub;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        protected virtual void InitializeChips()
        {
        }

        protected virtual void SetUsedChip()
        {
        }

        protected virtual void OnAttack()
        {
        }

        protected virtual void MovementIdle()
        {
        }

        protected virtual void MovementAnimation()
        {
            this.animationpoint = CharacterAnimation.MoveAnimation(this.waittime);
        }

        protected virtual void MovementEffect()
        {
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 44);
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((int)this.positionDirect.X + this.Shake.X, (int)this.positionDirect.Y + this.Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.animationpoint = new Point(5, 2);
                this._rect = new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height, this.wide, this.height);
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.animationpoint.Y * this.height + 840, this.wide, this.height), this._position, this.picturename);
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
                this._rect.Y += 840;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            if (!this.chips[this.usechip].chip.chipUseEnd && (this.motion == NaviBase.MOTION.attack || this.motion == NaviBase.MOTION.knockback))
                this.chips[this.usechip].chip.Render(dg, this);
            this.HPposition = new Vector2(this.positionDirect.X, this.positionDirect.Y + 48f - this.height / 2);
            this.Nameprint(dg, this.printNumber);
        }

        public override void RenderUP(IRenderer dg)
        {
            if (this.chips[this.usechip].chip.chipUseEnd)
                return;
            this.chips[this.usechip].chip.BlackOutRender(dg, this.union);
        }

        public override void BarrierRend(IRenderer dg)
        {
            this.BarrierRender(dg, new Vector2(this.positionDirect.X, this.positionDirect.Y + 24f));
        }

        public override void BarrierPowerRend(IRenderer dg)
        {
            this.BarrierPowerRender(dg, new Vector2(this.positionDirect.X, this.positionDirect.Y + 24f));
        }
    }
}
