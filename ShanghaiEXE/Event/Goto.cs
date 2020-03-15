using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class Goto : EventBase
    {
        public string lavelID;

        public Goto(IAudioEngine s, EventManager m, string ID, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.lavelID = ID;
        }

        public override void Update()
        {
            int num = 0;
            foreach (EventBase eventBase in this.manager.events)
            {
                if (eventBase is Lavel && ((Lavel)eventBase).lavelID == this.lavelID)
                {
                    this.manager.Playeventnumber = num;
                    break;
                }
                ++num;
            }
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
