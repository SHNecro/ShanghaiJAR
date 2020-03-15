using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap.Character;

namespace NSEvent
{
    internal class PlayerHide : EventBase
    {
        public bool hide;
        private readonly Player player;

        public PlayerHide(IAudioEngine s, EventManager m, bool hide, Player player, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.hide = hide;
            this.player = player;
        }

        public override void Update()
        {
            this.player.NoPrint = this.hide;
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
