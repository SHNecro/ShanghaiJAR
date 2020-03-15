using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class NAback : BackgroundBase
    {
        public NAback()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = nameof(NAback);
            this.size = new Point(256, 256);
            this.flames = 0;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 16;
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
