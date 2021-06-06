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
    internal class PushTornado : AttackBase
    {
        private const int hit = 3;
        private readonly int movespeed;
        private readonly int hits;
        private bool powerup;
        private int realflame;

        public PushTornado(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele,
          int s,
          int hits,
          bool wait)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.hits = hits;
            this.invincibility = false;
            this.movespeed = s;
            this.speed = 1;
            this.realflame = wait ? 0 : 30;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 64);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 64);
            this.frame = 0;
            if (this.union == Panel.COLOR.blue)
                this.movespeed *= -1;
            this.position.X = (int)(positionDirect.X / 40.0);
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.InArea && (this.StandPanel.state == Panel.PANEL._sand && !this.powerup))
            {
                this.element = ChipBase.ELEMENT.earth;
                this.power *= 2;
                this.powerup = true;
            }
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame == 5)
                    this.frame = 0;
            }
            ++this.realflame;
            if (this.realflame >= 30)
                this.positionDirect.X += movespeed;
            this.position.X = this.Calcposition(this.positionDirect, 20, true).X;
            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;
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
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * 32, 48 * (int)this.Element, 32, 48);
            dg.DrawImage(dg, "tornado", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, charaposition.X, charaposition.Y, 1, this.element));
            if (this.hits > 1 && this.flag)
            {
                Storm storm = new Storm(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, this.element)
                {
                    hits = this.hits
                };
                storm.hitflag[this.position.X, this.position.Y] = true;
                this.parent.attacks.Add(storm);
            }
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, charaposition.X, charaposition.Y, 1, this.element));
            if (this.hits > 1 && this.flag)
            {
                Storm storm = new Storm(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.powerup ? this.power / 2 : this.power, this.element)
                {
                    hits = this.hits
                };
                storm.hitflag[this.position.X, this.position.Y] = true;
                this.parent.attacks.Add(storm);
            }
            this.flag = false;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            p.Knockbuck(true, false, this.union);
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            e.Knockbuck(true, false, this.union);
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            o.Knockbuck(true, o.overslip, this.union);
            return base.HitEvent(o);
        }
    }
}
