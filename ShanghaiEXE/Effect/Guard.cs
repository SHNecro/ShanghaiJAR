using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Guard : EffectBase
    {
        public Guard(IAudioEngine s, SceneBattle p, Vector2 pd, int sp, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
        }

        public Guard(IAudioEngine s, SceneBattle p, int pX, int pY, int sp)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
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
            this._rect = new Rectangle(this.animationpoint.X * 48, 232, 48, 48);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
