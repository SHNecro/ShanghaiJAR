using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class DruidAttack : EffectBase
    {
        private readonly int[,] action = new int[2, 3]
        {
      {
        0,
        1,
        2
      },
      {
        0,
        0,
        1
      }
        };
        private readonly float y = 0.0f;
        private const int interval = 3;
        private readonly int jumpflame;

        public DruidAttack(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 15;
        }

        public override void Updata()
        {
            this.FlameControl(3);
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
            this._rect = new Rectangle(784 + this.animationpoint.X * 56, 0, 56, 80);
            this._position = new Vector2(this.positionDirect.X + 2f + Shake.X, (float)(positionDirect.Y - 10.0 + Shake.Y - 4.0) - (int)this.y);
            this.color = Color.White;
            dg.DrawImage(dg, "body3", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
