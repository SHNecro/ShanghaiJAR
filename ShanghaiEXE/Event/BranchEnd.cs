using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class BranchEnd : EventBase
    {
        public BranchEnd(IAudioEngine s, EventManager m, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
