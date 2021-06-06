using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class DelayScissor : AttackBase
    {
        private const int hit = 8;
        private int hittime;
        private readonly int hp;
        private Vector2 startPosi;
        private Vector2 movespeed;

        public DelayScissor(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int hittime,
          ChipBase.ELEMENT ele,
          Vector2 startPosi,
          bool sp,
          int hp = 10)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.hp = hp;
            this.invincibility = true;
            this.hitting = false;
            this.hittime = hittime;
            this.animationpoint.X = 0;
            this.animationpoint.Y = sp ? 2 : 0;
            this.hitrange = new Point(0, 0);
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = startPosi;
            Vector2 vector2 = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 50);
            this.movespeed.X = (vector2.X - startPosi.X) / hittime;
            this.movespeed.Y = (vector2.Y - startPosi.Y) / hittime;
            this.frame = 0;
            this.breaking = false;
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
            this.positionDirect.X += this.movespeed.X;
            this.positionDirect.Y += this.movespeed.Y;
            --this.hittime;
            ++this.frame;
            if (this.moveflame)
                ++this.animationpoint.X;
            if (this.hittime <= 0)
            {
                if (!this.StandPanel.Hole)
                    this.parent.objects.Add(new Scissor(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.hp, this.animationpoint.Y == 2));
                this.hitting = false;
                this.flag = false;
            }
            else if (this.hittime == 1)
            {
                this.sound.PlaySE(SoundEffect.breakObject);
                this.ShakeStart(5, 5);
                this.hitting = true;
            }
            this.FlameControl(2);
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(160 + 160 * (this.animationpoint.X % 4), 432 + 144 * this.animationpoint.Y, 160, 144);
            dg.DrawImage(dg, "ScissorMan", this._rect, false, this._position, this.rebirth, Color.White);
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
