using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using Common.OpenGL;
using Common.OpenAL;

namespace Common.EncodeDecode
{
    public class FolderOverrideLoadStrategy : ITextureLoadStrategy, ISoundLoadStrategy
    {
        private TCDLoadStrategy tcdLoader;
        private FolderLoadStrategy folderLoader;

        public event EventHandler<LoadProgressUpdatedEventArgs> ProgressUpdated;

        public FolderOverrideLoadStrategy(string tcdPatternOrResource, string password, string fileFormat, string graphicsFormat)
        {
            this.tcdLoader = new TCDLoadStrategy(tcdPatternOrResource, password, fileFormat);
            this.folderLoader = new FolderLoadStrategy(graphicsFormat);

            this.tcdLoader.ProgressUpdated += (sender, args) => this.ProgressUpdated?.Invoke(this, args);
        }

        public void Load()
        {
            this.tcdLoader.Load();
            this.folderLoader.Load();
        }

        public Texture ProvideTexture(string textureName, TextureUnit textureUnit, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {
            var tcdTexture = this.tcdLoader.ProvideTexture(textureName, textureUnit, minFilter, magFilter);
            if (this.folderLoader.CanProvideFile(textureName) || tcdTexture == Texture.NotLoaded)
            {
                return this.folderLoader.ProvideTexture(textureName, textureUnit, minFilter, magFilter);
            }
            return tcdTexture;
        }

        public WAVData ProvideSound(string file)
        {
            return this.tcdLoader.ProvideSound(file);
        }

        public IEnumerable<string> GetProvidableFiles()
        {
            return this.tcdLoader.GetProvidableFiles();
        }

        public bool CanProvideFile(string fileName)
        {
            return this.tcdLoader.CanProvideFile(fileName);
        }
    }
}
