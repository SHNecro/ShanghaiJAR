using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class MultiIN : EffectBase
    {
        private readonly int[,] action = new int[2, 7]
        {
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6
      },
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6
      }
        };
        private int angle = 0;
        private const int interval = 2;
        private const int intervalLong = 16;
        private int f;

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

        public MultiIN(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 3;
        }

        public override void Updata()
        {
            this.FlameControl(2);
            if (this.moveflame)
                --this.animationpoint.X;
            if (this.animationpoint.X < 0)
                this.flag = false;
            ++this.f;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 64, 48, 64, 160);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 48.0));
            this.color = Color.White;
            dg.DrawImage(dg, "body3", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
