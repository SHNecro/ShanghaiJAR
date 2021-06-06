using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class ImpactBomb : EffectBase
    {
        private const byte _speed = 3;

        public ImpactBomb(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = 3;
            this.positionDirect = pd;
        }

        public ImpactBomb(IAudioEngine s, SceneBattle p, int pX, int pY)
          : base(s, p, pX, pY)
        {
            this.speed = 3;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame > 6)
                this.flag = false;
            this.FlameControl(4);
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 40, 920, 40, 40);
            this._position = this.positionDirect;
            dg.DrawImage(dg, "darkPA", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
