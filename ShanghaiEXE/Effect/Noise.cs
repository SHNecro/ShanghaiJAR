using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Noise : EffectBase
    {
        private readonly Rectangle textureRect = new Rectangle(1600, 590, 36, 30);
        private readonly int frames;

        public Noise(IAudioEngine s, Vector2 pd, Point posi, bool randomFrame = false, int frames = 5)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = randomFrame ? this.Random.Next(0, frames) : 0;
            this.frames = frames;
        }

        public override void Updata()
        {
            this.FlameControl(4);
            if (this.moveflame)
            {
                this.animationpoint.X = ((this.animationpoint.X + 1) % this.frames);
                this.waittime++;
            }
            if (this.waittime >= frames)
                this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * textureRect.Width + textureRect.X, textureRect.Y, textureRect.Width, textureRect.Height);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "body25", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
