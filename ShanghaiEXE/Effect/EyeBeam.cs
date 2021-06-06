using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class EyeBeam : EffectBase
    {
        private readonly bool spColor;

        public EyeBeam(IAudioEngine s, SceneBattle p, int pX, int pY, int sp, bool spColor)
          : base(s, p, pX, pY)
        {
            this.spColor = spColor;
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 - 64, pY * 24 + 64);
        }

        public override void Updata()
        {
            if (this.animationpoint.X < 4)
                this.animationpoint.X = this.frame;
            if (this.frame >= 16)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 192, 480, 192, 72);
            if (this.spColor)
                this._rect.Y += 72;
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "kikuriAttack", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
