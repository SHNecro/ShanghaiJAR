namespace NSShanghaiEXE.InputOutput.Rendering.DirectX9
{
    public class SlimFont : GraphicsClass
    {
        public SlimDX.Direct3D9.Font font;

        public SlimDX.Direct3D9.Font Font
        {
            get
            {
                return this.font;
            }
        }

        public SlimFont(System.Drawing.Font font)
        {
            this.font = this.LoadFont(font);
        }

        private SlimDX.Direct3D9.Font LoadFont(System.Drawing.Font font)
        {
            return new SlimDX.Direct3D9.Font(GraphicsClass.device, font);
        }

        public override void Dispose()
        {
            this.font.Dispose();
        }
    }
}
