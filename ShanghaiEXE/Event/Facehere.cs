using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;

namespace NSEvent
{
    internal class Facehere : EventBase
    {
        private readonly SceneMap map;

        public Facehere(MyAudio s, EventManager m, SceneMap map, SaveData save)
          : base(s, m, save)
        {
            this.map = map;
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.map.Player.Angle = MapCharacterBase.ANGLE.DOWN;
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
