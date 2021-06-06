using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class CubeOpen : EffectBase
    {
        private readonly int interval = 2;
        private int leftp;
        private bool step;
        private int f;

        public CubeOpen(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 0;
        }

        public override void Updata()
        {
            this.FlameControl(this.interval);
            if (this.moveflame)
            {
                this.step = !this.step;
                if (this.f >= 15)
                    this.leftp = 64;
                if (this.f >= 30)
                    this.flag = false;
            }
            ++this.f;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(320 + this.leftp + (this.step ? 64 : 0), 392, 64, 64);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "body2", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
