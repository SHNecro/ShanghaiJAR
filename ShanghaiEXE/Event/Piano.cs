using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using System.Drawing;
using Common.Vectors;
using System;

namespace NSEvent
{
    internal class Piano : EventBase, IPersistentEvent
    {
        private SceneMap parent;

        private Note note;
        private int volume;
        private int frameDuration;

        public Piano(
          IAudioEngine s,
          EventManager m,
          Note note,
          int volume,
          int frameDuration,
          SceneMap parent,
          SaveData save)
          : base(s, m, save)
        {
			this.NoTimeNext = false;

            this.note = note;
            this.volume = volume;
            this.frameDuration = frameDuration;

            this.parent = parent;
        }

		public bool IsActive { get; set; }

		public override void Update()
		{
            if (!this.sound.IsPlayingNote)
            {
                parent.persistentEvents.Add(this);
                this.IsActive = true;
            }

            this.sound.PlayNote(this.note, this.volume, this.frameDuration);

			this.EndCommand();
		}

		public void PersistentUpdate()
		{
			this.FlameControl(1);
            this.sound.UpdateNoteTick();
            this.IsActive = this.sound.IsPlayingNote;
        }

        public void PersistentRender(IRenderer dg)
        {
        }

        public override void SkipUpdate()
        {
			this.IsActive = false;
        }

		public override void Render(IRenderer dg)
		{
		}
    }
}
