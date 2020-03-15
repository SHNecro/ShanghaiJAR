using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class EventDelete : EventBase
    {
        public string deadID;
        private readonly MapField field;

        public EventDelete(IAudioEngine s, EventManager m, string ID, MapField ma, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.deadID = ID;
            this.field = ma;
        }

        public override void Update()
        {
            this.field.Events.RemoveAll(e => e.ID == this.deadID);
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
