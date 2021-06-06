using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class G7 : EffectBase
    {
        private readonly int[,] action = new int[2, 5]
        {
      {
        0,
        1,
        2,
        3,
        4
      },
      {
        0,
        0,
        1,
        2,
        3
      }
        };
        private readonly float y = 0.0f;
        private const int interval = 3;
        private readonly SaveData savedata;

        public G7(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
        }

        public override void Updata()
        {
            this.FlameControl(3);
            if (this.animationpoint.X < 7)
            {
                this.FlameControl(3);
                if (!this.moveflame)
                    return;
                ++this.animationpoint.X;
            }
            else
                this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 80, 552, 80, 88);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 2.0 + 1.0) - (int)this.y);
            this.color = Color.White;
            dg.DrawImage(dg, "body11", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
