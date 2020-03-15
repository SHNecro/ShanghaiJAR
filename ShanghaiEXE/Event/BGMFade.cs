using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class BGMFade : EventBase
    {
        private readonly int endparsent;
        private readonly int fadeingTime;

        public BGMFade(IAudioEngine s, EventManager m, int endparsent, int ms, bool wait, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = !wait;
            this.endparsent = endparsent;
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
                    this.sound.BGMFadeStart(this.fadeingTime, this.endparsent);
                ++this.frame;
            }
            else
            {
                this.Init();
                this.sound.BGMFadeStart(this.fadeingTime, this.endparsent);
                this.EndCommand();
            }
        }

        public override void SkipUpdate()
        {
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
