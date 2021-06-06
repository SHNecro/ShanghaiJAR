using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class HeavenWater : EffectBase
    {
        private readonly int a = 0;
        private readonly bool up = true;

        public HeavenWater(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.color = Color.FromArgb(128, Color.White);
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X > 5)
                    this.animationpoint.X = 0;
            }
            this.FlameControl(24);
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 64, 664, 64, 32);
            for (int index1 = 0; index1 < 50; ++index1)
            {
                for (int index2 = 0; index2 < 50; ++index2)
                {
                    double num1 = positionDirect.X + (double)(64 * index1);
                    Point shake = this.Shake;
                    double x1 = shake.X;
                    double num2 = num1 + x1;
                    double num3 = positionDirect.Y - 64.0 + 32 * index2;
                    shake = this.Shake;
                    double y1 = shake.Y;
                    double num4 = num3 + y1;
                    this._position = new Vector2((float)num2, (float)num4);
                    if (_position.X > -120.0 && _position.X < 360.0 && _position.Y > -80.0 && _position.Y < 240.0)
                        dg.DrawImage(dg, "body21", this._rect, true, this._position, false, this.color);
                    if ((uint)index1 > 0U)
                    {
                        double num5 = positionDirect.X - (double)(64 * index1);
                        shake = this.Shake;
                        double x2 = shake.X;
                        double num6 = num5 + x2;
                        double num7 = positionDirect.Y - 64.0 + 32 * index2;
                        shake = this.Shake;
                        double y2 = shake.Y;
                        double num8 = num7 + y2;
                        this._position = new Vector2((float)num6, (float)num8);
                        if (_position.X > -120.0 && _position.X < 360.0 && _position.Y > -80.0 && _position.Y < 240.0)
                            dg.DrawImage(dg, "body21", this._rect, true, this._position, false, this.color);
                    }
                }
            }
        }
    }
}
