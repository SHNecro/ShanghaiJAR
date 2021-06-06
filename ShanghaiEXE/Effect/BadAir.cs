using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class BadAir : EffectBase
    {
        private int a = 0;
        private bool up = true;
        private readonly int colorNum;

        public BadAir(IAudioEngine s, Vector2 pd, Point posi, int colorNum)
          : base(s, null, posi.X, posi.Y)
        {
            this.colorNum = colorNum;
        }

        public override void Updata()
        {
            if (this.up)
            {
                if (this.a >= 128)
                    this.up = false;
                else
                    this.a += 2;
            }
            else if (this.a <= 55)
                this.up = true;
            else
                this.a -= 2;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            switch (this.colorNum)
            {
                case 0:
                    this.color = Color.FromArgb(this.a, 110, 48, 121);
                    break;
                case 1:
                    this.color = Color.FromArgb(this.a, byte.MaxValue, 30, 0);
                    break;
                case 2:
                    this.color = Color.FromArgb(this.a, 128, byte.MaxValue, byte.MaxValue);
                    break;
            }
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, false, this.color);
        }
    }
}
