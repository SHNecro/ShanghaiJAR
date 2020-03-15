using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class HeavenBack : BackgroundBase
    {
        public HeavenBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "heavenNetBack";
            this.size = new Point(240, 160);
            this.flames = 8;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 16;
            this.animespeed = 3;
            this.scrollspeed = new Point(0, 0);
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
