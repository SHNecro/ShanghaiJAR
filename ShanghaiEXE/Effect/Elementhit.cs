using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Elementhit : EffectBase
    {
        public Elementhit(
          IAudioEngine s,
          SceneBattle p,
          Vector2 pd,
          int sp,
          Point posi,
          ChipBase.ELEMENT e)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
            this.element = e;
        }

        public Elementhit(IAudioEngine s, SceneBattle p, int pX, int pY, int sp, ChipBase.ELEMENT e)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
            this.element = e;
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
            this._rect = new Rectangle(this.animationpoint.X * 64, 288 + (int)this.element * 64, 64, 64);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
