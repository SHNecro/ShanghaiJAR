using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class StopSkip : EventBase
    {
        public StopSkip(IAudioEngine s, EventManager m, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.manager.canSkip = false;
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
