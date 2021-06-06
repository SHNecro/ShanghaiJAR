using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class ROMHead : EffectBase
    {
        private readonly int[,] action = new int[2, 7]
        {
      {
        0,
        3,
        5,
        6,
        7,
        8,
        9
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
        private readonly float y = 0.0f;
        private const int interval = 6;
        private readonly int jumpflame;

        public ROMHead(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 15;
        }

        public override void Updata()
        {
            this.FlameControl(6);
            if (!this.moveflame)
                return;
            if (this.frame == 5)
                this.sound.PlaySE(SoundEffect.futon);
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
            this._position = new Vector2((float)(positionDirect.X + (double)this.Shake.X - 2.0), this.positionDirect.Y + Shake.Y);
            Rectangle _rect = new Rectangle(0, 384, 32, 48);
            dg.DrawImage(dg, "charachip1", _rect, false, this._position, false, Color.White);
            this._rect = new Rectangle(1128 + this.animationpoint.X * 40, 0, 40, 48);
            this._position = new Vector2((float)(positionDirect.X + (double)this.Shake.X - 4.0), (float)(positionDirect.Y + (double)this.Shake.Y - 1.0) - (int)this.y);
            this.color = Color.White;
            dg.DrawImage(dg, "body3", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
