using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class MimaWaveDream : EffectBase
    {
        public MimaWaveDream(IAudioEngine s, SceneBattle p, Vector2 pd, int sp, Point posi)
          : base(s, p, posi.X, posi.Y)
        {
            this.speed = sp;
            this.positionDirect = pd;
        }

        public MimaWaveDream(IAudioEngine s, SceneBattle p, int pX, int pY, int sp)
          : base(s, p, pX, pY)
        {
            this.speed = sp;
            this.positionDirect = new Vector2(pX * 40 + 36, pY * 24 + 80);
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
            this._rect = new Rectangle(80 * this.animationpoint.X, 192, 80, 64);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this.color = Color.White;
            dg.DrawImage(dg, "mimaAttack", this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
