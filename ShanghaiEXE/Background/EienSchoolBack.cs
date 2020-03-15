using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class EienSchoolBack : BackgroundBase
    {
        public EienSchoolBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "eienSchoolBack";
            this.size = new Point(240, 240);
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
