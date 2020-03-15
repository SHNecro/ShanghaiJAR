using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class SeirenNetBack2 : BackgroundBase
    {
        public SeirenNetBack2()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "seirenNetBack2";
            this.size = new Point(120, 120);
            this.flames = 8;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 12;
            this.animespeed = 4;
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
