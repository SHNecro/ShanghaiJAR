using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class FightShanhaiBack : EffectBase
    {
        private readonly int[,] action = new int[2, 1];
        private readonly float y = 0.0f;
        private const int interval = 6;
        private readonly int angle;
        private readonly SaveData savedata;

        public FightShanhaiBack(IAudioEngine s, Vector2 pd, Point posi, int angle)
          : base(s, null, posi.X, posi.Y)
        {
            this.angle = angle;
            this.positionDirect = pd;
        }

        public override void Updata()
        {
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 1.0));
            Rectangle _rect = new Rectangle(0, 384, 32, 48);
            dg.DrawImage(dg, "charachip1", _rect, false, this._position, false, Color.White);
            this._rect = new Rectangle(432 + this.angle * 24, 0, 24, 48);
            this._position = new Vector2(this.positionDirect.X + Shake.X, (float)(positionDirect.Y + (double)this.Shake.Y - 2.0 + 1.0) - (int)this.y);
            this.color = Color.White;
            dg.DrawImage(dg, "body3", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
