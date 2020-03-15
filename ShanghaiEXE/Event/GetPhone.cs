using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class GetPhone : EventBase
    {
        private readonly SceneMap map;

        public GetPhone(IAudioEngine s, EventManager m, SceneMap map, SaveData save)
          : base(s, m, save)
        {
            this.map = map;
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.sound.PlaySE(SoundEffect.phone);
            this.map.MailOn(false);
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
