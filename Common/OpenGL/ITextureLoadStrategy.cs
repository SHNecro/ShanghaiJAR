using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace Common.OpenGL
{
    public interface ITextureLoadStrategy
    {
        event EventHandler<TextureLoadProgressUpdatedEventArgs> ProgressUpdated;

        void Load();

        Texture ProvideTexture(string textureName, TextureUnit textureUnit = TextureUnit.Texture0, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Nearest);

        IEnumerable<string> GetProvidableTextures();

        bool CanProvideTexture(string textureName);
    }
}
