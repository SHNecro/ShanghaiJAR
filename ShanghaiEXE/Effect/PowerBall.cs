using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class PowerBall : EffectBase
    {
        private const byte _speed = 3;
        private Vector2 plusPosi;
        private readonly int time;

        public PowerBall(IAudioEngine s, SceneBattle p, Vector2 pd, Vector2 pdEnd, Point posi, int time)
          : base(s, p, posi.X, posi.Y)
        {
            this.time = time;
            this.upprint = true;
            this.speed = 3;
            this.positionDirect = pd;
            this.plusPosi = new Vector2((pdEnd.X - pd.X) / time, (pdEnd.Y - pd.Y) / time);
        }

        public override void Updata()
        {
            ++this.animationpoint.X;
            if (this.animationpoint.X > 4)
                this.animationpoint.X = 0;
            this.positionDirect.X += this.plusPosi.X;
            this.positionDirect.Y += this.plusPosi.Y;
            if (this.frame >= this.time)
                this.flag = false;
            this.FlameControl(1);
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 32, 0, 32, 32);
            this._position = this.positionDirect;
            dg.DrawImage(dg, "steal", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
