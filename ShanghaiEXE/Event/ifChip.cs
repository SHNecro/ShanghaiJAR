using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class ifChip : EventBase
    {
        public int chipnumber;
        public int codenumber;
        private readonly bool trueORfalse;
        private int ifID;

        public ifChip(IAudioEngine s, EventManager m, int n, int c, bool tORf, int ID, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.chipnumber = n;
            this.codenumber = c;
            this.trueORfalse = tORf;
            this.ifID = ID;
        }

        public override void Update()
        {
            while (this.manager.events.Count > this.manager.Playeventnumber && this.trueORfalse != this.savedata.Havechip[this.chipnumber, this.codenumber] >= 1)
            {
                ++this.manager.Playeventnumber;
                if (this.manager.events[this.manager.Playeventnumber] is ifChip)
                {
                    ifChip ifChip = (ifChip)this.manager.events[this.manager.Playeventnumber];
                    if (ifChip.ifID == this.ifID && ifChip != this)
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
