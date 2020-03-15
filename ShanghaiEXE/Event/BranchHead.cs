using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class BranchHead : EventBase
    {
        public int question;

        public BranchHead(IAudioEngine s, EventManager m, int q, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.question = q;
        }

        public override void Update()
        {
            while (this.manager.events.Count > this.manager.Playeventnumber && (!(this.manager.events[this.manager.Playeventnumber] is BranchHead) || ((BranchHead)this.manager.events[this.manager.Playeventnumber]).question != this.savedata.selectQuestion))
            {
                ++this.manager.Playeventnumber;
                if (this.manager.events[this.manager.Playeventnumber] is BranchEnd)
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
