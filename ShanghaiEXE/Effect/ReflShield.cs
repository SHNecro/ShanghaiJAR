using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class ReflShield : EffectBase
    {
        private readonly Rectangle textureRect = new Rectangle(0, 72 * 2, 32, 72);

        public ReflShield(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 0;
        }

        public override void Updata()
        {
            this.FlameControl(4);
            if (this.moveflame)
                this.animationpoint.X = this.Animate(this.frame).X;
            if (this.frame > 9)
                this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * textureRect.Width + textureRect.X, textureRect.Y, textureRect.Width, textureRect.Height);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "shield", this._rect, false, this._position, this.rebirth, this.color);
        }

        private Point Animate(int animateFrame)
        {
            var x = -1;
            switch (animateFrame)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    x = animateFrame;
                    break;
                case 4:
                case 5:
                    x = 3;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    x = animateFrame + 3;
                    break;

            }

            var y = (int)(this.textureRect.Y / this.textureRect.Height);

            return new Point(x, y);
        }
    }
}
