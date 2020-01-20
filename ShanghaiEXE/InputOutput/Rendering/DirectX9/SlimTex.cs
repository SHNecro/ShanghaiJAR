using SlimDX.Direct3D9;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace NSShanghaiEXE.InputOutput.Rendering.DirectX9
{
    public class SlimTex : GraphicsClass
    {
        public int Width = 2;
        public int Height = 2;
        public Texture tex;

        public Texture Texture
        {
            get
            {
                return this.tex;
            }
        }

        public SlimTex(Stream Memory, Usage num)
        {
            this.tex = this.LoadTexture(new Bitmap(Memory), num);
        }

        public SlimTex(string fileName, Usage num)
        {
            this.tex = this.LoadTexture(new Bitmap(fileName), num);
        }

        public SlimTex(Image src, Usage num)
        {
            this.tex = this.LoadTexture(src, num);
        }

        public SlimTex(string text, System.Drawing.Font font, int width, int height)
        {
            this.tex = this.LoadTexture(text, font, width, height);
        }

        private Texture LoadTexture(Image src, Usage num)
        {
            while (this.Width < src.Width)
                this.Width *= 2;
            while (this.Height < src.Height)
                this.Height *= 2;
            this.tex = new Texture(GraphicsClass.device, this.Width, this.Height, 1, num, Format.A8R8G8B8, Pool.Managed);
            using (Graphics graphics = Graphics.FromHdc(this.tex.GetSurfaceLevel(0).GetDC()))
            {
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(src, 0, 0);
            }
            return this.tex;
        }

        private Texture LoadTexture(string text, System.Drawing.Font font, int width, int height)
        {
            while (this.Width < width)
                this.Width *= 2;
            while (this.Height < height)
                this.Height *= 2;
            this.tex = new Texture(GraphicsClass.device, this.Width, this.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            using (Graphics graphics = Graphics.FromHdc(this.tex.GetSurfaceLevel(0).GetDC()))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                graphics.DrawString(text, font, Brushes.White, 0.0f, 0.0f);
            }
            return this.tex;
        }

        public override void Dispose()
        {
            this.tex.Dispose();
        }
    }
}
