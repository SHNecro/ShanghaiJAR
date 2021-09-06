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

            // prevents 1-frame full-volume when trying to fade in sound after battle
            if (this.manager.Playeventnumber + 1 < this.manager.events.Count)
            {
                if (this.manager.events[this.manager.Playeventnumber + 1] is BGMFade fadeEvent
                    && fadeEvent.fadeingTime == 0)
                {
                    this.sound.BGMVolumeSet(fadeEvent.endparsent);
                }
            }

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
