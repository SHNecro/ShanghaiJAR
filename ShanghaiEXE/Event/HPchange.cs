using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class HPchange : EventBase
    {
        private readonly int HP;
        private readonly bool upDown;

        public HPchange(IAudioEngine s, EventManager m, int hp, bool updown, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.HP = hp;
            this.upDown = updown;
        }

        public override void Update()
        {
            this.savedata.HPNow += this.upDown ? this.HP : -this.HP;
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
