using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class InteriorSet : EventBase
    {
        public SceneMap map;
        public MapField field;

        public InteriorSet(IAudioEngine s, EventManager m, MapField field, SceneMap map, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.field = field;
            this.map = map;
        }

        public override void Update()
        {
            this.field.InteriorSet();
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
