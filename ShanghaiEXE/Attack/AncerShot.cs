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
    internal class AncerShot : AttackBase
    {
        private readonly bool penetration;
        private readonly EnemyBase master;
        public int scene;

        public AncerShot(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele,
          EnemyBase master)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.master = master;
            this.penetration = true;
            this.upprint = false;
            this.speed = s;
            this.breaking = true;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.red;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 10, this.position.Y * 24 + 56);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 10, this.position.Y * 24 + 56);
            this.frame = 0;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            this.PanelBright();
            switch (this.scene)
            {
                case 0:
                    this.positionDirect.X += this.union == Panel.COLOR.red ? speed : -this.speed;
                    if (positionDirect.X < -160.0 || positionDirect.X > 400.0)
                    {
                        this.scene = 1;
                        goto case 1;
                    }
                    else
                        goto case 1;
                case 1:
                    this.position = this.Calcposition(this.positionDirect, 32, false);
                    if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                        this.hitting = false;
                    else
                        this.hitting = true;
                    break;
                default:
                    this.positionDirect.X += this.union == Panel.COLOR.red ? -this.speed : speed;
                    goto case 1;
            }
        }

        public override void Render(IRenderer dg)
        {
            if (!this.flag)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2 - 40.0;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2 - 36.0;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(1032, this.master.version < 4 ? 0 : 160, 40, 80);
            this.color = Color.White;
            dg.DrawImage(dg, "mrasa", this._rect, true, this._position, this.rebirth, this.color);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            if (!this.penetration)
                this.flag = false;
            this.parent.effects.Add(new Basterhit(this.sound, this.parent, charaposition.X, charaposition.Y, 2));
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            if (!this.penetration)
                this.flag = false;
            this.parent.effects.Add(new Basterhit(this.sound, this.parent, charaposition.X, charaposition.Y, 2));
            return true;
        }

        public override bool HitEvent(Player p)
        {
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            if (!this.penetration)
                this.flag = false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            if (!this.penetration)
                this.flag = false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
