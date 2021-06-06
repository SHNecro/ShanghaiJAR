using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class FireBall : EffectBase
    {
        private readonly int interval = 2;
        private readonly int leftp;
        private readonly bool step;
        private int f;
        private const int fallspeed = 6;
        private readonly Bomber bomber;
        private bool mute;

        public FireBall(IAudioEngine s, Vector2 pd, Point posi, bool mute = false)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;
            this.animationpoint.X = 0;
            this.f = 30;
            this.bomber = new Bomber(s, null, Bomber.BOMBERTYPE.flashbomber, new Vector2(pd.X + 24f, pd.Y - 24f), 3, new Point(0, 0));
            this.mute = mute;
        }

        public override void Updata()
        {
            if (this.f > 0)
            {
                this.FlameControl(this.interval);
                if (!this.moveflame) { }
                --this.f;
                if (this.f > 0)
                    return;
                if (!this.mute)
                {
                    this.sound.PlaySE(SoundEffect.bombmiddle);
                }
                this.ShakeStart(2, 4);
                this.bomber.positionDirect = this.positionDirect;
            }
            else
            {
                this.bomber.Updata();
                if (!this.bomber.flag)
                    this.flag = false;
            }
        }

        public override void Render(IRenderer dg)
        {
            if (this.f > 0)
            {
                this._rect = new Rectangle(0, 992, 24, 24);
                this._position = new Vector2(this.positionDirect.X - this.f * 6 + Shake.X, this.positionDirect.Y - this.f * 6 + Shake.Y);
                this.color = Color.White;
                dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, this.color);
            }
            else
                this.bomber.Render(dg);
        }
    }
}
