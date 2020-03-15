using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class Wait : EventBase
    {
        public int time;
        private readonly bool keywait;
        private bool waitend;

        public Wait(IAudioEngine s, EventManager m, int t, bool key, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = false;
            this.keywait = key;
            this.time = t;
        }

        public override void Update()
        {
            this.FlameControl(this.time);
            if (this.moveflame)
            {
                if (this.keywait)
                {
                    this.waitend = true;
                }
                else
                {
                    this.EndCommand();
                    return;
                }
            }
            if (!this.waitend || !Input.IsPress(Button._A))
                return;
            this.waitend = false;
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
