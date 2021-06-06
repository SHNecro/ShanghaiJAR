using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;
using Common.Vectors;
using System.Drawing;

namespace NSEvent
{
    internal class MapChange : EventBase
    {
        private readonly string mapName;
        private Point position;
        private readonly int floor;
        private readonly MapCharacterBase.ANGLE angle;

        public MapChange(
          IAudioEngine s,
          EventManager m,
          string name,
          Point po,
          int f,
          MapCharacterBase.ANGLE a,
          SaveData save,
          MapField field)
          : base(s, m, save)
        {
            field.parent.parent.MapTextureAdd(name);
            field.parent.parent.MapTextureAdd(save.pluginMap);
            this.NoTimeNext = true;
            this.mapName = name;
            this.position = po;
            this.floor = f;
            this.angle = a;
        }

        public override void Update()
        {
            if (this.manager.parent.Field.mapname != this.mapName)
            {
                this.manager.parent.FieldSet(this.mapName, this.position, this.floor, this.angle);
                this.manager.parent.Player.position = new Vector3(position.X, position.Y, this.floor * (this.manager.parent.Field.Height / 2));
                this.manager.parent.Player.floor = this.floor;
                foreach (EventBase eventBase in this.manager.events)
                {
                    if (eventBase is RunEvent)
                        ((RunEvent)eventBase).field = this.manager.parent.Field;
                }
            }
            else
            {
                this.manager.parent.Player.position = new Vector3(position.X, position.Y, this.floor * (this.manager.parent.Field.Height / 2));
                this.manager.parent.Player.floor = this.floor;
            }
            this.savedata.ValList[19] = 0;
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
