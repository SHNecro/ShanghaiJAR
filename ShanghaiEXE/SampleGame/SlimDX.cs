using SlimDX.Direct3D9;
using System.Drawing;
using System.Windows.Forms;

namespace NSSampleGame
{
    internal class SlimDX
    {
        private Sprite sprite;
        private Texture texture;
        private Form form;
        private Direct3D direct3D;
        private Device device;
        private PresentParameters pp;
        private Texture tex;

        public Form Form
        {
            get
            {
                return this.form;
            }
            private set
            {
                this.form = value;
            }
        }

        public void Init()
        {
            this.form = new Form();
            this.form.ClientSize = new Size(640, 480);
            this.pp = new PresentParameters()
            {
                BackBufferFormat = SlimDX.Direct3D9.Format.X8R8G8B8,
                BackBufferCount = 1,
                BackBufferWidth = this.form.ClientSize.Width,
                BackBufferHeight = this.form.ClientSize.Height,
                Multisample = MultisampleType.None,
                SwapEffect = SwapEffect.Discard,
                EnableAutoDepthStencil = true,
                AutoDepthStencilFormat = SlimDX.Direct3D9.Format.D16,
                PresentFlags = PresentFlags.DiscardDepthStencil,
                PresentationInterval = PresentInterval.Default,
                Windowed = true,
                DeviceWindowHandle = this.form.Handle
            };
            this.direct3D = new Direct3D();
            try
            {
                this.device = new Device(this.direct3D, 0, DeviceType.Hardware, this.form.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters[1]
                {
          this.pp
                });
            }
            catch (Direct3D9Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message, "DirectX Initialization failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.form.Close();
            }
            this.sprite = new Sprite(this.device);
            this.texture = Texture.FromFile(this.device, "roboicon.png");
            this.sprite = new Sprite(this.device);
            this.LoadResource();
        }

        private bool LoadResource()
        {
            this.tex = Texture.FromFile(this.device, "roboicon.png", 0, 0, 0, Usage.None, SlimDX.Direct3D9.Format.Unknown, Pool.Managed, Filter.None, Filter.None, 0);
            return true;
        }
    }
}
