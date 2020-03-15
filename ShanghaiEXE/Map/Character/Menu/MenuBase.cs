using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSMap.Character.Menu
{
    internal class MenuBase : AllBase
    {
        protected SaveData savedata;
        private int alpha;
        protected const string texture = "menuwindows";
        protected TopMenu topmenu;
        public const byte _fadeplus = 51;
        protected const byte waitlong = 10;
        protected const byte waitshort = 4;
        protected int waittime;
        protected Player player;

        protected int Alpha
        {
            set
            {
                this.alpha = value;
                if (this.alpha > byte.MaxValue)
                    this.alpha = byte.MaxValue;
                if (this.alpha >= 0)
                    return;
                this.alpha = 0;
            }
            get
            {
                return this.alpha;
            }
        }

        public MenuBase(IAudioEngine s, Player p, TopMenu t, SaveData save)
          : base(s)
        {
            this.savedata = save;
            this.topmenu = t;
            this.alpha = byte.MaxValue;
            this.player = p;
        }

        public virtual void UpDate()
        {
        }

        public virtual void Render(IRenderer dg)
        {
        }
    }
}
