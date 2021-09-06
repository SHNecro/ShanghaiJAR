using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class BgmSave : EventBase
    {
        private readonly MapField field;

        public BgmSave(IAudioEngine s, EventManager m, MapField field, SaveData save)
          : base(s, m, save)
        {
            this.field = field;
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.field.saveBGM = this.sound.CurrentBGM;
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
