using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class BGMon : EventBase
    {
        public string musicname;

        public BGMon(MyAudio s, EventManager m, string ID, int ms, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.musicname = ID;
        }

        public override void Update()
        {
            this.sound.StartBGM(this.musicname);
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
