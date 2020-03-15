using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using System.Drawing;

namespace NSEvent
{
    internal class Fade : EventBase
    {
        private readonly byte A;
        private byte R;
        private byte G;
        private byte B;
        private readonly int fadeingTime;

        public Fade(
          IAudioEngine s,
          EventManager m,
          int ms,
          byte a,
          byte r,
          byte g,
          byte b,
          bool wait,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = !wait;
            this.A = a;
            this.R = r;
            this.G = g;
            this.B = b;
            this.fadeingTime = ms;
        }

        private void Init()
        {
            this.frame = 0;
        }

        public override void Update()
        {
            if (!this.NoTimeNext && this.frame < this.fadeingTime)
            {
                if (this.frame == 0)
                {
                    if (this.A == 0)
                    {
                        this.R = this.manager.parent.fadeColor.R;
                        this.G = this.manager.parent.fadeColor.G;
                        this.B = this.manager.parent.fadeColor.B;
                    }
                    this.manager.parent.FadeStart(Color.FromArgb(A, R, G, B), this.fadeingTime);
                }
                ++this.frame;
            }
            else
            {
                this.Init();
                if (this.A == 0)
                {
                    this.R = this.manager.parent.fadeColor.R;
                    this.G = this.manager.parent.fadeColor.G;
                    this.B = this.manager.parent.fadeColor.B;
                }
                this.manager.parent.FadeStart(Color.FromArgb(A, R, G, B), this.fadeingTime);
                this.EndCommand();
            }
        }

        public override void SkipUpdate()
        {
            if (this.A == 0)
            {
                this.R = this.manager.parent.fadeColor.R;
                this.G = this.manager.parent.fadeColor.G;
                this.B = this.manager.parent.fadeColor.B;
            }
            this.manager.parent.FadeStart(Color.FromArgb(A, R, G, B), 0);
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
