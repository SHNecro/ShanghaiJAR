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
    internal class AssaultBuster : AttackBase
    {
        private readonly int movespeed;
        private readonly int type;
        private readonly float randomY;
        private float plusY;
        private readonly bool blue;

        public AssaultBuster(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int type,
          int movespeed,
          bool blue,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.blue = blue;
            this.type = type;
            this.knock = false;
            this.canCounter = false;
            this.invincibility = false;
            this.movespeed = movespeed * 3;
            this.randomY = this.Random.Next(-3, 3) * (movespeed / 2 + 1) / 10f;
            this.speed = 1;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 50);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 50);
            this.frame = 0;
            if (this.union == Panel.COLOR.red)
                movespeed *= -1;
            if (this.type >= 2)
            {
                this.animationpoint.X = this.Random.Next(6);
                this.positionDirect.Y -= 8f;
            }
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                if (this.type >= 2)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X >= 6)
                        this.animationpoint.X = 0;
                    if (this.animationpoint.Y < 3)
                        ++this.animationpoint.Y;
                }
                if (this.frame == 5)
                    this.frame = 0;
            }
            this.plusY += this.randomY;
            this.positionDirect.X += !this.rebirth ? movespeed : -this.movespeed;
            this.position.X = this.Calcposition(this.positionDirect, 24, false).X;
            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            int x1 = this.blue ? 144 : 0;
            switch (this.type)
            {
                case 0:
                    this._rect = new Rectangle(x1, 840, 24, 8);
                    break;
                case 1:
                    this._rect = new Rectangle(x1, 848, 24, 8);
                    break;
                default:
                    this._rect = new Rectangle(24 * this.animationpoint.X + x1, 856 + 24 * this.animationpoint.Y, 24, 24);
                    break;
            }
            double x2 = positionDirect.X;
            Point shake = this.Shake;
            double x3 = shake.X;
            double num1 = x2 + x3;
            double num2 = positionDirect.Y + 4.0 + plusY;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            dg.DrawImage(dg, "shot", this._rect, true, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.flag = false;
            this.parent.effects.Add(new Basterhit(this.sound, this.parent, new Vector2(this.positionDirect.X + 16 * this.UnionRebirth + this.Random.Next(8), (float)(positionDirect.Y + (double)this.plusY + 8.0)), 2, this.position));
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
            e.whitetime = 1;
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            o.whitetime = 1;
            return true;
        }
    }
}
