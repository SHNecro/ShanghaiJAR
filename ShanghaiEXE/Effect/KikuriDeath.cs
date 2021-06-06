using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class KikuriDeath : EffectBase
    {
        private Rectangle rect;
        private Rectangle rectW;
        private Rectangle rectD;
        private Vector2 posi;
        private Vector2 posib;
        private bool white;
        private readonly int alpha;

        public KikuriDeath(
          IAudioEngine s,
          SceneBattle p,
          Rectangle r,
          Rectangle rw,
          Rectangle rd,
          Vector2 v,
          string n,
          bool re,
          Point posis)
          : base(s, p, posis.X, posis.Y)
        {
            this.picturename = n;
            this.rectD = rd;
            this.rect = r;
            this.rectW = rw;
            this.posi = v;
            this.animationpoint.Y = 2;
            this.rebirth = re;
            this.white = true;
            this.sound.PlaySE(SoundEffect.clincher);
        }

        public override void Updata()
        {
            if (this.frame == 2)
                this.sound.StopBGM();
            if (this.parent.blackOut && this.frame < 2)
                return;
            this.animationpoint.X = this.frame;
            if (this.frame % 40 == 0)
            {
                this.posib = this.posi;
                this.posib.X += this.Random.Next(-8, 8);
                this.posib.Y += this.Random.Next(-8, 8);
                this.parent.effects.Add(new Flash(this.sound, this.parent, this.posib, this.position));
            }
            if (this.frame > 240 && this.frame < 800)
            {
                if (this.frame % 10 == 0)
                {
                    this.posib = this.posi;
                    this.posib.X += this.Random.Next(-24, 48);
                    this.posib.Y += this.Random.Next(-84, 84);
                    this.parent.effects.Add(new Bomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, this.posib, 3, this.position));
                    this.sound.PlaySE(SoundEffect.bomb);
                }
                if (this.frame % 45 == 0)
                    this.sound.PlaySE(SoundEffect.clincher);
                if (this.parent.fadealpha < byte.MaxValue)
                    ++this.parent.fadealpha;
            }
            if (this.frame % 8 == 0)
                this.white = !this.white;
            if (this.frame == 800)
            {
                this.posib = this.posi;
                this.parent.effects.Add(new Bomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, this.posib, 3, this.position));
                this.sound.PlaySE(SoundEffect.bombbig);
                if (this.parent.manyenemys <= 0)
                    this.parent.stopEnd = false;
            }
            this.FlameControl();
            ++this.frame;
        }

        public override void Render(IRenderer dg)
        {
            if (this.frame < 2)
            {
                double x1 = posi.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double y1 = posi.Y;
                shake = this.Shake;
                double y2 = shake.Y;
                double num2 = y1 + y2;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = this.rectD;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.White);
            }
            else
            {
                double x1 = posi.X;
                Point shake = this.Shake;
                double x2 = shake.X;
                double num1 = x1 + x2;
                double y1 = posi.Y;
                shake = this.Shake;
                double y2 = shake.Y;
                double num2 = y1 + y2;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = this.white ? this.rect : this.rectW;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.White);
            }
        }

        public override void RenderUP(IRenderer dg)
        {
        }
    }
}
