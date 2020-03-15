using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class CentralBack : BackgroundBase
    {
        public CentralBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "centralback";
            this.size = new Point(128, 64);
            this.flames = 50;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 4;
            this.animespeed = 4;
            this.scrollspeed = new Point(2, 4);

            for (var horizPass = 0; horizPass < 3; horizPass++)
            {
                for (var frame = 0; frame < 7; frame++)
                {
                    this.animasion[0, 14 * horizPass + 2 * frame] = frame;
                    this.animasion[0, 14 * horizPass + 1 + 2 * frame] = frame;
                }
            }

            for (var flash = 0; flash < 4; flash++)
            {
                this.animasion[0, 42 + 2 * flash] = 0;
                this.animasion[0, 42 + 1 + 2 * flash] = 6;
            }
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
