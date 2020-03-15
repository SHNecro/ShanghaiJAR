using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class QuestEnd : EventBase
    {
        public QuestEnd(IAudioEngine s, EventManager m, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            if (this.savedata.ValList[11] >= 0)
            {
                this.savedata.questEnd[this.savedata.ValList[11]] = true;
                this.savedata.ValList[11] = -1;
                this.savedata.ValList[4] = 0;
            }
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
