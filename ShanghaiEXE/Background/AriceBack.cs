using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class AriceBack : BackgroundBase
    {
        public AriceBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "ariceback";
            this.size = new Point(80, 80);
            this.flames = 8;
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
