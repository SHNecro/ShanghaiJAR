using NSBattle;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class MindChange : EventBase
    {
        public MindWindow mind;
        private readonly MindWindow.MIND tomind;

        public MindChange(
          MyAudio s,
          EventManager m,
          MindWindow mi,
          MindWindow.MIND tom,
          SaveData save)
          : base(s, m, save)
        {
            this.mind = mi;
            this.tomind = tom;
        }

        public override void SkipUpdate()
        {
            this.Update();
        }

        public override void Update()
        {
            this.mind.MindNow = this.tomind;
            this.EndCommand();
        }
    }
}
