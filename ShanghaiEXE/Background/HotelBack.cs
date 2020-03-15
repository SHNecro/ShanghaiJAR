using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class HotelBack : BackgroundBase
    {
        public HotelBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "hotelBack";
            this.size = new Point(80, 288);
            this.flames = 12;
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
