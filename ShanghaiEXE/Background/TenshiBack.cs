using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class TenshiBack : BackgroundBase
    {
        public TenshiBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "tenshiBack";
            this.size = new Point(160, 120);
            this.flames = 16;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 16;
            this.animespeed = 3;
            this.scrollspeed = new Point(this.animespeed, this.animespeed * 2);
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
