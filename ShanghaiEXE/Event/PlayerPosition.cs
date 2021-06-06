using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;

namespace NSEvent
{
    internal class PlayerPosition : EventBase
    {
        public int ifID;

        public PlayerPosition(IAudioEngine s, EventManager m, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            Vector3 position = this.manager.parent.Player.Position;
            this.savedata.ValList[0] = (int)position.X;
            this.savedata.ValList[1] = (int)position.Y;
            this.savedata.ValList[2] = (int)position.Z;
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
