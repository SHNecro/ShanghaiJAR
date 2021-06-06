using NSGame;
using Common.Vectors;
using SlimDX;
using SlimDX.Direct3D9;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Font = System.Drawing.Font;
using Vector2 = Common.Vectors.Vector2;

namespace NSShanghaiEXE.InputOutput.Rendering.DirectX9
{
    public class MySlimDG : IRenderer
    {
        public ShanghaiEXE form = null;
        public SlimFont font = null;
        private PresentParameters param = null;
        private readonly Direct3D direct3D;
        public Device device = null;
        public Sprite sprite = null;
        public bool RenderOK = true;
        public bool DisplayMode = false;
        public SlimFont minifont;
        public SlimFont microfont;
        private readonly Textures tex;
        public Thread thread_1;
        private static PrivateFontCollection customFontInstance;
        private ITextMeasurer measurer;

        public event EventHandler<TextureLoadProgressUpdatedEventArgs> ProgressUpdated;

        public MySlimDG(ShanghaiEXE wind)
        {
            this.direct3D = new Direct3D();
            this.form = wind;
            this.DisplayMode = false;
            this.SetParam();
            this.CreateDevice(wind, this.param);
            this.SetView();
            this.sprite = new Sprite(this.device);
            GraphicsClass.Direct3DDevice = this.device;
            GraphicsClass.Direct3DSprite = this.sprite;
            this.tex = new Textures(wind);
            this.tex.ProgressUpdated += this.TCDLoad_ProgressUpdate;
            this.thread_1 = new Thread(new ThreadStart(this.tex.Tex));
            this.thread_1.Start();

            var usedFont = default(Font);
            var usedMiniFont = default(Font);
            var usedMicroFont = default(Font);
            
            if ((new InstalledFontCollection().Families).Any(f => f.Name == "Microsoft Sans Serif"))
            {
                usedFont = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
                usedMiniFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular);
                usedMicroFont = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
            }
            else
            {
                MySlimDG.customFontInstance = new PrivateFontCollection();
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("JF-Dot-jiskan16s.ttf", StringComparison.InvariantCultureIgnoreCase));
                byte[] fontBytes;
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    fontBytes = new byte[stream.Length];
                    stream.Read(fontBytes, 0, fontBytes.Length);
                    stream.Close();
                }
                IntPtr handle = Marshal.AllocCoTaskMem(fontBytes.Length);
                Marshal.Copy(fontBytes, 0, handle, fontBytes.Length);
                MySlimDG.customFontInstance.AddMemoryFont(handle, fontBytes.Length);
                Marshal.FreeCoTaskMem(handle);
                usedFont = new Font(MySlimDG.customFontInstance.Families[0], 12, FontStyle.Regular);
                usedMiniFont = new Font(MySlimDG.customFontInstance.Families[0], 10, FontStyle.Regular);
                usedMicroFont = new Font(MySlimDG.customFontInstance.Families[0], 10, FontStyle.Regular);
            }

            this.font = new SlimFont(usedFont);
            this.minifont = new SlimFont(usedMiniFont);
            this.microfont = new SlimFont(usedMicroFont);

            this.measurer = new DGTextMeasurer(usedFont, usedMiniFont, usedMicroFont);
        }

        private void SetView()
        {
            this.device.SetRenderState(RenderState.AlphaBlendEnable, true);
            this.device.SetRenderState<Blend>(RenderState.SourceBlend, Blend.SourceAlpha);
            this.device.SetRenderState<Blend>(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            this.device.SetTextureStageState(0, TextureStage.AlphaArg1, TextureArgument.Texture);
            this.device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.Modulate);
            this.device.SetTextureStageState(0, TextureStage.AlphaArg2, TextureArgument.Diffuse);
            this.device.SetRenderState(RenderState.Lighting, true);
            this.device.SetLight(0, new Light()
            {
                Type = LightType.Directional,
                Diffuse = Color.White,
                Ambient = Color.GhostWhite,
                Direction = new SlimDX.Vector3(0.0f, -1f, 0.0f)
            });
            this.device.EnableLight(0, true);
            this.device.SetTransform(TransformState.Projection, Matrix.PerspectiveFovLH(0.7853982f, form.Width / (float)this.form.Height, 0.1f, 20f));
            this.device.SetTransform(TransformState.View, Matrix.LookAtLH(new SlimDX.Vector3(3f, 2f, -3f), SlimDX.Vector3.Zero, new SlimDX.Vector3(0.0f, 1f, 0.0f)));
            this.device.Material = new Material()
            {
                Diffuse = new Color4(Color.GhostWhite)
            };
        }

        private void CreateDevice(ShanghaiEXE wind, PresentParameters param)
        {
            try
            {
                this.device = new Device(this.direct3D, this.direct3D.Adapters.DefaultAdapter.Adapter, DeviceType.Hardware, wind.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters[1]
                {
                    param
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetParam()
        {
            this.param = new PresentParameters()
            {
                BackBufferFormat = Format.X8R8G8B8,
                BackBufferCount = 1,
                Multisample = MultisampleType.None,
                SwapEffect = SwapEffect.Discard,
                EnableAutoDepthStencil = true,
                AutoDepthStencilFormat = Format.D16,
                PresentFlags = PresentFlags.DiscardDepthStencil,
                PresentationInterval = PresentInterval.Default,
                Windowed = true,
                DeviceWindowHandle = this.form.Handle,
                BackBufferWidth = 240,
                BackBufferHeight = 160
            };
        }

        public void ChangeMode()
        {
            if (this.param.Windowed)
            {
                this.DisplayMode = true;
                this.param.Windowed = false;
                this.param.FullScreenRefreshRateInHertz = 60;
                this.ResetParam();
            }
            else
            {
                this.DisplayMode = false;
                this.param.Windowed = true;
                this.param.FullScreenRefreshRateInHertz = 0;
                this.ResetParam();
            }
        }

        public void Clear(Color color)
        {
            if (!ShanghaiEXE.rend)
                return;
            this.device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, color, 1f, 0);
        }

        public void Begin(Color color)
        {
            this.Clear(color);
            this.device.BeginScene();
            this.sprite.Begin(SpriteFlags.AlphaBlend);
        }

        public void End()
        {
            this.sprite.End();
            this.device.EndScene();
            this.device.Present();
        }

        public void AddBlendSet()
        {
            this.device.SetRenderState<Blend>(RenderState.SourceBlendAlpha, Blend.One);
            this.device.SetRenderState<Blend>(RenderState.DestinationBlend, Blend.One);
        }

        public void AddBlendEnd()
        {
            this.device.SetRenderState<Blend>(RenderState.SourceBlendAlpha, Blend.InverseSourceColor);
            this.device.SetRenderState<Blend>(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
        }

        public void ResetParam()
        {
            this.sprite.OnLostDevice();
            this.device.Reset(this.param);
            this.sprite.OnResetDevice();
            this.SetView();
        }

        public void Dispose()
        {
            this.device.Dispose();
            this.sprite.Dispose();
            this.direct3D.Dispose();
        }

        public virtual void DrawImage(
          IRenderer device,
          string t,
          Rectangle _rect,
          bool leftpoint,
          Vector2 _point,
          float scall,
          float rotation,
          Color color)
        {
            if (!ShanghaiEXE.rend)
                return;
            string _texture = t + ".png";
            try
            {
                var point = new SlimDX.Vector2(_point.X, _point.Y);
                var dgDevice = (MySlimDG)device;
                Matrix matrix = Matrix.AffineTransformation2D(scall, SlimDX.Vector2.Zero, MyMath.Rad(rotation), point);
                dgDevice.sprite.Transform = matrix;
                SlimDX.Vector3 vector3 = !leftpoint ? new SlimDX.Vector3(_rect.Width / 2, _rect.Height / 2, 0.0f) : new SlimDX.Vector3(0.0f, 0.0f, 0.0f);
                this.LoadTexture(_texture);
                dgDevice.sprite.Draw(this.form.Tex[_texture].Texture, new Rectangle?(_rect), new SlimDX.Vector3?(vector3), new SlimDX.Vector3?(new SlimDX.Vector3(0.0f, 0.0f, 0.0f)), new Color4(color));
                dgDevice.sprite.Transform = Matrix.Identity;
            }
            catch
            {
            }
        }

        public virtual void DrawImage(
          IRenderer device,
          string te,
          Rectangle _rect,
          bool leftpoint,
          Vector2 _point,
          float scall,
          float rotation,
          bool rebirth,
          Color color)
        {
            if (!ShanghaiEXE.rend)
                return;
            string _texture = te + ".png";
            try
            {
                var dgDevice = (MySlimDG)device;
                Point point = new Point((int)_point.X, (int)_point.Y);
                var dx_point = new SlimDX.Vector2(point.X, point.Y);
                Matrix matrix;
                if (!rebirth)
                {
                    matrix = Matrix.AffineTransformation2D(scall, SlimDX.Vector2.Zero, MyMath.Rad(rotation), dx_point);
                }
                else
                {
                    Quaternion rotation1 = new Quaternion(0.0f, -1f, 0.0f, 0.0f);
                    SlimDX.Vector3 rotationCenter = new SlimDX.Vector3(0.0f, 0.0f, 0.0f);
                    SlimDX.Vector3 translation = new SlimDX.Vector3(_point.X, _point.Y, 0.0f);
                    matrix = Matrix.AffineTransformation(scall, rotationCenter, rotation1, translation);
                }
                dgDevice.sprite.Transform = matrix;
                SlimDX.Vector3 vector3 = !leftpoint ? new SlimDX.Vector3(_rect.Width / 2, _rect.Height / 2, 0.0f) : new SlimDX.Vector3(0.0f, 0.0f, 0.0f);
                this.LoadTexture(_texture);
                dgDevice.sprite.Draw(this.form.Tex[_texture].Texture, new Rectangle?(_rect), new SlimDX.Vector3?(vector3), new SlimDX.Vector3?(new SlimDX.Vector3(0.0f, 0.0f, 0.0f)), new Color4(color));
                dgDevice.sprite.Transform = Matrix.Identity;
            }
            catch
            {
            }
        }

        public virtual void DrawImage(
          IRenderer device,
          string te,
          Rectangle _rect,
          bool leftpoint,
          Vector2 _point,
          bool rebirth,
          Color color)
        {
            if (!ShanghaiEXE.rend)
                return;
            string _texture = te + ".png";
            try
            {
                var dgDevice = (MySlimDG)device;
                Point point = new Point((int)_point.X, (int)_point.Y);
                var dx_point = new SlimDX.Vector2(point.X, point.Y);
                Matrix matrix;
                if (!rebirth)
                {
                    matrix = Matrix.AffineTransformation2D(1f, SlimDX.Vector2.Zero, MyMath.Rad(0.0f), dx_point);
                }
                else
                {
                    Quaternion rotation = new Quaternion(0.0f, -1f, 0.0f, 0.0f);
                    SlimDX.Vector3 rotationCenter = new SlimDX.Vector3(0.0f, 0.0f, 0.0f);
                    SlimDX.Vector3 translation = new SlimDX.Vector3(_point.X, _point.Y, 0.0f);
                    matrix = Matrix.AffineTransformation(1f, rotationCenter, rotation, translation);
                }
                dgDevice.sprite.Transform = matrix;
                SlimDX.Vector3 vector3 = !leftpoint ? new SlimDX.Vector3(_rect.Width / 2, _rect.Height / 2, 0.0f) : new SlimDX.Vector3(0.0f, 0.0f, 0.0f);
                this.LoadTexture(_texture);
                dgDevice.sprite.Draw(this.form.Tex[_texture].Texture, new Rectangle?(_rect), new SlimDX.Vector3?(vector3), new SlimDX.Vector3?(new SlimDX.Vector3(0.0f, 0.0f, 0.0f)), new Color4(color));
                dgDevice.sprite.Transform = Matrix.Identity;
            }
            catch
            {
            }
        }

        public virtual void DrawImage(
          IRenderer device,
          string te,
          Rectangle _rect,
          bool leftpoint,
          Vector2 _point,
          Color color)
        {
            if (!ShanghaiEXE.rend)
                return;
            string _texture = te + ".png";
            try
            {
                var dgDevice = (MySlimDG)device;
                Point point = new Point((int)_point.X, (int)_point.Y);
                var dx_point = new SlimDX.Vector2(point.X, point.Y);
                Matrix matrix = Matrix.AffineTransformation2D(1f, SlimDX.Vector2.Zero, MyMath.Rad(0.0f), dx_point);
                dgDevice.sprite.Transform = matrix;
                SlimDX.Vector3 vector3 = !leftpoint ? new SlimDX.Vector3(_rect.Width / 2, _rect.Height / 2, 0.0f) : new SlimDX.Vector3(0.0f, 0.0f, 0.0f);
                this.LoadTexture(_texture);
                dgDevice.sprite.Draw(this.form.Tex[_texture].Texture, new Rectangle?(_rect), new SlimDX.Vector3?(vector3), new SlimDX.Vector3?(new SlimDX.Vector3(0.0f, 0.0f, 0.0f)), new Color4(color));
                dgDevice.sprite.Transform = Matrix.Identity;
            }
            catch
            {
            }
        }

        public void LoadTexture(string texture)
        {
            if (this.form.Tex.ContainsKey(texture))
                return;
            this.tex.ReadTex(texture);
        }

        public ITextMeasurer GetTextMeasurer() => this.measurer;

        public bool TexNameCheck(string _texture)
        {
            return this.form.Tex.ContainsKey(_texture);
        }

        public void DrawText(string text, Vector2 point, Color color)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            this.font.Font.DrawString(this.sprite, text, x, y, new Color4(color));
        }

        public void DrawText(string text, Vector2 point, Color color, SaveData save)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            text = this.ControlCharacterChange(text, save);
            this.font.Font.DrawString(this.sprite, text, x, y, new Color4(color));
        }

        public void DrawText(string text, Vector2 point)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            Color color = Color.FromArgb(byte.MaxValue, 64, 56, 56);
            this.font.Font.DrawString(this.sprite, text, x, y, new Color4(color));
        }

        public void DrawText(string text, Vector2 point, SaveData save)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            text = this.ControlCharacterChange(text, save);
            Color color = Color.FromArgb(byte.MaxValue, 64, 56, 56);
            this.font.Font.DrawString(this.sprite, text, x, y, new Color4(color));
        }

        public void DrawText(string text, Vector2 point, bool shadow)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            if (shadow)
            {
                Color color = Color.FromArgb(byte.MaxValue, 32, 32, 32);
                this.font.Font.DrawString(this.sprite, text, x + 1, y + 1, new Color4(color));
            }
            Color white = Color.White;
            this.font.Font.DrawString(this.sprite, text, x, y, new Color4(white));
        }

        public void DrawText(string text, Vector2 point, bool shadow, SaveData save)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            text = this.ControlCharacterChange(text, save);
            if (shadow)
            {
                Color color = Color.FromArgb(byte.MaxValue, 32, 32, 32);
                this.font.Font.DrawString(this.sprite, text, x + 1, y + 1, new Color4(color));
            }
            Color white = Color.White;
            this.font.Font.DrawString(this.sprite, text, x, y, new Color4(white));
        }

        public void DrawMicroText(string text, Vector2 point, Color color)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            this.microfont.Font.DrawString(this.sprite, text, x, y, new Color4(color));
        }

        public void DrawMiniText(string text, Vector2 point, Color color)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            this.minifont.Font.DrawString(this.sprite, text, x, y, new Color4(color));
        }

        public void DrawMiniText(string text, Vector2 point, Color color, SaveData save)
        {
            if (!ShanghaiEXE.rend)
                return;
            int x = (int)point.X;
            int y = (int)point.Y;
            text = this.ControlCharacterChange(text, save);
            this.minifont.Font.DrawString(this.sprite, text, x, y, new Color4(color));
        }

        private string ControlCharacterChange(string text, SaveData save)
        {
            string str1 = "";
            str1 = "v";
            string[] strArray1 = text.Split('#');
            var strArray2 = new[]
            {
                "v",
                "s",
                "w",
                "b",
                "e",
                "i",
                "u",
                "c",
                "p"
            };
            string str2 = "";
            for (int index1 = 0; index1 < strArray1.Length; ++index1)
            {
                for (int index2 = 0; index2 < strArray2.Length; ++index2)
                {
                    string str3 = strArray2[index2];
                    if (strArray1[index1].Contains(str3 + "@"))
                    {
                        string[] strArray3 = strArray1[index1].Split('@');
                        string str4 = str3;
                        strArray1[index1] = str4 == "v" ? save.ValList[int.Parse(strArray3[1])].ToString() : (str4 == "i" ? save.item : (str4 == "c" ? save.category : ""));
                    }
                }
                str2 += strArray1[index1];
            }
            text = str2;
            return text;
        }

        private void TCDLoad_ProgressUpdate(object sender, TextureLoadProgressUpdatedEventArgs e)
        {
            if (e == null)
            {
                this.ProgressUpdated?.Invoke(this, null);
                ((Textures)sender).ProgressUpdated -= this.TCDLoad_ProgressUpdate;
            }
            else
            {
                this.ProgressUpdated?.Invoke(this, new TextureLoadProgressUpdatedEventArgs(e.UpdateLabel, e.UpdateProgress));
            }
        }

        public void AbortRenderThread()
        {
            this.thread_1.Abort();
        }
    }
}
