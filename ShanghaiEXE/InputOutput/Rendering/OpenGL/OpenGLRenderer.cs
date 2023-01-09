using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Common.EncodeDecode;
using Common.Vectors;
using NSGame;
using Common.OpenGL;
using Text = Common.OpenGL.Text;

namespace NSShanghaiEXE.InputOutput.Rendering.OpenGL
{
    public class OpenGLRenderer : IRenderer
    {
        private static readonly Color DefaultTextColor = Color.FromArgb(byte.MaxValue, 64, 56, 56);
        private static readonly Color TextShadowColor = Color.FromArgb(byte.MaxValue, 32, 32, 32);
        private static readonly string FallbackFont = "MICROSS.TTF";
        public SpriteRendererPanel rend => this.renderer;
        private readonly SpriteRendererPanel renderer;
        private readonly Font regularFont;
        private readonly Font miniFont;
        private readonly Font microFont;

        private readonly PrivateFontCollection customFontInstance;
        private readonly ITextMeasurer measurer;

        private bool lastWasText;
        private int currentRenderLevel;

        public event EventHandler<TextureLoadProgressUpdatedEventArgs> ProgressUpdated;

        public OpenGLRenderer(string tcdFile, string password, string graphicsFormat, double initialScale) : this(tcdFile, password, graphicsFormat, initialScale, initialScale)
        {
        }

        public OpenGLRenderer(string tcdFile, string password, string graphicsFormat, double initialScaleX, double initialScaleY)
        {
            var loadStrategy = new FolderOverrideLoadStrategy(tcdFile, password, graphicsFormat, $"language/{ShanghaiEXE.Config.Language}/{{0}}.png");
            //var loadStrategy = new FolderTextureLoadStrategy("Graphics/{0}.png");
            loadStrategy.ProgressUpdated += this.Load_ProgressUpdate;
            loadStrategy.Load();
            this.renderer = new SpriteRendererPanel(loadStrategy, initialScaleX, initialScaleY);
            this.lastWasText = false;

            var usedFont = default(Font);
            var usedMiniFont = default(Font);
            var usedMicroFont = default(Font);

            var fontOverride = ShanghaiEXE.languageTranslationService.GetFontOverride();
            if (fontOverride != null)
            {
                this.customFontInstance = new PrivateFontCollection();
                var fontOverridePath = $"language/{ShanghaiEXE.Config.Language}/{fontOverride}.ttf";
                this.customFontInstance.AddFontFile(fontOverridePath);
                usedFont = new Font(this.customFontInstance.Families[0], 15, FontStyle.Regular);
                usedMiniFont = new Font(this.customFontInstance.Families[0], 12, FontStyle.Regular);
                usedMicroFont = new Font(this.customFontInstance.Families[0], 11, FontStyle.Regular);
                FontGlyphs.FontOverride = fontOverridePath;
            }
            else if ((new InstalledFontCollection().Families).Any(f => f.Name == "Microsoft Sans Serif"))
            {
                usedFont = new Font("Microsoft Sans Serif", 15f, FontStyle.Regular);
                usedMiniFont = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular);
                usedMicroFont = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular);
            }
            else
            {
                this.customFontInstance = new PrivateFontCollection();
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(OpenGLRenderer.FallbackFont, StringComparison.InvariantCultureIgnoreCase));
                byte[] fontBytes;
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    fontBytes = new byte[stream.Length];
                    stream.Read(fontBytes, 0, fontBytes.Length);
                    stream.Close();
                }
                IntPtr handle = Marshal.AllocCoTaskMem(fontBytes.Length);
                Marshal.Copy(fontBytes, 0, handle, fontBytes.Length);
                this.customFontInstance.AddMemoryFont(handle, fontBytes.Length);
                Marshal.FreeCoTaskMem(handle);
                usedFont = new Font(this.customFontInstance.Families[0], 15, FontStyle.Regular);
                usedMiniFont = new Font(this.customFontInstance.Families[0], 12, FontStyle.Regular);
                usedMicroFont = new Font(this.customFontInstance.Families[0], 11, FontStyle.Regular);
            }

            this.regularFont = usedFont;
            this.miniFont = usedMiniFont;
            this.microFont = usedMicroFont;

            this.measurer = new GLTextMeasurer(usedFont, usedMiniFont, usedMicroFont);
        }

        public SpriteRendererPanel GetPanel() => this.renderer;

        public void AbortRenderThread()
        {
        }

        public void Begin(Color color)
        {
            this.currentRenderLevel = 0;
            this.lastWasText = false;
            this.renderer.SetClearColor(color);
        }

        public void DrawImage(IRenderer device, string texture, Rectangle spriteRect, bool leftpoint, Vector2 position, float scale, float rotation, bool reversed, Color color)
        {
            if (this.lastWasText)
            {
                this.currentRenderLevel++;
                this.lastWasText = false;
            }

            var drawnSprite = new Sprite
            {
                Texture = texture,
                TexX = spriteRect.Left,
                TexY = spriteRect.Top,
                X = position.X,
                Y = position.Y,
                Width = spriteRect.Width,
                Height = spriteRect.Height,
                Scale = new Vector2(reversed ? -scale : scale, scale),
                Rotate = rotation * (float)Math.PI / 180.0f,
                ColorModulation = color
            };
            if (leftpoint)
            {
                drawnSprite = drawnSprite.WithTopLeftPosition();
            }
            drawnSprite.X += (drawnSprite.Width % 2) / 2;
            drawnSprite.Y += (drawnSprite.Height % 2) / 2;

            this.renderer.Draw(drawnSprite, this.currentRenderLevel);
        }

        public void DrawImage(IRenderer device, string texture, Rectangle spriteRect, bool leftpoint, Vector2 position, float scale, float rotation, Color color)
            => this.DrawImage(device, texture, spriteRect, leftpoint, position, scale, rotation, false, color);

        public void DrawImage(IRenderer device, string texture, Rectangle spriteRect, bool leftpoint, Vector2 position, bool reversed, Color color)
            => this.DrawImage(device, texture, spriteRect, leftpoint, position, 1, 0, reversed, color);

        public void DrawImage(IRenderer device, string texture, Rectangle spriteRect, bool leftpoint, Vector2 position, Color color)
            => this.DrawImage(device, texture, spriteRect, leftpoint, position, 1, 0, false, color);

        public void DrawMicroText(string text, Vector2 position, Color color)
        {
            if (!this.lastWasText)
            {
                this.currentRenderLevel++;
                this.lastWasText = true;
            }

            var drawnText = new Text
            {
                Content = text,
                Position = new Point((int)position.X, (int)position.Y),
                Font = this.microFont,
                Color = color
            };

            this.renderer.DrawText(drawnText, this.currentRenderLevel);
        }

        public void DrawMiniText(string text, Vector2 position, Color color)
        {
            if (!this.lastWasText)
            {
                this.currentRenderLevel++;
                this.lastWasText = true;
            }

            var drawnText = new Text
            {
                Content = text,
                Position = new Point((int)position.X, (int)position.Y - 1),
                Font = this.miniFont,
                Color = color
            };

            this.renderer.DrawText(drawnText, this.currentRenderLevel);
        }

        public void DrawMiniText(string text, Vector2 position, Color color, SaveData save)
        {
            var parsedText = this.ControlCharacterChange(text, save);
            this.DrawMiniText(parsedText, position, color);
        }

        public void DrawText(string text, Vector2 position)
        {
            this.DrawText(text, position, OpenGLRenderer.DefaultTextColor);
        }

        public void DrawText(string text, Vector2 position, bool shadow)
        {
            if (!this.lastWasText)
            {
                this.currentRenderLevel++;
                this.lastWasText = true;
            }

            if (shadow)
            {
                var shadowText = new Text
                {
                    Content = text,
                    Position = new Point((int)position.X + 1, (int)position.Y + 1 - 2),
                    Font = this.regularFont,
                    Color = OpenGLRenderer.TextShadowColor
                };
                this.renderer.DrawText(shadowText, this.currentRenderLevel);
            }

            var foregroundText = new Text
            {
                Content = text,
                Position = new Point((int)position.X, (int)position.Y - 2),
                Font = this.regularFont,
                Color = Color.White
            };
            this.renderer.DrawText(foregroundText, this.currentRenderLevel);
        }

        public void DrawText(string text, Vector2 position, bool shadow, SaveData save)
        {
            var parsedText = this.ControlCharacterChange(text, save);
            this.DrawText(parsedText, position, shadow);
        }

        public void DrawText(string text, Vector2 position, Color color)
        {
            if (!this.lastWasText)
            {
                this.currentRenderLevel++;
                this.lastWasText = true;
            }

            var drawnText = new Text
            {
                Content = text,
                Position = new Point((int)position.X - 1, (int)position.Y - 2),
                Font = this.regularFont,
                Color = color
            };
            this.renderer.DrawText(drawnText, this.currentRenderLevel);
        }

        public void DrawText(string text, Vector2 position, Color color, SaveData save)
        {
            var parsedText = this.ControlCharacterChange(text, save);
            this.DrawText(parsedText, position, color);
        }

        public void DrawText(string text, Vector2 position, SaveData save)
        {
            var parsedText = this.ControlCharacterChange(text, save);
            this.DrawText(parsedText, new Vector2(position.X + 1, position.Y));
        }

        public void End()
        {
            this.renderer.Render();
        }

        public void LoadTexture(string texture)
        {
        }

        public ITextMeasurer GetTextMeasurer() => this.measurer;

        private string ControlCharacterChange(string text, SaveData save)
        {
            string[] tokens = text.Split('#');
            var controlCharacters = new[] { "v", "s", "w", "b", "e", "i", "u", "c", "p" };
            string parsedString = string.Empty;
            for (int i = 0; i < tokens.Length; ++i)
            {
                for (int ii = 0; ii < controlCharacters.Length; ++ii)
                {
                    var currControlCharacter = controlCharacters[ii];
                    if (tokens[i].Contains(currControlCharacter + "@"))
                    {
                        string[] controlArguments = tokens[i].Split('@');
                        string str4 = currControlCharacter;
                        switch (currControlCharacter)
                        {
                            case "v":
                                tokens[i] = save.ValList[int.Parse(controlArguments[1])].ToString();
                                break;
                            case "i":
                                tokens[i] = save.item;
                                break;
                            case "c":
                                tokens[i] = save.category;
                                break;
                            default:
                                tokens[i] = string.Empty;
                                break;
                        }
                    }
                }
                parsedString += tokens[i];
            }
            text = parsedString;
            return text;
        }

        private void Load_ProgressUpdate(object sender, LoadProgressUpdatedEventArgs e)
        {
            if (e == null)
            {
                this.ProgressUpdated?.Invoke(this, null);
                ((FolderOverrideLoadStrategy)sender).ProgressUpdated -= this.Load_ProgressUpdate;
            }
            else
            {
                this.ProgressUpdated?.Invoke(this, new TextureLoadProgressUpdatedEventArgs(e.UpdateLabel, e.UpdateProgress));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OpenGLRenderer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
