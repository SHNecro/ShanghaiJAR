using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSEffect
{
    internal class SageFloat : EffectBase
    {
        private const int FloatHeight = 4;
        private const int InitialWait = 1;
        private const int YAdjust = 20;

        private bool reverse;
        private bool facingSouth;

        private int remainingWait;
        private int yOffset;

        public SageFloat(IAudioEngine s, Vector2 pd, Point posi, bool reverse, bool facingSouth)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 0;
            this.rebirth = true;
            this.remainingWait = InitialWait;

            this.reverse = reverse;
            this.facingSouth = facingSouth;
            if (this.reverse)
            {
                this.yOffset = FloatHeight;
            }
        }

        public override void Updata()
        {
            this.FlameControl(24);
            if (this.moveflame)
            {
                if (this.remainingWait > 0)
                {
                    this.remainingWait--;
                    return;
                }

                this.yOffset += this.reverse ? -1 : 1;
            }
            if (this.yOffset < 0 || this.yOffset > FloatHeight)
            {
                this.yOffset = Math.Max(0, Math.Min(FloatHeight, this.yOffset));
                this.flag = false;
            }
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 2.0));
            var shadowrect = new Rectangle(0, 384, 32, 48);
            dg.DrawImage(dg, "charachip1", shadowrect, false, this._position, false, Color.White);
            this._rect = new Rectangle((64 * 7) + this.animationpoint.X * 64, this.facingSouth ? 0 : 96, 64, 96);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - YAdjust - this.yOffset));
            this.color = Color.White;
            dg.DrawImage(dg, "charachip19", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
