using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class MimaWaveLong : EffectBase
    {
        public MimaWaveLong(IAudioEngine s, SceneBattle p, Vector2 pd, int sp, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
        }

        public MimaWaveLong(IAudioEngine s, SceneBattle p, int pX, int pY, int sp)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40, pY * 24 + 76);
        }

        public override void Updata()
        {
            this.animationpoint.X = this.frame;
            if (this.frame >= 4)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(72 * this.animationpoint.X, 152, 72, 40);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "mimaAttack", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
