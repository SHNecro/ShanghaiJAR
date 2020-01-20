using OpenTK.Graphics.OpenGL;
using Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;
using Common.ExtensionMethods;

namespace Common.OpenGL
{
    public class FolderTextureLoadStrategy : ITextureLoadStrategy
    {
        public event EventHandler<TextureLoadProgressUpdatedEventArgs> ProgressUpdated;

        private readonly string graphicsFormat;

        public FolderTextureLoadStrategy(string graphicsFormat)
        {
            this.graphicsFormat = graphicsFormat;
        }

        public void Load()
        {
            this.ProgressUpdated?.Invoke(this, null);
        }

        public Texture ProvideTexture(string textureName, TextureUnit textureUnit, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {
            return new Texture(string.Format(this.graphicsFormat, textureName), textureUnit, minFilter, magFilter);
        }

        public IEnumerable<string> GetProvidableTextures()
        {
            var files = Directory.EnumerateFiles(Path.GetDirectoryName(this.graphicsFormat));

            var formatInverterFunc = this.graphicsFormat.CreateFormatInverter(files);

            return files.Select(formatInverterFunc).ToArray();
        }

        public bool CanProvideTexture(string textureName)
        {
            return File.Exists(string.Format(this.graphicsFormat, textureName));
        }
    }
}
