﻿using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSEvent
{
    internal class BGMoff : EventBase
    {
        private readonly int fadetime;

        public BGMoff(MyAudio s, EventManager m, int fade, SaveData save)
          : base(s, m, save)
        {
            this.fadetime = fade;
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.sound.StopBGM();
            this.sound.BGMVolumeSet(100);
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
