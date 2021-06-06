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
    internal class MonkeyPoleChip : AttackBase
    {
        private bool print = true;
        private readonly int time;
        private int Xplus;
        private readonly int color;

        public MonkeyPoleChip(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele,
          int color)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.color = color;
            this.upprint = true;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 72, this.position.Y * 24 + 78);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 - 72, this.position.Y * 24 + 78);
            this.frame = 0;
            this.time = 1;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.bright && this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                if (this.Xplus < 32)
                    this.Xplus += 4;
                else
                    this.hitting = false;
                if (this.frame > 32 && this.frame % 3 == 0)
                    this.print = !this.print;
                if (this.frame > 64)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (!this.print)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2 - this.Xplus * this.UnionRebirth;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(280, 64 + 64 * this.color, 32, 8);
            dg.DrawImage(dg, "woojow", this._rect, true, this._position, !this.rebirth, Color.White);
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
            p.Knockbuck(false, false, this.union);
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.frame = 6 - this.frame;
            this.hitting = false;
            e.Knockbuck(false, false, this.union);
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.frame = 6 - this.frame;
            this.hitting = false;
            o.Knockbuck(false, false, this.union);
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
