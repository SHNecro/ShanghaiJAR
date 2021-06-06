using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class AfterSteal : EffectBase
    {
        private const byte _speed = 2;

        public AfterSteal(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = 2;
            this.positionDirect = pd;
        }

        public AfterSteal(IAudioEngine s, SceneBattle p, int pX, int pY)
          : base(s, p, pX, pY)
        {
            this.speed = 2;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 79);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame >= 9)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 56, 32, 56, 32);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "steal", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
