using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;

namespace NSEvent
{
    internal class EventMoveEnd : EventBase
    {
        private readonly MapField field;

        public EventMoveEnd(IAudioEngine s, EventManager m, MapField fi, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = false;
            this.field = fi;
        }

        public override void Update()
        {
            bool flag = true;
            foreach (MapCharacterBase mapCharacterBase in this.field.Events)
            {
                if ((uint)mapCharacterBase.moveOrder.Length > 0U)
                {
                    flag = false;
                    break;
                }
                if ((uint)this.manager.parent.Player.moveOrder.Length > 0U)
                {
                    flag = false;
                    break;
                }
            }
            if (!flag)
                return;
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            foreach (MapEventBase mapEventBase in this.field.Events)
            {
                mapEventBase.MoveEndPosi();
                mapEventBase.moveOrder = new NSMap.Character.EventMove[0];
            }
            this.manager.parent.Player.MoveEndPosi();
            this.manager.parent.Player.moveOrder = new NSMap.Character.EventMove[0];
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
