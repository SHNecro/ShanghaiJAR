using OpenTK.Graphics.OpenGL;
using Common.EncodeDecode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Common.ExtensionMethods;
using System.Windows;
using System.Security.Cryptography;

namespace Common.OpenGL
{
    public class TCDTextureLoadStrategy : ITextureLoadStrategy
    {
        public static readonly string ResourceSuffix = "Resource.tcd";
        public static readonly string PatternSuffix = "Pattern.tcd";

        private readonly string selectedFile;
        private readonly string password;
        private readonly string graphicsFormat;
        private readonly Dictionary<string, byte[]> textureBytes;

        private bool texturesLoaded;

        public event EventHandler<TextureLoadProgressUpdatedEventArgs> ProgressUpdated;

        public TCDTextureLoadStrategy(string tcdPatternOrResource, string password, string graphicsFormat)
        {
            this.selectedFile = tcdPatternOrResource;
            this.password = password;
            this.graphicsFormat = graphicsFormat;

            this.textureBytes = new Dictionary<string, byte[]>();
        }

        public void Load()
        {
            string resourceFile, patternFile, fileBase;
            resourceFile = patternFile = fileBase = string.Empty;
            if (!File.Exists(this.selectedFile))
            {
                throw new InvalidOperationException($"Missing graphics file \"{this.selectedFile}\".");
            }
            else if (this.selectedFile.EndsWith(TCDTextureLoadStrategy.ResourceSuffix, StringComparison.InvariantCultureIgnoreCase))
            {
                fileBase = Path.GetFileName(this.selectedFile.Substring(0, this.selectedFile.Length - TCDTextureLoadStrategy.ResourceSuffix.Length));
                resourceFile = this.selectedFile;
                patternFile = Path.Combine(Path.GetDirectoryName(this.selectedFile), fileBase + TCDTextureLoadStrategy.PatternSuffix);
                if (!File.Exists(patternFile))
                {
                    throw new InvalidOperationException($"Missing associated pattern file \"{patternFile}\".");
                }
            }
            else if (this.selectedFile.EndsWith(TCDTextureLoadStrategy.PatternSuffix, StringComparison.InvariantCultureIgnoreCase))
            {
                fileBase = Path.GetFileName(this.selectedFile.Substring(0, this.selectedFile.Length - TCDTextureLoadStrategy.PatternSuffix.Length));
                resourceFile = Path.Combine(Path.GetDirectoryName(this.selectedFile), fileBase + TCDTextureLoadStrategy.ResourceSuffix);
                patternFile = this.selectedFile;
                if (!File.Exists(resourceFile))
                {
                    throw new InvalidOperationException($"Missing associated resource file \"{resourceFile}\".");
                }
            }
            else
            {
                throw new InvalidOperationException($"Invalid graphics file \"{this.selectedFile}\".{Environment.NewLine}Expected a \"<name>Resource\" or \"<name>Pattern\" .tcd file.");
            }

            var unpackThread = new Thread(() =>
            {
                try
                {
                    using (var rsr = new BinaryReader(File.OpenRead(resourceFile), Encoding.GetEncoding("Shift_JIS")))
                    {
                        using (var psr = new StreamReader(File.OpenRead(patternFile), Encoding.GetEncoding("Shift_JIS")))
                        {
                            var files = psr.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            var fileCount = files.Length;
                            var fileNumber = 0;
                            foreach (var file in files)
                            {
                                var pattern = TCDEncodeDecode.DecryptString(file, this.password);

                                var parts = pattern.Split('_');
                                var textureName = string.Join("_", parts.Take(parts.Length - 1));
                                var size = int.Parse(parts.Last());

                                var rawBuffer = rsr.ReadBytes(size);

                                var buffer = rawBuffer.Select((b, i) => i > 1024 ? (byte)b : (byte)(~b & 0xff)).ToArray();
                                this.textureBytes[textureName] = buffer;

                                fileNumber += 1;

                                this.UpdateProgress(textureName, (double)fileNumber / fileCount);
                            }
                        }
                    }
                }
                catch (CryptographicException)
                {
                    MessageBox.Show("Resource file decryption error. Check the password in the Settings.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    this.UpdateProgressComplete();
                }
            });

            unpackThread.Start();
        }

        public Texture ProvideTexture(string textureName, TextureUnit textureUnit, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {
            var textureKey = string.Format(this.graphicsFormat, textureName);
            if (textureBytes.ContainsKey(textureKey))
            {
                // Intermediate bitmap used since passing bytes failed to load textures
                var bitmap = new Bitmap(new MemoryStream(textureBytes[textureKey]));
                return new Texture(bitmap, textureUnit, minFilter, magFilter);
            }

            return Texture.NotLoaded;
        }

        public IEnumerable<string> GetProvidableTextures()
        {
            var files = this.textureBytes.Keys;

            var formatInverterFunc = this.graphicsFormat.CreateFormatInverter(files);

            return files.Select(formatInverterFunc).ToArray();
        }

        public bool CanProvideTexture(string textureName)
        {
            return !this.texturesLoaded || this.textureBytes.ContainsKey(string.Format(this.graphicsFormat, textureName));
        }

        private void UpdateProgress(string label, double progress)
        {
            this.ProgressUpdated?.Invoke(this, new TextureLoadProgressUpdatedEventArgs(label, progress));
        }

        private void UpdateProgressComplete()
        {
            this.ProgressUpdated?.Invoke(this, null);
            this.texturesLoaded = true;
        }
    }
}
