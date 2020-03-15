using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class GenNetBack : BackgroundBase
    {
        public GenNetBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "genNetback";
            this.size = new Point(32, 96);
            this.flames = 25;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 8;
            this.animespeed = 3;
            this.scrollspeed = new Point(this.animespeed * 2, this.animespeed);
            this.animasion[1, 0] = 16;
            this.animasion[1, 13] = 48;
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
