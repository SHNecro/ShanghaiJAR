using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class KikuriFade : EffectBase
    {
        private readonly int[,] action =
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
                13,
                14,
                15
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
                13,
                14,
                15
            }
        };

        public KikuriFade(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.color = Color.White;
        }

        public override void Updata()
        {
            this.FlameControl(8);
            if (!this.moveflame)
                return;
            for (int index = 0; index < this.action.GetLength(1); ++index)
            {
                if (this.frame == this.action[0, index])
                {
                    this.animationpoint.X = this.action[1, index];
                    break;
                }
            }
        }

        public override void Render(IRenderer dg)
        {
            if (this.frame >= this.action.GetLength(1))
            {
                return;
            }
            this._rect = new Rectangle(this.animationpoint.X * 136, 342, 136, 128);
            this._position = new Vector2((float)(positionDirect.X + (double)this.Shake.X), (float)(positionDirect.Y + (double)this.Shake.Y) - 1);
            dg.DrawImage(dg, "body3", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
