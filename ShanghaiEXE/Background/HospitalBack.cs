using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class HospitalBack : BackgroundBase
    {
        public HospitalBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = nameof(HospitalBack);
            this.size = new Point(224, 112);
            this.flames = 8;
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
