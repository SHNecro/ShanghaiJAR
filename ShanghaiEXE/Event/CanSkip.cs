using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using System.Drawing;
using System.Windows.Forms;

namespace NSEvent
{
    internal class CanSkip : EventBase
    {
        public CanSkip(MyAudio s, EventManager m, SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
        }

        public override void Update()
        {
            this.manager.canSkip = true;
            int playeventnumber = this.manager.Playeventnumber;
            do
            {
                ++playeventnumber;
                try
                {
                    if (this.manager.events[playeventnumber] is Battle)
                    {
                        this.manager.skipColor = Color.White;
                        break;
                    }
                    if (this.manager.events[playeventnumber] is StopSkip)
                    {
                        this.manager.skipColor = Color.Black;
                        break;
                    }
                }
                catch
                {
                    this.manager.canSkip = false;
                    int num = (int)MessageBox.Show("Script Error:", "No Skip End Found");
                }
            }
            while (playeventnumber < this.manager.events.Count);
            this.EndCommand();
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
