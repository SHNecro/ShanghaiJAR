using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class SityNetBack : BackgroundBase
    {
        public SityNetBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "cityNetback";
            this.size = new Point(128, 96);
            this.flames = 11;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 8;
            this.animespeed = 3;
            this.scrollspeed = new Point(this.animespeed, 0);
            this.animasion[1, 0] = 24;
            this.animasion[1, 5] = 48;
            this.animasion[0, 6] = 5;
            this.animasion[0, 7] = 4;
            this.animasion[0, 8] = 3;
            this.animasion[0, 9] = 2;
            this.animasion[0, 10] = 1;
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
