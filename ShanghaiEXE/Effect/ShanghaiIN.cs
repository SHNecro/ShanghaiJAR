using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Diagnostics;
using System.Drawing;

namespace NSEffect
{
    internal class ShanghaiIN : EffectBase
    {
        private readonly int[,] action = new int[2, 14]
        {
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13
      },
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13
      }
        };
        private int angle = 0;
        private const int interval = 2;
        private const int intervalLong = 16;
        private int f;
		private bool showShanghai;

        private int Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
                if (this.angle >= 0)
                    return;
                this.angle = 7;
            }
        }

        public ShanghaiIN(IAudioEngine s, Vector2 pd, Point posi, bool showShanghai)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
			this.showShanghai = showShanghai;
        }

        public override void Updata()
        {
            if (this.frame == 5)
                this.FlameControl(16);
            else
                this.FlameControl(2);
            if (this.moveflame)
            {
                ++this.animationpoint.X;
                if (this.frame >= 8)
                    --this.Angle;
            }
            if (this.frame >= 9 && this.Angle == 0)
            {
                this.flag = false;
                Debug.WriteLine(f);
            }
            ++this.f;
        }

        public override void Render(IRenderer dg)
        {
            if (this.frame >= 8 && this.showShanghai)
            {
                this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 2.0));
                Rectangle _rect = new Rectangle(0, 384, 32, 48);
                dg.DrawImage(dg, "charachip1", _rect, false, this._position, false, Color.White);
                this._rect = new Rectangle(0, this.angle * 48, 32, 48);
                this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 2.0));
                this.color = Color.White;
                dg.DrawImage(dg, "charachip1", this._rect, false, this._position, this.rebirth, this.color);
            }
            this._rect = new Rectangle(this.animationpoint.X * 64, 48, 64, 160);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 48.0));
            this.color = Color.White;
            dg.DrawImage(dg, "body3", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
