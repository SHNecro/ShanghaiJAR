using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class DeepUraBack : BackgroundBase
    {
        public DeepUraBack()
          : base(Color.White)
        {
            this.design = true;
            this.picturename = "deepuranet";
            this.size = new Point(40, 84);
            this.flames = 18;
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
