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
    internal class MadWave : AttackBase
    {
        public MadWave(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.upprint = true;
            this.speed = s;
            this.animationpoint.X = 0;
            --this.position.Y;
            this.hitrange = new Point(1, 2);
            this.hitting = false;
            this.rebirth = this.union != Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 100, this.position.Y * 24 + 24);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 - 100, this.position.Y * 24 + 24);
            this.frame = 0;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                switch (this.frame)
                {
                    case 4:
                        this.sound.PlaySE(SoundEffect.sand);
                        this.hitting = true;
                        for (int index1 = 0; index1 <= this.hitrange.X; ++index1)
                        {
                            for (int index2 = 0; index2 <= this.hitrange.Y; ++index2)
                            {
                                if (this.InAreaCheck(new Point(this.position.X + index1 * this.UnionRebirth, this.position.Y + index2)))
                                {
                                    this.parent.panel[this.position.X + index1 * this.UnionRebirth, this.position.Y + index2].State = Panel.PANEL._sand;
                                    this.parent.effects.Add(new Elementhit(this.sound, this.parent, this.position.X + index1 * this.UnionRebirth, this.position.Y + index2, 1, ChipBase.ELEMENT.earth));
                                }
                            }
                        }
                        break;
                    case 5:
                        this.flag = false;
                        break;
                }
            }
            this.FlameControl(this.speed);
        }

        public override void Render(IRenderer dg)
        {
            if (!this.flag)
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
            this._rect = new Rectangle(this.animationpoint.X * 120, 528, 120, 120);
            int element = (int)this.element;
            this.color = Color.White;
            dg.DrawImage(dg, "towers", this._rect, true, this._position, this.rebirth, this.color);
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
