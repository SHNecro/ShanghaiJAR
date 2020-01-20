using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class ifEnd : EventBase
    {
        public int ifID;

        public ifEnd(MyAudio s, EventManager m, int ID, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.ifID = ID;
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
