using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;

namespace NSEvent
{
    internal class RunEventParallel : RunEvent, IPersistentEvent
    {
        // Prevents a parallel runevent from marking the parent as "complete" in the middle of another event
        private EventManager detachedManager;

        public RunEventParallel(
          IAudioEngine s,
          EventManager m,
          string ID,
          int p,
          SceneMap map,
          MapField fi,
          SaveData save)
          : base(s, m, ID, p, map, fi, save)
        {
            this.detachedManager = new EventManager(map, s);
        }

        public bool IsActive { get; set; }

        public override void Update()
        {
            this.eventmanager.parent.persistentEvents.Add(this);
            this.IsActive = true;
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            base.Render(dg);
        }

        public override void SkipUpdate()
        {
            this.eventmanager.playevent = false;
        }

        public void PersistentRender(IRenderer dg)
        {
            base.Render(dg);
        }

        public void PersistentUpdate()
        {
            base.Update();
            this.IsActive = this.eventmanager.playevent;
            this.ManagerChange(this.detachedManager);
        }
    }
}
