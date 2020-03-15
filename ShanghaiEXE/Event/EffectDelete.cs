using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;

namespace NSEvent
{
    internal class EffectDelete : EventBase
    {
        public string deadID;
        private MapField field;

        public EffectDelete(IAudioEngine s, EventManager m, string ID, MapField ma, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.deadID = ID;
            this.field = ma;
        }

        public override void Update()
        {
            if (this.manager.parent != null)
                this.field = this.manager.parent.Field;
            this.field.effectgenerator.RemoveAll(e => e.ID == this.deadID);
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
