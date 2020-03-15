using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class Lavel : EventBase
    {
        public string lavelID;

        public Lavel(IAudioEngine s, EventManager m, string ID, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.lavelID = ID;
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
