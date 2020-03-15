using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class ClockTowerBack : BackgroundBase
    {
        public ClockTowerBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = nameof(ClockTowerBack);
            this.size = new Point(320, 160);
            this.flames = 12;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 8;
            this.animespeed = 3;
            this.scrollspeed = new Point(this.animespeed, 0);
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
