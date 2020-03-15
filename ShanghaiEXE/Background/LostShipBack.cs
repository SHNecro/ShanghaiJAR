using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class LostShipBack : BackgroundBase
    {
        public LostShipBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "lostshipback";
            this.size = new Point(120, 64);
            this.flames = 1;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 30;
            this.animespeed = 8;
            this.scrollspeed = new Point(this.animespeed / 2, this.animespeed);
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
