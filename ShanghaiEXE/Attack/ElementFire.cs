using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class ElementFire : AttackBase
    {
        public int count = 0;
        private readonly int roop = 3;
        private readonly int type;
        private readonly bool twohit;
        public ChipBase.ELEMENT cEle;

        public ElementFire(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele,
          bool twohit,
          int type = 0)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.type = type;
            this.twohit = twohit;
            this.speed = 3;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.invincibility = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 8, this.position.Y * 24 + 42);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 8, this.position.Y * 24 + 42);
            this.frame = 0;
            this.color = Color.White;
            this.cEle = this.element;
        }

        public ElementFire(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int roop,
          ChipBase.ELEMENT ele,
          bool twohit,
          int type = 0)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.type = type;
            this.twohit = twohit;
            this.roop = roop;
            this.speed = 3;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.invincibility = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.blue)
                this.positionDirect = new Vector2(this.position.X * 40 - 8, this.position.Y * 24 + 42);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 8, this.position.Y * 24 + 42);
            this.frame = 0;
            this.color = Color.White;
            this.cEle = this.element;
        }

        public override void PositionDirectSet()
        {
            if (!this.rebirth)
                this.positionDirect = new Vector2(this.position.X * 40 - 8, this.position.Y * 24 + 42);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 8, this.position.Y * 24 + 42);
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.type == 0)
                {
                    if (this.frame == 2)
                    {
                        if (this.count < this.roop)
                        {
                            this.frame = 0;
                            ++this.count;
                        }
                        else
                        {
                            if (this.twohit)
                            {
                                this.HitFlagReset();
                                this.invincibility = true;
                            }
                            this.flag = false;
                        }
                    }
                }
                else if (this.frame == 3)
                {
                    if (this.count < this.roop)
                    {
                        this.frame = 0;
                        ++this.count;
                    }
                    else
                    {
                        if (this.twohit)
                        {
                            this.HitFlagReset();
                            this.invincibility = true;
                        }
                        this.flag = false;
                        EffectBase effectBase = new AfterFire(this.sound, this.parent, this.positionDirect, this.position, this.cEle, this.speed, this.rebirth);
                        effectBase.color = this.color;
                        this.parent.effects.Add(effectBase);
                    }
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            Point shake;
            if (this.type == 0)
            {
                double x1 = positionDirect.X;
                shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double y1 = positionDirect.Y;
                shake = this.Shake;
                double y2 = shake.Y;
                double num2 = y1 + y2;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = new Rectangle(224 + this.animationpoint.X * 48, 32 * (int)this.cEle, 48, 32);
                dg.DrawImage(dg, "towers", this._rect, true, this._position, this.rebirth, this.color);
            }
            if (this.type == 1)
            {
                double x1 = positionDirect.X;
                shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double y1 = positionDirect.Y;
                shake = this.Shake;
                double y2 = shake.Y;
                double num2 = y1 + y2;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = new Rectangle(this.animationpoint.X * 64, 664 + 48 * (int)this.cEle, 64, 48);
                dg.DrawImage(dg, "towers", this._rect, true, this._position, !this.rebirth, this.color);
            }
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            return base.HitCheck(charaposition);
        }

        public override bool HitEvent(Player p)
        {
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            return base.HitEvent(e);
        }

        public override bool HitEvent(ObjectBase o)
        {
            return base.HitEvent(o);
        }
    }
}
