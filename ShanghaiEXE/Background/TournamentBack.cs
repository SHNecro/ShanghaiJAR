using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class TournamentBack : BackgroundBase
    {
        public TournamentBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "tournament";
            this.size = new Point(240, 80);
            this.flames = 2;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 30;
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
