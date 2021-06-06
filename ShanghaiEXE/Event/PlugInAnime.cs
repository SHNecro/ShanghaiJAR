using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;
using Common.Vectors;
using System.Drawing;

namespace NSEvent
{
    internal class PlugInAnime : EventBase
    {
        private byte alpha = byte.MaxValue;
        private const int fulltime = 90;
        private const int animeInterval = 3;
        private readonly Player player;
        private readonly MapField field;

        public PlugInAnime(IAudioEngine s, EventManager m, Player player, MapField field, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = false;
            this.player = player;
            this.field = field;
        }

        public override void Update()
        {
            if (this.frame == 0 && this.alpha == byte.MaxValue)
                this.sound.StartBGM("plugin");
            if (this.frame < 45 && this.alpha > 0)
                this.alpha -= 15;
            else if (this.frame > 73 && this.alpha < byte.MaxValue)
                this.alpha += 15;
            this.FlameControl(3);
            if (this.frame < 90)
                return;
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

        public override void Render(IRenderer dg)
        {
            Vector2 _point = new Vector2(0.0f, 0.0f);
            Rectangle _rect = new Rectangle(0, this.frame % 4 * 160, 240, 160);
            dg.DrawImage(dg, "plugin", _rect, true, _point, Color.White);
            Color color = Color.FromArgb(alpha, Color.White);
            _rect = new Rectangle(0, 0, 240, 160);
            _point = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", _rect, true, _point, color);
        }
    }
}
