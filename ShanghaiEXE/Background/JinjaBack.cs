using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class JinjaBack : BackgroundBase
    {
        public JinjaBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "Jinjaback";
            this.size = new Point(104, 80);
            this.flames = 4;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 24;
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
