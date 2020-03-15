using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using System;

namespace NSEvent
{
    internal class SEmon : EventBase
    {
        public string lavelID;

        public SEmon(IAudioEngine s, EventManager m, string ID, int ms, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.lavelID = ID;
        }

        public override void Update()
        {
            this.sound.PlaySE((SoundEffect)Enum.Parse(typeof(SoundEffect), this.lavelID));
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
