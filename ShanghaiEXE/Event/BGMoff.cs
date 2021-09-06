using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class BGMoff : EventBase
    {
        private readonly int fadetime;

        public BGMoff(IAudioEngine s, EventManager m, int fade, SaveData save)
          : base(s, m, save)
        {
            this.fadetime = fade;
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.sound.StopBGM();
            this.sound.BGMVolumeSet(100);
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
