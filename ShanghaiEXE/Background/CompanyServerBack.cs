using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class CompanyServerBack : BackgroundBase
    {
        public CompanyServerBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = nameof(CompanyServerBack);
            this.size = new Point(120, 160);
            this.flames = 20;
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
