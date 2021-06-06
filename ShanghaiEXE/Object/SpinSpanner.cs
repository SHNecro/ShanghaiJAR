using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSObject
{
    internal class SpinSpanner : ObjectBase
    {
        private bool breaked;
        private int count;
        private bool move;
        private int angle;

        public SpinSpanner(IAudioEngine s, SceneBattle p, int pX, int pY, int power, Panel.COLOR union)
          : base(s, p, pX, pY, union)
        {
            this.height = 40;
            this.wide = 40;
            this.hp = 30;
            this.hitPower = power;
            this.hpmax = this.hp;
            this.unionhit = false;
            this.overslip = false;
            this.effecting = true;
            this.noslip = true;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 76);
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 76);
        }

        public override void Updata()
        {
            if (this.move)
            {
                if (this.angle < 5)
                {
                    if (this.SlideMove(4f, this.angle))
                    {
                        this.SlideMoveEnd();
                        this.PositionDirectSet();
                        this.move = false;
                        this.count = 0;
                    }
                }
                else
                {
                    this.PositionDirectSet();
                    this.move = false;
                    this.count = 0;
                }
            }
            if (this.moveflame)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X >= 4)
                {
                    this.animationpoint.X = 0;
                    ++this.count;
                    if (this.count > 10 && !this.move)
                    {
                        this.count = 0;
                        this.move = true;
                        Point point = this.RandomTarget();
                        this.angle = point.X <= this.position.X ? (point.X >= this.position.X ? (point.Y >= this.position.Y ? (!this.InAreaCheck(new Point(this.position.X, this.position.Y + 1)) ? 5 : 3) : (!this.InAreaCheck(new Point(this.position.X, this.position.Y - 1)) ? 5 : 2)) : (!this.InAreaCheck(new Point(this.position.X - 1, this.position.Y)) ? 5 : (this.union == Panel.COLOR.blue ? 0 : 1))) : (!this.InAreaCheck(new Point(this.position.X + 1, this.position.Y)) ? 5 : (this.union != Panel.COLOR.blue ? 0 : 1));
                    }
                }
            }
            this.FlameControl(2);
            this.AttackMake(this.hitPower, 0, 0);
            base.Updata();
        }

        public override void Break()
        {
            if (!this.breaked)
            {
                this.breaked = true;
                this.sound.PlaySE(SoundEffect.clincher);
                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
            }
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (this.whitetime <= 0)
                this._rect = new Rectangle(this.animationpoint.X * 40, 224, 40, 40);
            else
                this._rect = new Rectangle(this.animationpoint.X * 40, 264, 40, 40);
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            dg.DrawImage(dg, "objects1", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
