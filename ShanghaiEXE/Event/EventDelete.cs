﻿using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;
using System;

namespace NSEvent
{
    internal class EventDelete : EventBase
    {
        public string deadID;
        private readonly MapField field;

        public EventDelete(MyAudio s, EventManager m, string ID, MapField ma, SaveData save)
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
