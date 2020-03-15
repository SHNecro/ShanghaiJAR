using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class RefrigeratorBack : BackgroundBase
    {
        public RefrigeratorBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "refrigeratorback";
            this.size = new Point(64, 32);
            this.flames = 25;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 8;
            this.animespeed = 3;
            this.scrollspeed = new Point(8, 8);
            int index1 = 0;
            for (int index2 = 0; index2 < 4; ++index2)
            {
                for (int index3 = 0; index3 < 4; ++index3)
                {
                    this.animasion[0, index1] = index3 + 14;
                    ++index1;
                }
            }
            for (int index2 = 0; index2 < 14; ++index2)
            {
                this.animasion[0, index1] = index2;
                ++index1;
            }
            this.flames = index1;
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
