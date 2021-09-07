using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class BGMon : EventBase
    {
        public string musicname;

        public BGMon(IAudioEngine s, EventManager m, string ID, int ms, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.musicname = ID;
        }

        public override void Update()
        {
            this.sound.StartBGM(this.musicname);

            // prevents 1-frame full-volume when trying to fade in new bgm
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
