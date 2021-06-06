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
    internal class DelayKnife : AttackBase
    {
        private const int hit = 8;
        private readonly int movespeed;
        private int hittime;

        public DelayKnife(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int hittime,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.invincibility = true;
            this.movespeed = 8;
            this.hitting = false;
            this.hittime = hittime;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 50);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 50);
            this.frame = 0;
            this.breaking = false;
            if (this.union == Panel.COLOR.red)
                this.movespeed *= -1;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hittime <= 90)
            {
                if (this.frame % 5 == 0)
                    this.bright = !this.bright;
                if (this.bright)
                    this.PanelBright();
            }
            if (this.moveflame)
            {
                --this.hittime;
                ++this.frame;
            }
            if (this.hittime <= 0)
            {
                this.parent.effects.Add(new KnifeAfter(this.sound, this.parent, this.position.X, this.position.Y, this.union));
                this.parent.attacks.Add(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.element));
                this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag || positionDirect.Y - (double)(this.hittime * 8) < -40.0)
                return;
            this._position = new Vector2(this.positionDirect.X - this.hittime * 8 * this.UnionRebirth + Shake.X, this.positionDirect.Y - this.hittime * 8 + Shake.Y);
            this._rect = new Rectangle(160, 176, 32, 32);
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
