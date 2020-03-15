using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class editMenu : EventBase
    {
        public int number;
        private readonly bool trueORfalse;

        public editMenu(IAudioEngine s, EventManager m, int n, bool tORf, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.number = n;
            this.trueORfalse = tORf;
        }

        public override void Update()
        {
            this.savedata.canselectmenu[this.number] = this.trueORfalse;
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
