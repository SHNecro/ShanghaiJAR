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
    internal class Storm : AttackBase
    {
        private int roop = 0;
        public int hits = 8;
        public ChipBase.ELEMENT cEle = ChipBase.ELEMENT.normal;

        public Storm(
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
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 4, this.position.Y * 24 + 42);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 - 4, this.position.Y * 24 + 42);
            this.frame = 0;
            if (this.StandPanel.state == Panel.PANEL._sand)
            {
                this.element = ChipBase.ELEMENT.earth;
                this.power *= 2;
                this.StandPanel.state = Panel.PANEL._nomal;
            }
            this.sound.PlaySE(SoundEffect.shoot);
            this.cEle = this.element;
        }

        public Storm(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int hits,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.hits = hits;
            this.invincibility = false;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 4, this.position.Y * 24 + 42);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 - 4, this.position.Y * 24 + 42);
            this.frame = 0;
            if (this.StandPanel.state == Panel.PANEL._sand)
            {
                this.element = ChipBase.ELEMENT.earth;
                this.power *= 2;
                this.StandPanel.state = Panel.PANEL._nomal;
            }
            this.sound.PlaySE(SoundEffect.shoot);
            this.cEle = this.element;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame == 5)
                {
                    this.frame = 0;
                    ++this.roop;
                    if (this.roop >= this.hits)
                        this.flag = false;
                    else if (this.InArea)
                        this.hitflag[this.position.X, this.position.Y] = false;
                }
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
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * 32, 48 * (int)this.cEle, 32, 48);
            dg.DrawImage(dg, "tornado", this._rect, true, this._position, this.rebirth, Color.White);
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
