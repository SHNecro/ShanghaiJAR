using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class FinalFloorBack : BackgroundBase
    {
        public FinalFloorBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "finalfloorBack";
            this.size = new Point(224, 256);
            this.flames = 0;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 16;
            this.animespeed = 8;
            this.scrollspeed = new Point(4, 1);
        }

        public override void Update()
        {
            this.UpdateHighSpeed();
        }

        public override void Render(IRenderer dg)
        {
            base.Render(dg);
        }
    }
}
