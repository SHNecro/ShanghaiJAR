using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSEffect
{
    internal class BadWater : EffectBase
    {
        public BadWater(MyAudio s, SceneBattle p, int pX, int pY, int sp)
          : base(s, p, pX, pY)
        {
            this.speed = sp * 2;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 74);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame + 1;
            if (this.frame >= 4)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 40, 360, 40, 32);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "kikuriAttack", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
