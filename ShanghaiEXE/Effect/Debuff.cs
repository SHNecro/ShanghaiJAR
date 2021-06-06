using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class Debuff : EffectBase
    {
        private static Color ColorMask = Color.Purple;

        public Debuff(IAudioEngine s, SceneBattle p, Vector2 pd, int sp, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
            this.animationpoint.X = 6;
        }

        public override void Updata()
        {
            this.animationpoint.X = 5 - this.frame;
            if (this.frame < 0)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.animationpoint.X >= 6) return;

            this._rect = new Rectangle(40 * this.animationpoint.X, 144, 40, 72);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Debuff.ColorMask;
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
