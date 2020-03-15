using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class SchoolBack : BackgroundBase
    {
        public SchoolBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "schoolback";
            this.size = new Point(64, 64);
            this.flames = 40;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 12;
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
