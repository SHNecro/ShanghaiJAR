using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class ChipPlus : EventBase
    {
        private readonly int chipID;
        private readonly int codeNumber;
        private readonly bool upDown;

        public ChipPlus(IAudioEngine s, EventManager m, int id, int code, bool updown, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.chipID = id;
            this.codeNumber = code;
            this.upDown = updown;
        }

        public override void Update()
        {
            if (this.upDown)
                this.savedata.AddChip(this.chipID, this.codeNumber, true);
            else
                this.savedata.LosChip(this.chipID, this.codeNumber);
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
