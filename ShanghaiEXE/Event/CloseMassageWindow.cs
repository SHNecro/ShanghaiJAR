using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEvent
{
    internal class CloseMassageWindow : EventBase
    {
        private const int speed = 2;
        private const int maxflame = 4;
        private bool printend;

        public CloseMassageWindow(IAudioEngine s, EventManager m)
          : base(s, m, null)
        {
        }

        public override void Update()
        {
			if (this.frame == 0)
			{
				this.printend = false;
			}

            this.FlameControl(2);
            if (this.frame <= 4)
                return;
            this.printend = true;
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            if (this.manager.skip)
                return;
            if (!this.printend)
            {
                this._position = new Vector2(0.0f, 104f);
                this._rect = new Rectangle(0, 56 * this.frame, 240, 56);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            }
        }
    }
}
