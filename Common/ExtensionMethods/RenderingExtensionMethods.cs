using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Common.ExtensionMethods
{
    public static class RenderingExtensionMethods
    {
        public static int ToInt(this TextureUnit unit)
        {
            switch (unit)
            {
                case TextureUnit.Texture0:
                    return 0;
                case TextureUnit.Texture1:
                    return 1;
                case TextureUnit.Texture2:
                    return 2;
                case TextureUnit.Texture3:
                    return 3;
                case TextureUnit.Texture4:
                    return 4;
                case TextureUnit.Texture5:
                    return 5;
                case TextureUnit.Texture6:
                    return 6;
                case TextureUnit.Texture7:
                    return 7;
                case TextureUnit.Texture8:
                    return 8;
                case TextureUnit.Texture9:
                    return 9;
                case TextureUnit.Texture10:
                    return 10;
                case TextureUnit.Texture11:
                    return 11;
                case TextureUnit.Texture12:
                    return 12;
                case TextureUnit.Texture13:
                    return 13;
                case TextureUnit.Texture14:
                    return 14;
                case TextureUnit.Texture15:
                    return 15;
                default:
                    throw new NotImplementedException(" Only 16 texture units guaranteed by opengl spec");
            }
        }

        public static TextureUnit ToTextureUnit(this int unit)
        {
            switch (unit)
            {
                case 0:
                    return TextureUnit.Texture0;
                case 1:
                    return TextureUnit.Texture1;
                case 2:
                    return TextureUnit.Texture2;
                case 3:
                    return TextureUnit.Texture3;
                case 4:
                    return TextureUnit.Texture4;
                case 5:
                    return TextureUnit.Texture5;
                case 6:
                    return TextureUnit.Texture6;
                case 7:
                    return TextureUnit.Texture7;
                case 8:
                    return TextureUnit.Texture8;
                case 9:
                    return TextureUnit.Texture9;
                case 10:
                    return TextureUnit.Texture10;
                case 11:
                    return TextureUnit.Texture11;
                case 12:
                    return TextureUnit.Texture12;
                case 13:
                    return TextureUnit.Texture13;
                case 14:
                    return TextureUnit.Texture14;
                case 15:
                    return TextureUnit.Texture15;
                default:
                    throw new NotImplementedException(" Only 16 texture units guaranteed by opengl spec");
            }
        }

        public static Bitmap Crop(this Bitmap b, Rectangle r)
        {
            Bitmap nb = new Bitmap(r.Width, r.Height);
            using (var g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -r.X, -r.Y);
            }
            return nb;
        }
    }
}
