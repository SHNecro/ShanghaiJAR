using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class BgmLoad : EventBase
    {
        private readonly MapField field;

        public BgmLoad(IAudioEngine s, EventManager m, MapField field, SaveData save)
          : base(s, m, save)
        {
            this.field = field;
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.sound.StartBGM(this.field.saveBGM);
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
