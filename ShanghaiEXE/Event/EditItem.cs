using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class EditItem : EventBase
    {
        public int number;
        private readonly bool get;
        private readonly bool message;

        public EditItem(IAudioEngine s, EventManager m, int q, bool get, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.get = get;
            this.number = q;
        }

        public override void Update()
        {
            if (this.get)
                this.savedata.keyitem.Add(this.number);
            else
                this.savedata.keyitem.RemoveAll(i => i == this.number);
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
