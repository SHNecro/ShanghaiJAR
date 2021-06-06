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
    internal class Hadouken : AttackBase
    {
        private const int hit = 8;
        private readonly int movespeed;

        public Hadouken(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.invincibility = false;
            this.movespeed = 10;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 50);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 50);
            this.frame = 0;
            this.breaking = true;
            if (this.union == Panel.COLOR.red)
                this.movespeed *= -1;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame == 5)
                    this.frame = 0;
            }
            this.positionDirect.X += this.rebirth ? movespeed : -this.movespeed;
            this.position = this.Calcposition(this.positionDirect, 48, false);
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
            double num2 = positionDirect.Y - 16.0;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle((int)this.element * 16 - 16, 384, 16, 48);
            dg.DrawImage(dg, "bomber", this._rect, true, this._position, this.rebirth, Color.White);
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
