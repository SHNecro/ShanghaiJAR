using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSMap.Character;

namespace NSEvent
{
    internal class OpenMenu : EventBase
    {
        private readonly Player player;

        public OpenMenu(IAudioEngine s, EventManager m, Player pl)
          : base(s, m, null)
        {
            this.player = pl;
            this.player.OpenMenu();
            this.player.NoPrint = true;
        }

        public override void Update()
        {
            if (!this.player.openMenu)
                this.EndCommand();
            else
                this.player.Update();
        }

        public override void Render(IRenderer dg)
        {
            this.player.Render(dg);
        }
    }
}
