using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class EnemyDeath : EffectBase
    {
        private Rectangle rect;
        private Rectangle rectW;
        private Vector2 posi;
        private Vector2 posib;
        private bool white;

        public EnemyDeath(
          IAudioEngine s,
          SceneBattle p,
          Rectangle r,
          Rectangle rw,
          Vector2 v,
          string n,
          bool re,
          Point posis)
          : base(s, p, posis.X, posis.Y)
        {
            this.picturename = n;
            this.rect = r;
            this.rectW = rw;
            this.posi = v;
            this.animationpoint.Y = 2;
            this.rebirth = re;
            this.white = true;
        }

        public override void Updata()
        {
            if (this.parent.blackOut && this.frame < 2)
                return;
            this.animationpoint.X = this.frame;
            switch (this.frame)
            {
                case 2:
                    this.sound.PlaySE(SoundEffect.enemydeath);
                    break;
                case 10:
                    this.posib = this.posi;
                    this.posib.X += 8f;
                    this.posib.Y += 8f;
                    this.parent.effects.Add(new Bomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, this.posib, 3, this.position));
                    break;
                case 30:
                    this.posib = this.posi;
                    this.posib.X -= 8f;
                    this.parent.effects.Add(new Bomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, this.posib, 3, this.position));
                    break;
                case 50:
                    this.posib = this.posi;
                    this.posib.X += 8f;
                    this.posib.Y -= 8f;
                    this.parent.effects.Add(new Bomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, this.posib, 3, this.position));
                    break;
            }
            if (this.frame % 8 == 0)
                this.white = !this.white;
            if (this.frame >= 60)
            {
                this.flag = false;
                this.parent.stopEnd = false;
                this.posib = this.posi;
                this.parent.effects.Add(new Bomber(this.sound, this.parent, Bomber.BOMBERTYPE.bomber, this.posib, 3, this.position));
                this.sound.PlaySE(SoundEffect.clincher);
            }
            this.FlameControl();
            ++this.frame;
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.posi.X + Shake.X, this.posi.Y + Shake.Y);
            this._rect = this.white ? this.rect : this.rectW;
            dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, Color.White);
        }
    }
}
