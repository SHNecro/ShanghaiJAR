using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class MariBack : BackgroundBase
    {
        public MariBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "mariHPback";
            this.size = new Point(64, 64);
            this.flames = 4;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 12;
            this.animespeed = 4;
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
