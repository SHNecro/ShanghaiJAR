﻿using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class UraBack2 : BackgroundBase
    {
        public UraBack2()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "uraNetBack2";
            this.size = new Point(24, 48);
            this.flames = 5;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 8;
            this.animespeed = 3;
            this.scrollspeed = new Point(this.animespeed, this.animespeed);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Render(IRenderer dg)
        {
            base.Render(dg);
        }
    }
}
