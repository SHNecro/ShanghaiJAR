using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEvent
{
    internal class OpenMassageWindow : EventBase
    {
        private const int speed = 2;
        private const int maxflame = 3;
        private int anime;

        public OpenMassageWindow(IAudioEngine s, EventManager m)
          : base(s, m, null)
        {
            this.frame = 0;
        }

        public override void Update()
		{
			if (this.frame == 0)
				this.anime = 0;

			this.FlameControl(2);
            if (this.moveflame && this.anime < 3)
                ++this.anime;
            if (3 > this.anime)
                return;
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            if (!this.manager.skip)
            {
                this._position = new Vector2(0.0f, 104f);
                this._rect = new Rectangle(0, 56 * (3 - this.anime), 240, 56);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            }
        }
    }
}
