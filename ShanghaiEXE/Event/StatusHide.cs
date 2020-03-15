using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class StatusHide : EventBase
    {
        public bool hide;
        public SceneMap map;

        public StatusHide(IAudioEngine s, EventManager m, bool hide, SceneMap map, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.hide = hide;
            this.map = map;
        }

        public override void Update()
        {
            this.map.hideStatus = this.hide;
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
