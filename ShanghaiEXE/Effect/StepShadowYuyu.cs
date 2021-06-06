using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class StepShadowYuyu : EffectBase
    {
        private Rectangle rect;
        private Vector2 posi;
        private Vector2 posib;
        private bool white;
        private int a, b, c;

        public StepShadowYuyu(
          IAudioEngine s,
          SceneBattle p,
          Rectangle r,
          Vector2 v,
          string n,
          bool re,
          Point posis, int a, int b, int c)
          : base(s, p, posis.X, posis.Y)
        {
            this.picturename = n;
            this.rect = r;
            this.posi = v;
            this.animationpoint.Y = 2;
            this.rebirth = re;
            this.white = true;
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override void Updata()
        {
            if (this.parent.blackOut && this.frame < 2)
                return;
            this.animationpoint.X = this.frame;
            if (this.frame % 4 == 0)
                this.white = !this.white;
            if (this.frame >= 40)
            {
                this.flag = false;
                this.posib = this.posi;
            }
            this.FlameControl();
            ++this.frame;
        }

        public override void Render(IRenderer dg)
        {
            if (this.white)
                return;
            double x1 = posi.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = posi.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = this.rect;
            dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.FromArgb(this.a, this.b, this.c));
        }
    }
}
