using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class BugHoleDead : EffectBase
    {
        private const byte _speed = 3;

        public BugHoleDead(IAudioEngine s, SceneBattle p, Vector2 pd, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = 3;
            this.positionDirect = pd;
        }

        public BugHoleDead(IAudioEngine s, SceneBattle p, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = 3;
            this.positionDirect = new Vector2(posi.X * 40 + 20, posi.Y * 24 + 64);
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                ++this.animationpoint.X;
                if (this.frame > 4)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 32, 1424, 32, 40);
            this._position = this.positionDirect;
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
