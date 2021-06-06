using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class SlashYorihime : EffectBase
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
        private int moving;
        private readonly int angle;
        private readonly SaveData savedata;

        public SlashYorihime(IAudioEngine s, Vector2 pd, Point posi, int angle)
          : base(s, null, posi.X, posi.Y)
        {
            this.angle = angle;
            this.positionDirect = pd;
        }

        public override void Updata()
        {
            this.FlameControl(3);
            if (this.animationpoint.X < 3)
            {
                this.FlameControl(3);
                if (this.moveflame)
                    ++this.animationpoint.X;
            }
            if (this.moving >= 48)
                return;
            this.speed = 4;
            switch (this.angle)
            {
                case 0:
                    this.position.Y += this.speed;
                    break;
                case 1:
                    this.position.X -= this.speed;
                    break;
                case 2:
                    this.position.X += this.speed;
                    break;
                case 3:
                    this.position.Y -= this.speed;
                    break;
            }
            this.PositionDirectSet();
            this.moving += this.speed;
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 1.0));
            Rectangle _rect = new Rectangle(0, 384, 32, 48);
            dg.DrawImage(dg, "charachip1", _rect, false, this._position, false, Color.White);
            this._rect = new Rectangle(this.animationpoint.X * 64 + this.angle / 2 * 256, 208 + this.angle % 2 * 64, 64, 64);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 2.0 + 1.0) - (int)this.y);
            this.color = Color.White;
            dg.DrawImage(dg, "body3", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
