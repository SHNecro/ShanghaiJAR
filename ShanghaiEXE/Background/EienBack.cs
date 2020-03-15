using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class EienBack : BackgroundBase
    {
        public EienBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "eienback";
            this.size = new Point(48, 160);
            this.flames = 6;
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
