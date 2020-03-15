using NSShanghaiEXE.InputOutput.Rendering;
using System.Drawing;

namespace NSBackground
{
    internal class ROMback : BackgroundBase
    {
        public ROMback(int color)
          : base(Color.White)
        {
            this.design = true;
            ++color;
            this.picturename = nameof(ROMback) + color.ToString();
            this.size = new Point(80, 192);
            this.flames = 25;
            this.type = BackgroundBase.BACKTYPE.scroll;
            this.speed = 16;
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
