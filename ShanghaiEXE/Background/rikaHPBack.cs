using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class rikaHPBack : BackgroundBase
    {
        public rikaHPBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "rikaHPback";
            this.size = new Point(240, 160);
            this.flames = 13;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 8;
            this.animespeed = 3;
            this.scrollspeed = new Point(0, 0);
            this.scroll.X = 120;
            this.scroll.Y = 80;
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
