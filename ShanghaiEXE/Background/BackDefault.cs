using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class BackDefault : BackgroundBase
    {
        public BackDefault()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "backdefault";
            this.size = new Point(64, 64);
            this.flames = 6;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 6;
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
