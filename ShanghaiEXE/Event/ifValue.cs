using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class ifValue : EventBase
    {
        public int number;
        private readonly bool numberORvalue;
        private readonly int referenceNo;
        private readonly byte math;
        private int ifID;

        public ifValue(
          IAudioEngine s,
          EventManager m,
          int n,
          bool nORv,
          int rN,
          byte ma,
          int ID,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.number = n;
            this.numberORvalue = nORv;
            this.referenceNo = rN;
            this.math = ma;
            this.ifID = ID;
        }

        public override void Update()
        {
            while (this.manager.events.Count > this.manager.Playeventnumber)
            {
                switch (this.math)
                {
                    case 0:
                        if (this.savedata.ValList[this.number] != (this.numberORvalue ? this.savedata.ValList[this.referenceNo] : this.referenceNo))
                            break;
                        goto label_12;
                    case 1:
                        if (this.savedata.ValList[this.number] < (this.numberORvalue ? this.savedata.ValList[this.referenceNo] : this.referenceNo))
                            break;
                        goto label_12;
                    case 2:
                        if (this.savedata.ValList[this.number] > (this.numberORvalue ? this.savedata.ValList[this.referenceNo] : this.referenceNo))
                            break;
                        goto label_12;
                    case 3:
                        if (this.savedata.ValList[this.number] <= (this.numberORvalue ? this.savedata.ValList[this.referenceNo] : this.referenceNo))
                            break;
                        goto label_12;
                    case 4:
                        if (this.savedata.ValList[this.number] >= (this.numberORvalue ? this.savedata.ValList[this.referenceNo] : this.referenceNo))
                            break;
                        goto label_12;
                    case 5:
                        if (this.savedata.ValList[this.number] == (this.numberORvalue ? this.savedata.ValList[this.referenceNo] : this.referenceNo))
                            break;
                        goto label_12;
                }
                ++this.manager.Playeventnumber;
                if (this.manager.events[this.manager.Playeventnumber] is ifValue)
                {
                    ifValue ifValue = (ifValue)this.manager.events[this.manager.Playeventnumber];
                    if (ifValue.ifID == this.ifID && ifValue != this)
                        break;
                }
                if (this.manager.events[this.manager.Playeventnumber] is ifEnd && ((ifEnd)this.manager.events[this.manager.Playeventnumber]).ifID == this.ifID)
                    break;
            }
        label_12:
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
