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
    internal class KnifeAttack : AttackBase
    {
        public KnifeAttack(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele,
          bool bre)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.breaking = bre;
            this.upprint = true;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 5, this.position.Y * 24 + 48);
            this.frame = 0;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                switch (this.frame)
                {
                    case 2:
                        this.hitting = false;
                        break;
                    case 5:
                        this.flag = false;
                        break;
                }
            }
            this.FlameControl();
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
            this._rect = new Rectangle(this.animationpoint.X * 48, 0, 48, 32);
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    this.color = Color.FromArgb(byte.MaxValue, byte.MaxValue, 55, 55);
                    break;
                case ChipBase.ELEMENT.aqua:
                    this.color = Color.FromArgb(byte.MaxValue, 50, 155, byte.MaxValue);
                    break;
                case ChipBase.ELEMENT.eleki:
                    this.color = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                    break;
                case ChipBase.ELEMENT.leaf:
                    this.color = Color.FromArgb(byte.MaxValue, 155, byte.MaxValue, 55);
                    break;
                case ChipBase.ELEMENT.poison:
                    this.color = Color.FromArgb(byte.MaxValue, 100, 50, byte.MaxValue);
                    break;
                case ChipBase.ELEMENT.earth:
                    this.color = Color.FromArgb(byte.MaxValue, 160, 100, 50);
                    break;
                default:
                    this.color = Color.White;
                    break;
            }
            dg.DrawImage(dg, "sword", this._rect, true, this._position, this.rebirth, this.color);
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
