using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;

namespace NSEvent
{
    internal class RunEvent : EventBase
    {
        private readonly int page;
        private readonly string eventID;
        protected readonly EventManager eventmanager;
        public MapField field;
        private bool lun;
        private bool skip;

        public RunEvent(
          IAudioEngine s,
          EventManager m,
          string ID,
          int p,
          SceneMap map,
          MapField fi,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = false;
            this.eventID = ID;
            this.page = p - 1;
            this.field = fi;
            this.eventmanager = new EventManager(map, this.sound);
        }

        public override void Update()
        {
            if (!this.lun)
            {
                foreach (MapEventBase mapEventBase in this.field.Events)
                {
                    if (mapEventBase.ID == this.eventID)
                    {
                        if (this.page >= 0)
                        {
                            this.eventmanager.EventClone(mapEventBase.eventPages[this.page].eventmanager);
                        }
                        else
                        {
                            mapEventBase.LunPageCheck();
                            this.eventmanager.EventClone(mapEventBase.LunPage.eventmanager);
                        }
                        this.eventmanager.playevent = true;
                        this.lun = true;
                        break;
                    }
                }
            }
            this.eventmanager.UpDate();
            if (this.eventmanager.playevent)
                return;
            this.lun = false;
            this.skip = false;
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            base.SkipUpdate();
        }

        public override void Render(IRenderer dg)
        {
            this.eventmanager.Render(dg);
        }
    }
}
