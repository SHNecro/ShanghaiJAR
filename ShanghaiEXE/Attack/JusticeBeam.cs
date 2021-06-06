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
    internal class JusticeBeam : AttackBase
    {
        private bool flash;
        public bool end;

        public JusticeBeam(IAudioEngine so, SceneBattle p, int pX, int pY, Panel.COLOR u, int po, int s)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            if (!this.flag)
                return;
            this.invincibility = true;
            this.upprint = false;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(5, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 56);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 56);
            this.frame = 0;
            this.color = Color.White;
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                if (this.frame % 2 == 0)
                    this.flash = !this.flash;
                if (!this.end)
                {
                    if (this.animationpoint.X < 2 && this.frame % 4 == 3)
                        ++this.animationpoint.X;
                    switch (this.frame)
                    {
                        case 0:
                            this.sound.PlaySE(SoundEffect.beam);
                            break;
                        case 4:
                            this.hitting = true;
                            break;
                    }
                }
                else if (this.animationpoint.X >= 0)
                {
                    if (this.frame % 4 == 3)
                        --this.animationpoint.X;
                    if (this.animationpoint.X < 0)
                        this.flag = false;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.animationpoint.X < 0)
                return;
            int x1 = this.flash ? 80 : 0;
            double x2 = positionDirect.X;
            Point shake = this.Shake;
            double x3 = shake.X;
            double num1 = x2 + x3;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(x1, 784 + 24 * this.animationpoint.X, 40, 24);
            dg.DrawImage(dg, "darkPA", this._rect, true, this._position, this.rebirth, this.color);
            for (int index = 1; index < this.hitrange.X; ++index)
            {
                double num3 = positionDirect.X + (double)(index * 40 * this.UnionRebirth);
                shake = this.Shake;
                double x4 = shake.X;
                double num4 = num3 + x4;
                double y3 = positionDirect.Y;
                shake = this.Shake;
                double y4 = shake.Y;
                double num5 = y3 + y4;
                this._position = new Vector2((float)num4, (float)num5);
                this._rect = new Rectangle(40 + x1, 784 + 24 * this.animationpoint.X, 40, 24);
                dg.DrawImage(dg, "darkPA", this._rect, true, this._position, this.rebirth, this.color);
            }
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
