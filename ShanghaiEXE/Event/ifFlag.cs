using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class ifFlag : EventBase
    {
        public int flagnumber;
        private readonly bool trueORfalse;
        private int ifID;

        public ifFlag(IAudioEngine s, EventManager m, int n, bool tORf, int ID, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.flagnumber = n;
            this.trueORfalse = tORf;
            this.ifID = ID;
        }

        public override void Update()
        {
            while (this.manager.events.Count > this.manager.Playeventnumber && this.savedata.FlagList[this.flagnumber] != this.trueORfalse)
            {
                ++this.manager.Playeventnumber;
                if (this.manager.events[this.manager.Playeventnumber] is ifFlag)
                {
                    ifFlag ifFlag = (ifFlag)this.manager.events[this.manager.Playeventnumber];
                    if (ifFlag.ifID == this.ifID && ifFlag != this)
                        break;
                }
                if (this.manager.events[this.manager.Playeventnumber] is ifEnd && ((ifEnd)this.manager.events[this.manager.Playeventnumber]).ifID == this.ifID)
                    break;
            }
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
