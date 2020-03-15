using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class editFlag : EventBase
    {
        public int number;
        private readonly bool trueORfalse;
        private readonly bool rebirth;
        private readonly bool numberORvalue;

        public editFlag(IAudioEngine s, EventManager m, int n, bool nORv, int tORf, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.number = n;
            this.numberORvalue = nORv;
            if (this.numberORvalue)
                return;
            switch (tORf)
            {
                case 0:
                    this.trueORfalse = false;
                    break;
                case 1:
                    this.trueORfalse = true;
                    break;
                case 2:
                    this.rebirth = true;
                    break;
            }
        }

        public editFlag(IAudioEngine s, EventManager m, int n, bool tORf, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.number = n;
            this.numberORvalue = false;
            this.trueORfalse = tORf;
        }

        public override void Update()
        {
            int index = this.numberORvalue ? this.savedata.ValList[this.number] : this.number;
            if (this.rebirth)
                this.savedata.FlagList[index] = !this.savedata.FlagList[index];
            else
                this.savedata.FlagList[index] = this.trueORfalse;
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
