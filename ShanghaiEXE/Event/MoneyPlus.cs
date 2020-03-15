using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class MoneyPlus : EventBase
    {
        private readonly int manyMoney;
        private readonly bool upDown;

        public MoneyPlus(IAudioEngine s, EventManager m, int money, bool updown, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.manyMoney = money;
            this.upDown = updown;
        }

        public override void Update()
        {
            this.savedata.Money += this.upDown ? this.manyMoney : -this.manyMoney;
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
