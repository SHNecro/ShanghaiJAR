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
    internal class MonkeyPole : AttackBase
    {
        private readonly int colol;

        public MonkeyPole(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          int c,
          bool pl,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.colol = c;
            this.upprint = false;
            this.speed = s;
            this.element = ChipBase.ELEMENT.leaf;
            this.animationpoint.X = 0;
            this.invincibility = false;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (pl)
            {
                if (this.union == Panel.COLOR.red)
                    this.positionDirect = new Vector2(this.position.X * 40 + 7, this.position.Y * 24 + 53);
                else
                    this.positionDirect = new Vector2((this.position.X + 1) * 40 - 5, this.position.Y * 24 + 53);
            }
            else if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 4, this.position.Y * 24 + 68);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 4, this.position.Y * 24 + 68);
            this.frame = 0;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.moveflame)
            {
                if (this.frame == 4)
                    this.hitting = false;
                if (this.frame < 3)
                {
                    ++this.hitrange.X;
                    if (this.animationpoint.X < 3)
                        ++this.animationpoint.X;
                }
                if (this.frame > 6)
                {
                    if (this.animationpoint.X >= 0)
                        --this.animationpoint.X;
                    if (this.animationpoint.X < 0)
                        this.flag = false;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(280, 64 + this.animationpoint.X * 16 + this.colol * 64, 120, 16);
            dg.DrawImage(dg, "woojow", this._rect, true, this._position, this.rebirth, Color.White);
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
            this.frame = 6 - this.frame;
            this.hitting = false;
            p.Knockbuck(true, false, this.union);
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.frame = 6 - this.frame;
            this.hitting = false;
            e.Knockbuck(true, false, this.union);
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.frame = 6 - this.frame;
            this.hitting = false;
            o.Knockbuck(true, false, this.union);
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
