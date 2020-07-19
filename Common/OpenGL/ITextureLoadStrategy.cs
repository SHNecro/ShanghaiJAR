using Common.EncodeDecode;
using OpenTK.Graphics.OpenGL;

namespace Common.OpenGL
{
    public interface ITextureLoadStrategy : ILoadStrategy
    {
        Texture ProvideTexture(string textureName, TextureUnit textureUnit = TextureUnit.Texture0, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Nearest);
    }
}
