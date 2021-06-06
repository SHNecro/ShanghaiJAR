using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class FallMedi : EffectBase
    {
        private readonly int[,] action = new int[2, 1];
        private float y = 0.0f;
        private const int interval = 6;
        private readonly int angle;
        private int move;
        private float yplus;
        private readonly SaveData savedata;

        public FallMedi(IAudioEngine s, Vector2 pd, Point posi, int angle)
          : base(s, null, posi.X, posi.Y)
        {
            this.angle = angle;
            this.positionDirect = pd;
        }

        public override void Updata()
        {
            ++this.move;
            this.y += this.yplus;
            ++this.yplus;
            if (y <= 240.0)
                return;
            this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(0, 192, 64, 96);
            this._position = new Vector2(this.positionDirect.X + this.move * 2 + Shake.X, (float)(positionDirect.Y + (double)this.move + Shake.Y - 2.0 + 1.0 + (int)this.y - 24.0));
            this.color = Color.White;
            dg.DrawImage(dg, "charachip7", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
