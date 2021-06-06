using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSBattle
{
    public class CustomGauge : AllBase
    {
        public int customspeed = 2;
        private int customgauge = 0;
        private bool customflag = false;
        private const int custommax = 1024;

        public int Customspeed
        {
            get
            {
                return this.customspeed;
            }
            set
            {
                this.customspeed = value;
            }
        }

        public int Customgauge
        {
            get
            {
                return this.customgauge;
            }
            set
            {
                this.customgauge = value;
            }
        }

        public bool Customflag
        {
            get
            {
                return this.customflag;
            }
        }

        public CustomGauge(IAudioEngine s)
          : base(s)
        {
        }

        public void Update()
        {
            if (this.customgauge < 1024)
            {
                this.customgauge += this.customspeed;
                if (this.customgauge <= 1024)
                    return;
                this.customgauge = 1024;
            }
            else
            {
                if (!this.customflag)
                {
                    this.sound.PlaySE(SoundEffect.fullcustom);
                    this.customflag = true;
                }
                ++this.frame;
                if (this.frame >= 25)
                    this.frame = 0;
            }
        }

        public void Render(IRenderer dg)
        {
            this._rect = new Rectangle(208, 16, 144, 16);
            this._position = new Vector2(48f, 0.0f);
            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
            if (this.customgauge < 1024)
            {
                int num = 0;
                if (this.customspeed < 2)
                    num = 2;
                if (this.customspeed > 2)
                    num = 1;
                this._rect = new Rectangle(592, num * 8, this.customgauge / 8, 6);
                this._position = new Vector2(56f, 8f);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
            }
            else
            {
                this._rect = new Rectangle(216, 40, 128, 6);
                this._position = new Vector2(56f, 8f);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                this._rect = new Rectangle(216, 48 + 8 * (this.frame / 5), 128, 6);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
            }
        }

        public void Reset()
        {
            this.customflag = false;
            this.customgauge = 0;
        }

        public void Max()
        {
            this.customgauge = 1024;
        }
    }
}
