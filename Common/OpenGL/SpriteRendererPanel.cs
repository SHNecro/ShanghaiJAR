using Common.ExtensionMethods;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace Common.OpenGL
{
    public class SpriteRendererPanel : GLControl, ISpriteRenderer, IMouseInteractionControl
    {
        public static readonly Color DefaultBackgroundColor = Color.FromArgb(64, 96, 96);

        public static event Action TextureLoad;
        public static event Action TexturesReloaded;
        public event Action<OriginChangedEventArgs> OriginChanged;

        public event MouseEventHandler ScaledMouseDown;
        public event MouseEventHandler ScaledMouseMove;
        public event MouseEventHandler ScaledMouseUp;
        public event EventHandler ScaledMouseEnter;
        public event EventHandler ScaledMouseHover;
        public event EventHandler ScaledMouseLeave;

        // Max size 64, set by array size in shader
        private const int DrawBatchSize = 64;
        // If >16, will need updates to shader.frag, extension methods.
        // Last unit used for loading textures
        private const int UsedTextureUnits = 16;
        // At limit, remove oldest
        private const int MaxCachedTextTextures = 256;

        private static readonly object RenderLock = new object();

        private static readonly float[] Vertices =
        {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, //Top-right vertex
             0.5f, -0.5f, 1.0f, 1.0f, 0.0f, //Bottom-right vertex
            -0.5f, -0.5f, 2.0f, 0.0f, 0.0f, //Bottom-left vertex
            -0.5f,  0.5f, 3.0f, 0.0f, 1.0f,  //Top-left vertex
        };

        private static readonly int[] Indices = {
            0, 1, 2 ,
            2, 3, 0
        };

        private static Dictionary<Tuple<TextureType, string>, TextureUnit> TextureUnits;

        private static Dictionary<string, Texture> SpriteSheets;
        private static Dictionary<string, Texture> TextTextures;

        private static Shader Shader;

        private static int vertexBufferObject;
        private static int vertexArrayObject;
        private static int elementBufferObject;

		private static Dictionary<Color, Vector4> CachedColorModulation;

        private readonly Dictionary<int, RenderedObjectCollection> renderedObjects;
        private ITextureLoadStrategy textureLoadStrategy;
        private Tile backgroundTile;
        private Color backgroundColor;
        private Matrix4 projection;
        private Vector2 origin;
        private double renderScaleX;
        private double renderScaleY;
        private bool viewportUpdate;
        private double? initialScaleX;
        private double? initialScaleY;

        static SpriteRendererPanel()
        {
            SpriteRendererPanel.TextureUnits = new Dictionary<Tuple<TextureType, string>, TextureUnit>();
            SpriteRendererPanel.SpriteSheets = new Dictionary<string, Texture>();
            SpriteRendererPanel.TextTextures = new Dictionary<string, Texture>();

			SpriteRendererPanel.CachedColorModulation = new Dictionary<Color, Vector4>();
        }

        public SpriteRendererPanel(ITextureLoadStrategy textureLoadStrategy) : this(textureLoadStrategy, null)
        {
            this.backgroundColor = SpriteRendererPanel.DefaultBackgroundColor;
        }

        public SpriteRendererPanel(ITextureLoadStrategy textureLoadStrategy, double? initialScale) : this(textureLoadStrategy, initialScale, initialScale)
        {
        }

        public SpriteRendererPanel(ITextureLoadStrategy textureLoadStrategy, double? initialScaleX, double? initialScaleY)
        {
            this.textureLoadStrategy = textureLoadStrategy;

            this.initialScaleX = initialScaleX;
            this.initialScaleY = initialScaleY;
            this.Load += this.GLControlOnLoad;
            this.Resize += this.GLControlOnResize;

            this.renderedObjects = new Dictionary<int, RenderedObjectCollection>();

            this.renderScaleX = 1;
            this.renderScaleY = 1;

            this.MouseDown += SpriteRendererPanel_MouseDown;
            this.MouseMove += SpriteRendererPanel_MouseMove;
            this.MouseUp += SpriteRendererPanel_MouseUp;
            this.MouseEnter += SpriteRendererPanel_MouseEnter;
            this.MouseHover += SpriteRendererPanel_MouseHover;
            this.MouseLeave += SpriteRendererPanel_MouseLeave;
        }

        public Point Origin
        {
            get
            {
                return new Point((int)this.origin.X, (int)this.origin.Y);
            }
            set
            {
                var previousOrigin = this.Origin;
                this.origin = new Vector2(value.X, value.Y);
                this.OriginChanged?.Invoke(new OriginChangedEventArgs { PreviousOrigin = previousOrigin, NewOrigin = value });
            }
        }

        public double RenderScale
        {
            get
            {
                return (Math.Abs(this.RenderScaleX - this.RenderScaleY) < double.Epsilon * 8) ? this.RenderScaleX : double.NaN;
            }
            set
            {
                this.RenderScaleX = value;
                this.RenderScaleY = value;
            }
        }

        public double RenderScaleX
        {
            get
            {
                return this.renderScaleX;
            }
            set
            {
                var baseSize = new Size((int)(this.Size.Width / this.renderScaleX), (int)(this.Size.Height / this.renderScaleY));
                this.renderScaleX = value;
                this.SetSize(baseSize);
            }
        }

        public double RenderScaleY
        {
            get
            {
                return this.renderScaleY;
            }
            set
            {
                var baseSize = new Size((int)(this.Size.Width / this.renderScaleX), (int)(this.Size.Height / this.renderScaleY));
                this.renderScaleY = value;
                this.SetSize(baseSize);
            }
        }

        public ITextureLoadStrategy TextureLoadStrategy
        {
            get { return this.textureLoadStrategy; }
            set { this.textureLoadStrategy = value; }
        }

        public void Draw(Sprite sprite, int renderPass)
        {
            if (!this.renderedObjects.ContainsKey(renderPass))
            {
                this.renderedObjects[renderPass] = new RenderedObjectCollection();
            }

            this.renderedObjects[renderPass].Sprites.Add(sprite);
        }

        public void DrawLevel(int x, int y, string texture, int renderPass)
        {
            if (!this.renderedObjects.ContainsKey(renderPass))
            {
                this.renderedObjects[renderPass] = new RenderedObjectCollection();
            }

            this.renderedObjects[renderPass].Levels.Add(new Sprite { X = x, Y = y, Texture = texture });
        }

        public void DrawTiledBackground(Tile backgroundTile)
        {
            this.backgroundTile = backgroundTile;
        }

        public void DrawQuad(Quad quad, int renderPass)
        {
            if (!this.renderedObjects.ContainsKey(renderPass))
            {
                this.renderedObjects[renderPass] = new RenderedObjectCollection();
            }

            this.renderedObjects[renderPass].Quads.Add(quad);
        }

        public void DrawText(Text t, int renderPass)
        {
            if (!this.renderedObjects.ContainsKey(renderPass))
            {
                this.renderedObjects[renderPass] = new RenderedObjectCollection();
            }

            this.renderedObjects[renderPass].Texts.Add(t);
        }

        public Size? GetTextureSize(string texture)
        {
            if (SpriteRendererPanel.SpriteSheets.TryGetValue(texture, out Texture tex))
            {
                return tex.Size;
            }

            return null;
        }

        public void Render()
        {
            if (this.initialScaleX.HasValue)
            {
                this.RenderScaleX = this.initialScaleX.Value;
                this.initialScaleX = null;
            }

            if (this.initialScaleY.HasValue)
            {
                this.RenderScaleY = this.initialScaleY.Value;
                this.initialScaleY = null;
            }

            lock (SpriteRendererPanel.RenderLock)
            {
                this.MakeCurrent();

                if (this.viewportUpdate)
                {
                    GL.Viewport(0, 0, this.Width, this.Height);
                    this.viewportUpdate = false;
                }

                if (this.backgroundTile == null)
                {
                    GL.ClearColor(this.backgroundColor);
                }

                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.BindVertexArray(vertexArrayObject);
                SpriteRendererPanel.Shader.Use();

                GL.BindVertexArray(vertexArrayObject);
                SpriteRendererPanel.Shader.SetVector2("origin", this.origin);
                SpriteRendererPanel.Shader.SetMatrix4("projection", this.projection);
                
                SpriteRendererPanel.Shader.SetInt($"drawType", 2);
                if (this.backgroundTile != null)
                {
                    var textureName = this.backgroundTile.Texture;
                    var textureIndex = this.GetOrSetTextureUnit(textureName);
                    if (textureIndex != -1)
                    {
                        var textureSize = SpriteRendererPanel.SpriteSheets[textureName].Size;
                        var bgTiles = new Size(2 + this.Width / this.backgroundTile.TileWidth, 2 + this.Height / textureSize.Height);
                        var texX = this.backgroundTile.TextureFrame * this.backgroundTile.TileWidth;
                        SpriteRendererPanel.Shader.SetFloat($"spriteX[0]", this.backgroundTile.OffsetX - this.backgroundTile.TileWidth / 2);
                        SpriteRendererPanel.Shader.SetFloat($"spriteY[0]", this.backgroundTile.OffsetY - textureSize.Height / 2);
                        SpriteRendererPanel.Shader.SetInt($"spriteW[0]", this.backgroundTile.TileWidth);
                        SpriteRendererPanel.Shader.SetInt($"spriteW[1]", bgTiles.Width);
                        SpriteRendererPanel.Shader.SetInt($"spriteH[0]", textureSize.Height);
                        SpriteRendererPanel.Shader.SetInt($"spriteH[1]", bgTiles.Height);
                        SpriteRendererPanel.Shader.SetInt($"spriteTX[0]", texX);
                        SpriteRendererPanel.Shader.SetInt($"spriteTY[0]", 0);
                        SpriteRendererPanel.Shader.SetInt($"spriteTI[0]", textureIndex);
                        SpriteRendererPanel.Shader.SetVector2($"spriteScale[0]", Vector2.One);
                        SpriteRendererPanel.Shader.SetFloat($"spriteRotate[0]", 0);
                        SpriteRendererPanel.Shader.SetVector4($"colorModulation[0]", Vector4.One);
                        GL.DrawElementsInstanced(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, bgTiles.Width * bgTiles.Height);
                    }
                }

                foreach (var kvp in this.renderedObjects.OrderBy(kvp => kvp.Key))
                {
                    var renderPass = kvp.Key;
                    var levels = kvp.Value.Levels;
                    var sprites = kvp.Value.Sprites;
                    var texts = kvp.Value.Texts;
                    var fillQuads = kvp.Value.Quads.Where(q => q.Type.HasFlag(DrawType.Fill)).ToList();
                    var outlineQuads = kvp.Value.Quads.Where(q => q.Type.HasFlag(DrawType.Outline)).ToList();
                    var batchSize = 0;

                    SpriteRendererPanel.Shader.SetInt($"drawType", 1);
                    for (int i = 0; i < levels.Count; i++)
                    {
                        var textureName = levels[i].Texture;
                        var textureIndex = this.GetOrSetTextureUnit(textureName);
                        var textureSize = textureIndex != -1 ? SpriteRendererPanel.SpriteSheets[textureName].Size : new Size(2000, 2000);
                        if (textureIndex == -1)
                        {
                            continue;
                        }

                        // Notable speed improvement over multiple $"[{batchSize}]"
                        var batchIndex = "[" + batchSize + "]";
                        SpriteRendererPanel.Shader.SetFloat("spriteX" + batchIndex, levels[i].X + textureSize.Width / 2);
                        SpriteRendererPanel.Shader.SetFloat("spriteY" + batchIndex, levels[i].Y + textureSize.Height / 2);
                        SpriteRendererPanel.Shader.SetInt("spriteW" + batchIndex, textureSize.Width);
                        SpriteRendererPanel.Shader.SetInt("spriteH" + batchIndex, textureSize.Height);
                        SpriteRendererPanel.Shader.SetInt("spriteTX" + batchIndex, 0);
                        SpriteRendererPanel.Shader.SetInt("spriteTY" + batchIndex, 0);
                        SpriteRendererPanel.Shader.SetInt("spriteTI" + batchIndex, textureIndex);
                        SpriteRendererPanel.Shader.SetVector2("spriteScale" + batchIndex, Vector2.One);
                        SpriteRendererPanel.Shader.SetFloat("spriteRotate" + batchIndex, 0);
                        SpriteRendererPanel.Shader.SetVector4("colorModulation" + batchIndex, Vector4.One);
                        batchSize++;
                        if (batchSize == SpriteRendererPanel.DrawBatchSize ||
                            i + 1 == levels.Count ||
                            (SpriteRendererPanel.TextureUnits.Count == SpriteRendererPanel.UsedTextureUnits - 1 && !SpriteRendererPanel.TextureUnits.ContainsKey(Tuple.Create(TextureType.Spritesheet, levels[i + 1].Texture))))
                        {
                            GL.DrawElementsInstanced(PrimitiveType.Triangles, SpriteRendererPanel.Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, batchSize);
                            batchSize = 0;
                        }
                    }

                    SpriteRendererPanel.Shader.SetInt($"drawType", 4);
                    for (int i = 0; i < fillQuads.Count; i++)
                    {
                        var batchIndex = "[" + batchSize + "]";
                        SpriteRendererPanel.Shader.SetFloat($"spriteX" + batchIndex, fillQuads[i].A.X);
                        SpriteRendererPanel.Shader.SetFloat($"spriteY" + batchIndex, fillQuads[i].A.Y);
                        SpriteRendererPanel.Shader.SetInt($"spriteW" + batchIndex, fillQuads[i].B.X);
                        SpriteRendererPanel.Shader.SetInt($"spriteH" + batchIndex, fillQuads[i].B.Y);
                        SpriteRendererPanel.Shader.SetInt($"spriteTX" + batchIndex, fillQuads[i].C.X);
                        SpriteRendererPanel.Shader.SetInt($"spriteTY" + batchIndex, fillQuads[i].C.Y);
                        SpriteRendererPanel.Shader.SetInt($"spriteTI" + batchIndex, fillQuads[i].Color.ToArgb());
                        SpriteRendererPanel.Shader.SetVector2($"spriteScale" + batchIndex, new Vector2(fillQuads[i].D.X, fillQuads[i].D.Y));
                        SpriteRendererPanel.Shader.SetVector4($"colorModulation" + batchIndex, Vector4.One);
                        batchSize++;
                        if (batchSize == SpriteRendererPanel.DrawBatchSize || i + 1 == fillQuads.Count)
                        {
                            GL.DrawElementsInstanced(PrimitiveType.Triangles, SpriteRendererPanel.Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, batchSize);
                            batchSize = 0;
                        }
                    }

                    SpriteRendererPanel.Shader.SetInt($"drawType", 4);
                    for (int i = 0; i < outlineQuads.Count; i++)
                    {
                        var batchIndex = "[" + batchSize + "]";
                        SpriteRendererPanel.Shader.SetFloat($"spriteX" + batchIndex, outlineQuads[i].A.X);
                        SpriteRendererPanel.Shader.SetFloat($"spriteY" + batchIndex, outlineQuads[i].A.Y);
                        SpriteRendererPanel.Shader.SetInt($"spriteW" + batchIndex, outlineQuads[i].B.X);
                        SpriteRendererPanel.Shader.SetInt($"spriteH" + batchIndex, outlineQuads[i].B.Y);
                        SpriteRendererPanel.Shader.SetInt($"spriteTX" + batchIndex, outlineQuads[i].C.X);
                        SpriteRendererPanel.Shader.SetInt($"spriteTY" + batchIndex, outlineQuads[i].C.Y);
                        SpriteRendererPanel.Shader.SetInt($"spriteTI" + batchIndex, outlineQuads[i].Color.ToArgb());
                        SpriteRendererPanel.Shader.SetVector2($"spriteScale" + batchIndex, new Vector2(outlineQuads[i].D.X, outlineQuads[i].D.Y));
                        SpriteRendererPanel.Shader.SetVector4($"colorModulation" + batchIndex, Vector4.One);
                        batchSize++;
                        if (batchSize == SpriteRendererPanel.DrawBatchSize || i + 1 == outlineQuads.Count)
                        {
                            GL.DrawElementsInstanced(PrimitiveType.LineStrip, SpriteRendererPanel.Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, batchSize);
                            batchSize = 0;
                        }
                    }

                    SpriteRendererPanel.Shader.SetInt($"drawType", 0);
                    for (int i = 0; i < sprites.Count; i++)
                    {
                        var textureName = sprites[i].Texture;
                        var textureIndex = this.GetOrSetTextureUnit(textureName);
						if (textureIndex == -1)
                        {
                            continue;
                        }

						// Manually clips texture since CLAMP_TO_EDGE didn't do anything on one test computer
						var textureRect = new Rectangle(new Point(), this.GetTextureSize(textureName) ?? new Size(2000, 2000));
						var spriteRect = new Rectangle(sprites[i].TexX, sprites[i].TexY, sprites[i].Width, sprites[i].Height);
						if (!textureRect.Contains(spriteRect))
						{
							spriteRect.Intersect(textureRect);
							sprites[i].TexX = spriteRect.Left;
							sprites[i].TexY = spriteRect.Top;
							sprites[i].Width = spriteRect.Width;
							sprites[i].Height = spriteRect.Height;
						}

						var spriteColorModulation = default(Vector4);
						if (!SpriteRendererPanel.CachedColorModulation.TryGetValue(sprites[i].ColorModulation, out spriteColorModulation))
						{
							spriteColorModulation = new Vector4(
								sprites[i].ColorModulation.R / 255.0f,
								sprites[i].ColorModulation.G / 255.0f,
								sprites[i].ColorModulation.B / 255.0f,
								sprites[i].ColorModulation.A / 255.0f);
							SpriteRendererPanel.CachedColorModulation[sprites[i].ColorModulation] = spriteColorModulation;
						}

						var batchIndex = "[" + batchSize + "]";
                        SpriteRendererPanel.Shader.SetFloat($"spriteX" + batchIndex, sprites[i].X);
                        SpriteRendererPanel.Shader.SetFloat($"spriteY" + batchIndex, sprites[i].Y);
                        SpriteRendererPanel.Shader.SetInt($"spriteW" + batchIndex, sprites[i].Width);
                        SpriteRendererPanel.Shader.SetInt($"spriteH" + batchIndex, sprites[i].Height);
                        SpriteRendererPanel.Shader.SetInt($"spriteTX" + batchIndex, sprites[i].TexX);
                        SpriteRendererPanel.Shader.SetInt($"spriteTY" + batchIndex, sprites[i].TexY);
                        SpriteRendererPanel.Shader.SetInt($"spriteTI" + batchIndex, textureIndex);
                        var tkScaleVector = new Vector2(sprites[i].Scale.X, sprites[i].Scale.Y);
                        SpriteRendererPanel.Shader.SetVector2($"spriteScale" + batchIndex, tkScaleVector);
                        SpriteRendererPanel.Shader.SetFloat($"spriteRotate" + batchIndex, sprites[i].Rotate);
                        SpriteRendererPanel.Shader.SetVector4($"colorModulation" + batchIndex, spriteColorModulation);
                        batchSize++;
                        if (batchSize == SpriteRendererPanel.DrawBatchSize ||
                            i + 1 == sprites.Count ||
                            (SpriteRendererPanel.TextureUnits.Count == SpriteRendererPanel.UsedTextureUnits - 1 && !SpriteRendererPanel.TextureUnits.ContainsKey(Tuple.Create(TextureType.Spritesheet, sprites[i + 1].Texture))))
                        {
                            GL.DrawElementsInstanced(PrimitiveType.Triangles, SpriteRendererPanel.Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, batchSize);
                            batchSize = 0;
                        }
                    }

                    SpriteRendererPanel.Shader.SetInt($"drawType", 0);
                    foreach (var text in texts)
                    {
						var offset = 0.0;
                        for (var i = 0; i < text.Content.Length; i++)
                        {
                            var textureIndex = SpriteRendererPanel.GetOrSetTextureUnit(text, i);
                            if (textureIndex == -1 || !SpriteRendererPanel.TextTextures.ContainsKey(text.GetCharKey(i)))
                            {
                                continue;
                            }

                            var texture = SpriteRendererPanel.TextTextures[text.GetCharKey(i)];
							var metrics = FontGlyphs.TextMetrics[text.GetCharKey(i)];

							var face = FontGlyphs.TextFaces[text.Font];
                            var faceHeight = FontGlyphs.TextFaceHeights[text.Font];

							var adjustedPosition = new PointF(
                                text.Position.X + (float)(offset + metrics.HorizontalBearingX + (metrics.Width / 2.0)),
                                text.Position.Y + (float)(faceHeight - metrics.HorizontalBearingY + (metrics.Height / 2.0)));

                            var advance = metrics.HorizontalAdvance;

							var textColorModulation = default(Vector4);
							if (!SpriteRendererPanel.CachedColorModulation.TryGetValue(text.Color, out textColorModulation))
							{
								textColorModulation = new Vector4(text.Color.R / 255.0f, text.Color.G / 255.0f, text.Color.B / 255.0f, text.Color.A / 255.0f);
								SpriteRendererPanel.CachedColorModulation[text.Color] = textColorModulation;
							}
							SpriteRendererPanel.Shader.SetFloat($"spriteX[0]", adjustedPosition.X);
                            SpriteRendererPanel.Shader.SetFloat($"spriteY[0]", adjustedPosition.Y);
                            SpriteRendererPanel.Shader.SetInt($"spriteW[0]", texture.Width);
                            SpriteRendererPanel.Shader.SetInt($"spriteH[0]", texture.Height);
                            SpriteRendererPanel.Shader.SetInt($"spriteTX[0]", 0);
                            SpriteRendererPanel.Shader.SetInt($"spriteTY[0]", 0);
                            SpriteRendererPanel.Shader.SetInt($"spriteTI[0]", textureIndex);
                            SpriteRendererPanel.Shader.SetVector2($"spriteScale[0]", Vector2.One);
                            SpriteRendererPanel.Shader.SetFloat($"spriteRotate[0]", 0);
                            SpriteRendererPanel.Shader.SetVector4($"colorModulation[0]", textColorModulation);

                            GL.DrawElementsInstanced(PrimitiveType.Triangles, SpriteRendererPanel.Indices.Length, DrawElementsType.UnsignedInt, (IntPtr)0, 1);

                            offset += advance;
                        }
                    }
                    
                }
                this.backgroundTile = null;
                this.renderedObjects.Clear();

			    this.SwapBuffers();
                
                // TODO: Figure out what messes with textureunits between renders
                SpriteRendererPanel.TextureUnits.Clear();
            }
        }

        private int GetOrSetTextureUnit(string textureName)
        {
            if (SpriteRendererPanel.TextureUnits.ContainsKey(Tuple.Create(TextureType.Spritesheet, textureName)))
            {
                return SpriteRendererPanel.TextureUnits[Tuple.Create(TextureType.Spritesheet, textureName)].ToInt();
            }
            else
            {
                // If at 16, will have already drawn batch
                if (SpriteRendererPanel.TextureUnits.Count >= SpriteRendererPanel.UsedTextureUnits - 1)
                {
                    SpriteRendererPanel.TextureUnits.Clear();
                }
                if (!string.IsNullOrEmpty(textureName) && (SpriteRendererPanel.SpriteSheets.ContainsKey(textureName) || this.LoadTexture(textureName)))
                {
                    var unitNumber = SpriteRendererPanel.TextureUnits.Count;
                    var usedUnit = unitNumber.ToTextureUnit();
                    var usedTexture = SpriteRendererPanel.SpriteSheets[textureName];
                    usedTexture.Use(usedUnit);
                    SpriteRendererPanel.Shader.SetInt($"textureArrays[{unitNumber}]", unitNumber);
                    SpriteRendererPanel.Shader.SetInt($"sheetW[{unitNumber}]", usedTexture.Width);
                    SpriteRendererPanel.Shader.SetInt($"sheetH[{unitNumber}]", usedTexture.Height);
                    SpriteRendererPanel.TextureUnits[Tuple.Create(TextureType.Spritesheet, textureName)] = usedUnit;
                    return unitNumber;
                }
                else
                {
                    return -1;
                }
            }
        }

        private static int GetOrSetTextureUnit(Text text, int index)
        {
            if (string.IsNullOrWhiteSpace(text?.Content))
            {
                return -1;
            }
            
            if (SpriteRendererPanel.TextureUnits.ContainsKey(Tuple.Create(TextureType.Text, text.GetCharKey(index))))
            {
                return SpriteRendererPanel.TextureUnits[Tuple.Create(TextureType.Text, text.GetCharKey(index))].ToInt();
            }
            else
            {
                // If at 16, will have already drawn batch
                if (SpriteRendererPanel.TextureUnits.Count >= SpriteRendererPanel.UsedTextureUnits - 1)
                {
                    SpriteRendererPanel.TextureUnits.Clear();
                }
                if (SpriteRendererPanel.TextTextures.ContainsKey(text.GetCharKey(index)) || SpriteRendererPanel.LoadTextTextures(text))
                {
                    var unitNumber = SpriteRendererPanel.TextureUnits.Count;
                    var usedUnit = unitNumber.ToTextureUnit();
                    var usedTexture = SpriteRendererPanel.TextTextures[text.GetCharKey(index)];
                    usedTexture.Use(usedUnit);
                    SpriteRendererPanel.Shader.SetInt($"textureArrays[{unitNumber}]", unitNumber);
                    SpriteRendererPanel.Shader.SetInt($"sheetW[{unitNumber}]", usedTexture.Width);
                    SpriteRendererPanel.Shader.SetInt($"sheetH[{unitNumber}]", usedTexture.Height);
                    SpriteRendererPanel.TextureUnits[Tuple.Create(TextureType.Text, text.GetCharKey(index))] = usedUnit;
                    return unitNumber;
                }
                else
                {
                    return -1;
                }
            }
        }

        public static void ReloadTextures()
        {
            foreach (var sheet in SpriteRendererPanel.SpriteSheets.Values)
            {
                sheet.Dispose();
            }
            foreach (var sheet in SpriteRendererPanel.TextTextures.Values)
            {
                sheet.Dispose();
            }

            SpriteRendererPanel.SpriteSheets.Clear();
            SpriteRendererPanel.TextTextures.Clear();
            SpriteRendererPanel.TextureUnits.Clear();

            SpriteRendererPanel.TexturesReloaded?.Invoke();
        }

        public Control GetControl()
        {
            return this;
        }

        public void SetSize(Size size)
        {
            var adjustedSize = new Size((int)(size.Width * this.renderScaleX), (int)(size.Height * this.renderScaleY));
            this.MinimumSize = adjustedSize;
            this.MaximumSize = adjustedSize;
        }

        public void SetClearColor(Color color)
        {
            this.backgroundColor = color;
        }

        private bool LoadTexture(string textureName)
        {
            if (SpriteRendererPanel.SpriteSheets.ContainsKey(textureName))
            {
                SpriteRendererPanel.SpriteSheets[textureName].Dispose();
            }

            var newTexture = this.TextureLoadStrategy.ProvideTexture(textureName, (SpriteRendererPanel.UsedTextureUnits - 1).ToTextureUnit());
            if (newTexture.Loaded)
            {
                SpriteRendererPanel.SpriteSheets[textureName] = newTexture;
            }

            return newTexture.Loaded;
        }

        private static bool LoadTextTextures(Text text)
        {
            var success = true;

            for (var i = 0; i < text.Content.Length; i++)
            {
                if (SpriteRendererPanel.TextTextures.ContainsKey(text.GetCharKey(i)))
                {
                    continue;
                }

				var glyphSlot = FontGlyphs.GetOrSetGlyphMetrics(text, i);

                var gdipBitmap = default(Bitmap);
                using (var glyph = glyphSlot.GetGlyph())
                {
                    var bitmapGlyph = glyph.ToBitmapGlyph();
                    var bitmap = bitmapGlyph.Bitmap;

                    if (bitmap.Rows != 0)
                    {
                        var metricRect = new Rectangle(
                            (bitmap.Width - glyphSlot.Metrics.Width.Round()) / 2,
                            (bitmap.Rows - glyphSlot.Metrics.Height.Round()) / 2,
                            glyphSlot.Metrics.Width.Round(),
                            glyphSlot.Metrics.Height.Round());
                        gdipBitmap = bitmap.ToGdipBitmap(Color.White).Crop(metricRect);
                        
                        // TODO: Fix real reason for malformed X
                        if (text.Content[i] == 'X' && Math.Abs(text.Font.SizeInPoints - 15) < double.Epsilon * 8)
                        {
                            var fixedXBitmap = new Bitmap(gdipBitmap, metricRect.Size);
                            for (int c = 0; c < metricRect.Width; c++)
                            {
                                fixedXBitmap.SetPixel(c, metricRect.Height - 1, fixedXBitmap.GetPixel(c, metricRect.Height - 2));
                            }
                            gdipBitmap = fixedXBitmap;
                        }
                    }
                    else
                    {
                        gdipBitmap = new Bitmap(1, 1);
                    }

                    var newTexture = new Texture(gdipBitmap, (SpriteRendererPanel.UsedTextureUnits - 1).ToTextureUnit());
                    if (newTexture.Loaded)
                    {
                        SpriteRendererPanel.TextTextures[text.GetCharKey(i)] = newTexture;
                    }

                    success &= newTexture.Loaded;
                }
            }

            return success;
        }

        private static void Initialize()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 0.0f);
            SpriteRendererPanel.vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, SpriteRendererPanel.vertexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, SpriteRendererPanel.Vertices.Length * sizeof(float), SpriteRendererPanel.Vertices, BufferUsageHint.StaticDraw);
            SpriteRendererPanel.elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, SpriteRendererPanel.elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, SpriteRendererPanel.Indices.Length * sizeof(float), SpriteRendererPanel.Indices, BufferUsageHint.StaticDraw);

            SpriteRendererPanel.Shader = new Shader("shader.vert", "shader.frag");
            SpriteRendererPanel.Shader.Use();

            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.AlphaFunc(AlphaFunction.Notequal, 0.0f);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.LineWidth(1);

            SpriteRendererPanel.vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(SpriteRendererPanel.vertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, SpriteRendererPanel.vertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, SpriteRendererPanel.elementBufferObject);

            int vertexLocation = SpriteRendererPanel.Shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

			int indexLocation = SpriteRendererPanel.Shader.GetAttribLocation("aIndex");
			GL.EnableVertexAttribArray(indexLocation);
			GL.VertexAttribPointer(indexLocation, 1, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));

			int texCoordLocation = SpriteRendererPanel.Shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            SpriteRendererPanel.TextureLoad?.Invoke();
        }

        private void GLControlOnLoad(object sender, EventArgs e)
        {
            SpriteRendererPanel.Initialize();
            this.projection = Matrix4.CreateScale(2.0f / this.Width, -2.0f / this.Height, 1);
        }

        private void GLControlOnResize(object sender, EventArgs e)
        {
            this.projection = Matrix4.CreateScale(2.0f / (float)(this.Width / (this.renderScaleX)), -2.0f / (float)(this.Height / (this.renderScaleY)), 1);
            this.viewportUpdate = true;
        }

        private void SpriteRendererPanel_MouseDown(object sender, MouseEventArgs e)
        {
            var scaledArgs = new MouseEventArgs(e.Button, e.Clicks, (int)(e.X / this.renderScaleX), (int)(e.Y / this.renderScaleY), e.Delta);
            this.ScaledMouseDown?.Invoke(sender, scaledArgs);
        }

        private void SpriteRendererPanel_MouseMove(object sender, MouseEventArgs e)
        {
            var scaledArgs = new MouseEventArgs(e.Button, e.Clicks, (int)(e.X / this.renderScaleX), (int)(e.Y / this.renderScaleY), e.Delta);
            this.ScaledMouseMove?.Invoke(sender, scaledArgs);
        }

        private void SpriteRendererPanel_MouseUp(object sender, MouseEventArgs e)
        {
            var scaledArgs = new MouseEventArgs(e.Button, e.Clicks, (int)(e.X / this.renderScaleX), (int)(e.Y / this.renderScaleY), e.Delta);
            this.ScaledMouseUp?.Invoke(sender, scaledArgs);
        }

        private void SpriteRendererPanel_MouseEnter(object sender, EventArgs e)
        {
            this.ScaledMouseEnter?.Invoke(sender, e);
        }

        private void SpriteRendererPanel_MouseHover(object sender, EventArgs e)
        {
            this.ScaledMouseHover?.Invoke(sender, e);
        }

        private void SpriteRendererPanel_MouseLeave(object sender, EventArgs e)
        {
            this.ScaledMouseLeave?.Invoke(sender, e);
        }

        #region IDisposable Support
        private bool disposedValue = false;
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Shader?.Dispose();
                }
                disposedValue = true;
            }
        }
        #endregion
    }
}
