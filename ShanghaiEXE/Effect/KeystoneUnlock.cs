using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class KeystoneUnlock : EffectBase
    {
        private const int FrameCount = 20;
        private static readonly Rectangle SpriteRect = new Rectangle(840, 400, 140, 200);

        public KeystoneUnlock(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 0;
        }

        public override void Updata()
        {
            this.FlameControl(4);
            if (!this.moveflame)
                return;
            if (this.animationpoint.X > frame)
            {
                this.flag = false;
                return;
            }
            this.animationpoint.X++;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(SpriteRect.Left + this.animationpoint.X * SpriteRect.Width, SpriteRect.Top, SpriteRect.Width, SpriteRect.Height);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - SpriteRect.Height / 2));
            this.color = Color.White;
            dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
