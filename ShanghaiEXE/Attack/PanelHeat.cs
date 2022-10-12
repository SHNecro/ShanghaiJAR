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
    internal class PanelHeat : AttackBase
    {
        private readonly int time;

        public PanelHeat(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          int time)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.heat)
        {
            if (!this.flag)
                return;
            this.knock = true;
            this.invincibility = true;
            this.canCounter = false;
            this.time = time;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 78);
            this.frame = 0;
            this.power = 50;
            if (this.StandPanel.Hole)
                this.flag = false;
        }

        public override void Updata()
        {
            this.union = this.union == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue;
            if (this.over)
                return;
            if (this.StandPanel.Hole)
                this.flag = false;
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame / 3 % 5;
                if (this.frame >= this.time)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.frame > this.time - 60 && (uint)(this.frame % 3) <= 0U)
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
            this._rect = new Rectangle(40 * this.animationpoint.X, 320, 40, 32);
            dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
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
            if (p.Element == ChipBase.ELEMENT.heat || !base.HitEvent(p))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (e.Element == ChipBase.ELEMENT.heat || !base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (o.Element == ChipBase.ELEMENT.heat || !base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
