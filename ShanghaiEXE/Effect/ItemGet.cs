using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class ItemGet : EffectBase
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
        1,
        2
      }
        };
        private readonly float y = 0.0f;
        private const int interval = 6;
        private readonly SaveData savedata;

        public ItemGet(IAudioEngine s, Vector2 pd, Point posi, SaveData savedata)
          : base(s, null, posi.X, posi.Y)
        {
            this.savedata = savedata;
            this.positionDirect = pd;
        }

        public override void Updata()
        {
            if (this.frame < this.action.GetLength(1))
                this.FlameControl(6);
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
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 1.0));
            Rectangle _rect = new Rectangle(0, 384, 32, 48);
            dg.DrawImage(dg, "charachip1", _rect, false, this._position, false, Color.White);
            this._rect = new Rectangle(192 + this.animationpoint.X * 24 + (!this.savedata.isJackedIn ? 72 : 0), 0, 24, 48);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 2.0 + 1.0) - (int)this.y);
            this.color = Color.White;
            dg.DrawImage(dg, "body3", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
