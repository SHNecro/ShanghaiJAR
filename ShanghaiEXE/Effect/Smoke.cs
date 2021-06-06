using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Smoke : EffectBase
    {
        private const byte _speed = 2;

        public Smoke(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi, ChipBase.ELEMENT ele)
          : base(s, p, posi.X, posi.Y)
        {
            this.downprint = true;
            this.speed = 2;
            this.positionDirect = pd;
            this.animationpoint.Y = (int)ele;
        }

        public Smoke(IAudioEngine s, SceneBattle p, int pX, int pY, ChipBase.ELEMENT ele)
          : base(s, p, pX, pY)
        {
            this.speed = 2;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 80);
            this.animationpoint.Y = (int)ele;
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame >= 8)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 32, this.animationpoint.Y * 32, 32, 32);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "smoke", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
