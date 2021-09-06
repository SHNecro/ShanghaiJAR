using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class ShakeStart : EventBase
    {
        private readonly int level;
        private readonly int shakeFlame;

        public ShakeStart(IAudioEngine s, EventManager m, int l, int f, SaveData save)
          : base(s, m, save)
        {
            this.shakeFlame = f;
            this.NoTimeNext = true;
            this.level = l;
        }

        public override void Update()
        {
            if (this.shakeFlame <= 0)
                this.ShakeStart(this.level);
            else
                this.ShakeStart(this.level, this.shakeFlame);
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            this.Update();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
