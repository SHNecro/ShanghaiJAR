using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Common.OpenGL
{
    //A helper class, much like Shader, meant to simplify loading textures.
    public class Texture : IDisposable
    {
        public static Texture NotLoaded { get; } = new Texture();

        private readonly int Handle;

        //Create texture from path.
        public Texture(string pathUnchecked, TextureUnit unit = TextureUnit.Texture0, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {

            this.Loaded = File.Exists(pathUnchecked);
            if (!this.Loaded)
            {
                return;
            }

            var path = pathUnchecked;

            //Generate handle
            this.Handle = GL.GenTexture();

            //Bind the handle
            this.Use(unit);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            byte[] pixelData;
            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                var raw = new Bitmap(stream);
                var image = raw.LockBits(new Rectangle(0, 0, raw.Width, raw.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                this.Size = new Size(image.Width, image.Height);
                pixelData = new byte[Math.Abs(image.Stride * image.Height)];
                Marshal.Copy(image.Scan0, pixelData, 0, pixelData.Length);
                raw.UnlockBits(image);
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, this.Width, this.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, pixelData);
        }

        //Create texture from bitmap.
        public Texture(Bitmap bitmap, TextureUnit unit = TextureUnit.Texture0, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {
            //Generate handle
            this.Handle = GL.GenTexture();

            //Bind the handle
            this.Use(unit);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            byte[] pixelData;
            var image = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            this.Size = new Size(image.Width, image.Height);
            pixelData = new byte[Math.Abs(image.Stride * image.Height)];
            Marshal.Copy(image.Scan0, pixelData, 0, pixelData.Length);
            bitmap.UnlockBits(image);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, this.Width, this.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, pixelData);
            this.Loaded = true;
        }

        private Texture()
        {
            this.Loaded = false;
        }

        public Size Size { get; }

        public int Width => this.Size.Width;
        public int Height => this.Size.Height;

        public bool Loaded { get; }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        #region IDisposable
        private bool alreadyDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!alreadyDisposed)
            {
                if (disposing)
                {
                }

                GL.DeleteTexture(Handle);

                alreadyDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
