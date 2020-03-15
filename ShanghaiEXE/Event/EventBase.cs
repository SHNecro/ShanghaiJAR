using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    public class EventBase : AllBase
    {
        protected EventManager manager;
        protected SaveData savedata;
        public bool NoTimeNext;

        public EventBase(IAudioEngine s, EventManager m, SaveData save)
          : base(s)
        {
            this.savedata = save;
            this.manager = m;
        }

        public virtual void Update()
        {
        }

        public virtual void Render(IRenderer dg)
        {
        }

        protected void EndCommand()
        {
            if (!this.NoTimeNext)
                this.manager.NextEvent();
            else
                this.manager.NoTimeNext();
            this.frame = 0;
        }

        protected void NoTimesUpdate()
        {
            this.NoTimeNext = true;
        }

        protected void NoTimesRender(IRenderer dg)
        {
        }

        public virtual void SkipUpdate()
        {
        }

        public void ManagerChange(EventManager m)
        {
            this.manager = m;
        }
    }
}
