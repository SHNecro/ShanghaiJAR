using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;

namespace NSEvent
{
    internal class PlugIn : EventBase
    {
        private readonly byte alpha = byte.MaxValue;
        private const int fulltime = 90;
        private const int animeInterval = 3;
        private readonly Player player;
        private readonly MapField field;

        public PlugIn(IAudioEngine s, EventManager m, Player player, MapField field, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.player = player;
            this.field = field;
        }

        public override void Update()
        {
            this.savedata.isJackedIn = true;
            this.savedata.pluginMap = this.field.mapname;
            this.savedata.pluginX = this.player.position.X;
            this.savedata.pluginY = this.player.position.Y;
            this.savedata.pluginZ = this.player.position.Z;
            this.savedata.pluginFroor = this.player.floor;
            this.player.encountInterval = 300;
            this.savedata.FlagList[2] = true;
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            this.Update();
        }

        public override void Render(IRenderer dg)
        {
        }
    }
}
