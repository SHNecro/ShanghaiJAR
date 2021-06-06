using OpenTK.Graphics.OpenGL;
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
using Common.Vectors;
using Common.OpenGL;
using Common.OpenAL;

namespace Common.EncodeDecode
{
    public class TCDLoadStrategy : ITextureLoadStrategy, ISoundLoadStrategy
    {
        public static readonly string ResourceSuffix = "Resource.tcd";
        public static readonly string PatternSuffix = "Pattern.tcd";

        private readonly string selectedFile;
        private readonly string password;
        private readonly string fileFormat;
        private readonly Dictionary<string, byte[]> fileBytes;

        private bool filesLoaded;

        public event EventHandler<LoadProgressUpdatedEventArgs> ProgressUpdated;

        public TCDLoadStrategy(string tcdPatternOrResource, string password, string fileFormat)
        {
            this.selectedFile = tcdPatternOrResource;
            this.password = password;
            this.fileFormat = fileFormat;

            this.fileBytes = new Dictionary<string, byte[]>();
        }

        public void Load()
        {
            string resourceFile, patternFile, fileBase;
            resourceFile = patternFile = fileBase = string.Empty;
            if (!File.Exists(this.selectedFile))
            {
                throw new InvalidOperationException($"Missing file \"{this.selectedFile}\".");
            }
            else if (this.selectedFile.EndsWith(TCDLoadStrategy.ResourceSuffix, StringComparison.InvariantCultureIgnoreCase))
            {
                fileBase = Path.GetFileName(this.selectedFile.Substring(0, this.selectedFile.Length - TCDLoadStrategy.ResourceSuffix.Length));
                resourceFile = this.selectedFile;
                patternFile = Path.Combine(Path.GetDirectoryName(this.selectedFile), fileBase + TCDLoadStrategy.PatternSuffix);
                if (!File.Exists(patternFile))
                {
                    throw new InvalidOperationException($"Missing associated pattern file \"{patternFile}\".");
                }
            }
            else if (this.selectedFile.EndsWith(TCDLoadStrategy.PatternSuffix, StringComparison.InvariantCultureIgnoreCase))
            {
                fileBase = Path.GetFileName(this.selectedFile.Substring(0, this.selectedFile.Length - TCDLoadStrategy.PatternSuffix.Length));
                resourceFile = Path.Combine(Path.GetDirectoryName(this.selectedFile), fileBase + TCDLoadStrategy.ResourceSuffix);
                patternFile = this.selectedFile;
                if (!File.Exists(resourceFile))
                {
                    throw new InvalidOperationException($"Missing associated resource file \"{resourceFile}\".");
                }
            }
            else
            {
                throw new InvalidOperationException($"Invalid file \"{this.selectedFile}\".{Environment.NewLine}Expected a \"<name>Resource\" or \"<name>Pattern\" .tcd file.");
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
                                var fileName = string.Join("_", parts.Take(parts.Length - 1));
                                var size = int.Parse(parts.Last());

                                var rawBuffer = rsr.ReadBytes(size);

                                var buffer = rawBuffer.Select((b, i) => i > 1024 ? (byte)b : (byte)(~b & 0xff)).ToArray();
                                this.fileBytes[fileName] = buffer;

                                fileNumber += 1;

                                this.UpdateProgress(fileName, (double)fileNumber / fileCount);
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
            var textureKey = string.Format(this.fileFormat, textureName);
            if (this.fileBytes.ContainsKey(textureKey))
            {
                // Intermediate bitmap used since passing bytes failed to load textures
                var bitmap = new Bitmap(new MemoryStream(fileBytes[textureKey]));
                return new Texture(bitmap, textureUnit, minFilter, magFilter);
            }

            return Texture.NotLoaded;
        }

        public WAVData ProvideSound(string file)
        {
            using (var stream = new MemoryStream(this.fileBytes[string.Format(this.fileFormat, file)]))
            {
                return AudioEngine.LoadBytes(stream);
            }
        }

        public IEnumerable<string> GetProvidableFiles()
        {
            var files = this.fileBytes.Keys.ToArray();

            var formatInverterFunc = this.fileFormat.CreateFormatInverter(files);

            return files.Select(formatInverterFunc).ToArray();
        }

        public bool CanProvideFile(string fileName)
        {
            return !this.filesLoaded || this.fileBytes.ContainsKey(string.Format(this.fileFormat, fileName));
        }

        private void UpdateProgress(string label, double progress)
        {
            this.ProgressUpdated?.Invoke(this, new LoadProgressUpdatedEventArgs(label, progress));
        }

        private void UpdateProgressComplete()
        {
            this.ProgressUpdated?.Invoke(this, null);
            this.filesLoaded = true;
        }
    }
}
