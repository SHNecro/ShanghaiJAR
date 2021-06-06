using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using Common.ExtensionMethods;
using Common.Vectors;
using Common.OpenGL;
using Common.OpenAL;

namespace Common.EncodeDecode
{
    public class FolderLoadStrategy : ITextureLoadStrategy, ISoundLoadStrategy
    {
        public event EventHandler<LoadProgressUpdatedEventArgs> ProgressUpdated;

        private readonly string graphicsFormat;

        public FolderLoadStrategy(string graphicsFormat)
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

        public WAVData ProvideSound(string file)
        {
            var filePath = string.Format(this.graphicsFormat, file);
            using (var fileStream = File.Open(filePath, FileMode.Open))
            {
                return AudioEngine.LoadBytes(fileStream);
            }
        }

        public IEnumerable<string> GetProvidableFiles()
        {
            var files = Directory.EnumerateFiles(Path.GetDirectoryName(this.graphicsFormat));

            var formatInverterFunc = this.graphicsFormat.CreateFormatInverter(files);

            return files.Select(formatInverterFunc).ToArray();
        }

        public bool CanProvideFile(string fileName)
        {
            return File.Exists(string.Format(this.graphicsFormat, fileName));
        }
    }
}
