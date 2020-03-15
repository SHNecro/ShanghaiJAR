using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class IngBack : BackgroundBase
    {
        public IngBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "ingback";
            this.size = new Point(80, 256);
            this.flames = 32;
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
