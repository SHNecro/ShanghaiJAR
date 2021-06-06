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
    internal class ThrowKnife : AttackBase
    {
        private const int hit = 8;
        private readonly int movespeed;
        private int spin;
        private readonly int angle;
        private int spintime;
        private int stoptime;

        private int Spin
        {
            set
            {
                this.spin = value;
                if (this.spin < 8)
                    return;
                this.spin = 0;
            }
            get
            {
                return this.spin;
            }
        }

        public ThrowKnife(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int movespeed,
          int po,
          int spintime,
          int stoptime,
          int angle)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            if (!this.flag)
                return;
            this.angle = angle;
            this.spintime = spintime;
            this.stoptime = stoptime;
            this.Spin = angle * 2;
            this.invincibility = true;
            this.movespeed = movespeed;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 4, this.position.Y * 24 + 82);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 - 4, this.position.Y * 24 + 82);
            this.frame = 0;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.stoptime <= 0)
            {
                if (!this.hitting)
                    this.hitting = true;
                switch (this.angle)
                {
                    case 0:
                        this.positionDirect.Y += -this.movespeed;
                        this.position.Y = this.Calcposition(new Vector2(this.positionDirect.X, this.positionDirect.Y - 28f), 32, true).Y;
                        break;
                    case 1:
                        this.positionDirect.X += this.rebirth ? movespeed : -this.movespeed;
                        this.position.X = this.Calcposition(new Vector2(this.positionDirect.X, this.positionDirect.Y - 28f), 32, true).X;
                        break;
                    case 2:
                        this.positionDirect.Y += movespeed;
                        this.position.Y = this.Calcposition(new Vector2(this.positionDirect.X, this.positionDirect.Y - 28f), 32, true).Y;
                        break;
                    case 3:
                        this.positionDirect.X += !this.rebirth ? movespeed : -this.movespeed;
                        this.position.X = this.Calcposition(new Vector2(this.positionDirect.X, this.positionDirect.Y - 28f), 32, true).X;
                        break;
                }
                if (!this.InArea)
                    this.flag = false;
            }
            else if (this.spintime <= 0)
            {
                --this.stoptime;
                if (this.stoptime == 0)
                    this.sound.PlaySE(SoundEffect.knife);
            }
            else
            {
                --this.spintime;
                if (this.moveflame)
                    ++this.Spin;
                if (this.spintime <= 0)
                    this.Spin = this.angle * 2;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double num2 = positionDirect.Y - 44.0;
            shake = this.Shake;
            double y1 = shake.Y;
            double num3 = num2 + y1;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle(this.Spin * 32, 176, 32, 32);
            dg.DrawImage(dg, "sword", this._rect, true, this._position, this.rebirth, Color.White);
            double x3 = positionDirect.X;
            shake = this.Shake;
            double x4 = shake.X;
            double num4 = x3 + x4;
            double num5 = positionDirect.Y - 28.0;
            shake = this.Shake;
            double y2 = shake.Y;
            double num6 = num5 + y2;
            this._position = new Vector2((float)num4, (float)num6);
            this._rect = new Rectangle(384, 176, 32, 32);
            dg.DrawImage(dg, "sword", this._rect, true, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
