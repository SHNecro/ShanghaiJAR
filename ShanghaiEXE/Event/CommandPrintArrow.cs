using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSEvent
{
    internal class CommandPrintArrow : EventBase
    {
        private CommandPrintArrow.ARROW nowarrow = CommandPrintArrow.ARROW.green;
        private const int speed = 15;
        private readonly int roop;
        private int rooping;
        private Vector2 position;

        public CommandPrintArrow(IAudioEngine s, EventManager m, int r, Vector2 p)
          : base(s, m, null)
        {
            this.position = p;
            this.roop = r;
        }

        public override void Update()
        {
            this.FlameControl(10);
            if (!this.moveflame)
                return;
            switch (this.nowarrow)
            {
                case CommandPrintArrow.ARROW.green:
                    this.nowarrow = CommandPrintArrow.ARROW.yellow;
                    this.sound.PlaySE(SoundEffect.teacharrow);
                    break;
                case CommandPrintArrow.ARROW.yellow:
                    ++this.rooping;
                    if (this.rooping >= this.roop)
                    {
                        this.EndCommand();
                        break;
                    }
                    this.nowarrow = CommandPrintArrow.ARROW.green;
                    break;
            }
        }

        public override void Render(IRenderer dg)
        {
            this._position = this.position;
            this._rect = new Rectangle(240 + 24 * (int)this.nowarrow, 24, 24, 24);
            dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
        }

        private enum ARROW
        {
            green,
            yellow,
        }
    }
}
