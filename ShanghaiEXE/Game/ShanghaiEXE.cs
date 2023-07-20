using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSMap.Character.Menu;
using Common.Vectors;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NSTitle;
using Services;
using Common;
using NSShanghaiEXE.Game;
using Button = NSShanghaiEXE.InputOutput.Button;
using NSShanghaiEXE.InputOutput.Rendering.OpenGL;
using NSShanghaiEXE.Common;
using NSShanghaiEXE.InputOutput.Rendering;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using Common.Config;
using System.Diagnostics;
using NSEvent;
using NSShanghaiEXE.InputOutput.Audio.XAudio2;
using NSShanghaiEXE.InputOutput.Audio.OpenAL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace NSGame
{
    public class ShanghaiEXE : Form
    {
        private Vector2 CursorVector2 = new Vector2(80f, 80f);
        private Stopwatch updateStopwatch = Stopwatch.StartNew();
        private Stopwatch renderStopwatch = Stopwatch.StartNew();
        private Stopwatch fpsUpdateStopwatch = Stopwatch.StartNew();
        private int rendersSinceLastFPSUpdate = 0;
        private int updatesSinceLastFPSUpdate = 0;
        private double fpsAdjustmentFactor = 0;
        private static readonly int FPSAdjustmentWindow = 10;
        private static readonly TimeSpan FPSUpdatePeriod = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan FPSAdjustmentPeriod = TimeSpan.FromSeconds(0.05);
        private Stopwatch fpsAdjustmentStopwatch = Stopwatch.StartNew();
        private int updatesSinceLastFPSAdjustment = 0;
        private static readonly int UpdateRate = 60;
        public Dictionary<string, SlimTex> Tex = new Dictionary<string, SlimTex>();
        public string[] KeepTexList = new string[87]
        {
            "battleobjects.png",
            "charachip1.png",
            "objects1.png",
            "shield.png",
            "bombs.png",
            "barrier.png",
            "bomber.png",
            "charge.png",
            "darkPA.png",
            "plugineffect.png",
            "shot.png",
            "smoke.png",
            "steal.png",
            "sword.png",
            "tornado.png",
            "towers.png",
            "fadescreen.png",
            "font.png",
            "menuwindows.png",
            "systemwindow.png",
            "window.png",
            "bakebake.png",
            "bakebake.png",
            "Barlizard.png",
            "beetle.png",
            "beetleman.png",
            "bibitybat.png",
            "bouzu.png",
            "Brocooler.png",
            "bronzer.png",
            "cirno.png",
            "doripper.png",
            "evileye.png",
            "firecat.png",
            "flae.png",
            "flowertank.png",
            "furjirn.png",
            "gekohat.png",
            "gelpark.png",
            "gunbut.png",
            "hakutaku.png",
            "holenake.png",
            "iku.png",
            "junks.png",
            "juraigon.png",
            "kedamar.png",
            "KorYor.png",
            "lanster.png",
            "mantiser.png",
            "marisa.png",
            "massdliger.png",
            "medicine.png",
            "mossa.png",
            "mrasa.png",
            "musya.png",
            "navi1.png",
            "navi10.png",
            "navi11.png",
            "navi12.png",
            "navi2.png",
            "navi3.png",
            "navi4.png",
            "navi5.png",
            "navi6.png",
            "navi8.png",
            "navi9.png",
            "onohawk.png",
            "panemole.png",
            "poisorlin.png",
            "ponpoko.png",
            "putioni.png",
            "PyroMan.png",
            "raijun.png",
            "reycanon.png",
            "rieber.png",
            "riveradar.png",
            "sakuya.png",
            "screwn.png",
            "shelln.png",
            "spannerman.png",
            "SquAnchor.png",
            "swordog.png",
            "tankman.png",
            "TortoiseMan.png",
            "woojow.png",
            "yorihime.png",
            "zarinear.png"
        };
        public List<string> KeepActiveTexList = new List<string>();
        private bool init = false;
        public bool loadend = false;
        private readonly IContainer components = null;
        private MyKeyBoard mk;
        private Controller co;
        public IRenderer dg;
        public IAudioEngine ad;
        private static SceneBase scene;
        public SaveData savedata;
        public bool tutorial;
        public int battlenum;
        public Loading loading;
        private readonly bool fps30;
        public float volBGM;
        public float volSE;
        public bool textureLoad;
        public bool soundLoad;
        public bool loadSUCCESS;
        public static ITextMeasurer measurer;
        public static bool rend;
        public static Config Config;
        private bool isPaused;
        private double scaleFactorX;
        private double scaleFactorY;

        public static ILanguageTranslationService languageTranslationService;

        public MyKeyBoard MyKeyBoard
        {
            get
            {
                return this.mk;
            }
        }

        public Controller Controller
        {
            get
            {
                return this.co;
            }
        }

        public void UpdateLoadingText(LoadType type, int progress)
        {
            this.loading.BeginInvoke(new Action(() =>
            {
                this.loading.UpdateLoadingText(type, progress);
            }));
        }

        public void TexClear(bool All)
        {
            if (!All && this.Tex.Count <= 150)
                return;
            List<string> stringList = new List<string>();
            foreach (KeyValuePair<string, SlimTex> keyValuePair in this.Tex)
            {
                if (All)
                    keyValuePair.Value.Dispose();
                else if (this.TexNameKeepCheck(keyValuePair.Key))
                {
                    keyValuePair.Value.Dispose();
                    stringList.Add(keyValuePair.Key);
                }
            }
            if (All)
            {
                this.Tex.Clear();
            }
            else
            {
                foreach (string key in stringList)
                    this.Tex.Remove(key);
            }
        }

        public void MapTextureAdd(string mapName)
        {
            for (int index = 1; index < 10; ++index)
            {
                string _texture = mapName + index.ToString() + ".png";
                if (!this.TexNameCheckList(_texture))
                    break;
                if (!this.KeepActiveTexList.Contains(_texture))
                    this.KeepActiveTexList.Add(_texture);
            }
        }

        public bool TexNameCheckList(string _texture)
        {
            return Textures.texSizeList.ContainsKey(_texture);
        }

        public bool TexNameKeepCheck(string key)
        {
            new List<string>().Concat<string>(this.KeepTexList.Cast<string>()).Concat<string>(this.KeepActiveTexList.Cast<string>()).ToList<string>();
            foreach (string keepTex in this.KeepTexList)
            {
                if (key == keepTex)
                    return false;
            }
            return true;
        }

        public static Dialogue Translate(string key) => ShanghaiEXE.languageTranslationService?.Translate(key) ?? new Dialogue { Text = key };

        public ShanghaiEXE()
        {
            Debug.DebugSet();
            this.InitializeComponent();
            this.Icon = new Icon("icon.ico");

            var oldConfig = Config.FromCFG("option.cfg");
            if (oldConfig != null)
            {
                File.Move("option.cfg", "option.cfg.OLD");
            }

            ShanghaiEXE.Config = Config.FromXML("option.xml") ?? oldConfig ?? new Config();

            SaveData.Pad[1, 0] = ShanghaiEXE.Config.ControllerMapping.Up;
            SaveData.Pad[1, 1] = ShanghaiEXE.Config.ControllerMapping.Right;
            SaveData.Pad[1, 2] = ShanghaiEXE.Config.ControllerMapping.Down;
            SaveData.Pad[1, 3] = ShanghaiEXE.Config.ControllerMapping.Left;
            SaveData.Pad[1, 4] = ShanghaiEXE.Config.ControllerMapping.A;
            SaveData.Pad[1, 5] = ShanghaiEXE.Config.ControllerMapping.B;
            SaveData.Pad[1, 6] = ShanghaiEXE.Config.ControllerMapping.L;
            SaveData.Pad[1, 7] = ShanghaiEXE.Config.ControllerMapping.R;
            SaveData.Pad[1, 8] = ShanghaiEXE.Config.ControllerMapping.Start;
            SaveData.Pad[1, 9] = ShanghaiEXE.Config.ControllerMapping.Select;
            SaveData.Pad[1, 11] = ShanghaiEXE.Config.ControllerMapping.Turbo ?? 8;

            SaveData.Pad[0, 0] = ShanghaiEXE.Config.KeyboardMapping.Up;
            SaveData.Pad[0, 1] = ShanghaiEXE.Config.KeyboardMapping.Right;
            SaveData.Pad[0, 2] = ShanghaiEXE.Config.KeyboardMapping.Down;
            SaveData.Pad[0, 3] = ShanghaiEXE.Config.KeyboardMapping.Left;
            SaveData.Pad[0, 4] = ShanghaiEXE.Config.KeyboardMapping.A;
            SaveData.Pad[0, 5] = ShanghaiEXE.Config.KeyboardMapping.B;
            SaveData.Pad[0, 6] = ShanghaiEXE.Config.KeyboardMapping.L;
            SaveData.Pad[0, 7] = ShanghaiEXE.Config.KeyboardMapping.R;
            SaveData.Pad[0, 8] = ShanghaiEXE.Config.KeyboardMapping.Start;
            SaveData.Pad[0, 9] = ShanghaiEXE.Config.KeyboardMapping.Select;
            SaveData.Pad[0, 11] = ShanghaiEXE.Config.KeyboardMapping.Turbo ?? 78;

            this.scaleFactorX = ShanghaiEXE.Config.ScaleFactor;
            this.scaleFactorY = ShanghaiEXE.Config.ScaleFactor;

            var clientWidth = (int)(240 * Math.Max(1, this.scaleFactorX));
            var clientHeight = (int)(160 * Math.Max(1, this.scaleFactorY));
            this.ClientSize = new Size(clientWidth, clientHeight);

            if (!ShanghaiEXE.Config.Fullscreen)
            {
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                SaveData.ScreenMode = false;
            }
            else
            {
                SaveData.ScreenMode = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                if (ShanghaiEXE.Config.RenderEngine == "OpenGL")
                {
                    var screenSize = Screen.FromControl(this).Bounds;
                    if (ShanghaiEXE.Config.StretchFullscreen == null || ShanghaiEXE.Config.StretchFullscreen.Value)
                    {
                        this.scaleFactorX = (double)screenSize.Width / Constants.ScreenSize.Width;
                        this.scaleFactorY = (double)screenSize.Height / Constants.ScreenSize.Height;
                    }
                    else
                    {
                        var minimumEvenScale = Math.Min((double)screenSize.Width / Constants.ScreenSize.Width, (double)screenSize.Height / Constants.ScreenSize.Height);
                        this.scaleFactorX = minimumEvenScale;
                        this.scaleFactorY = minimumEvenScale;
                    }
                }
                this.ControlBox = false;
                this.Text = String.Empty;
            }


            this.volBGM = (float)ShanghaiEXE.Config.VolumeBGM;
            this.volSE = (float)(ShanghaiEXE.Config.VolumeSE / 100);

            Controller.ctl = (ShanghaiEXE.Config.PausedWhenInactive) ? CooperativeLevel.Foreground : CooperativeLevel.Background;

            this.fps30 = ShanghaiEXE.Config.FPS30;

            ShanghaiEXE.languageTranslationService = new LanguageTranslationService(ShanghaiEXE.Config.Language);

            if (ShanghaiEXE.Config.ShowDialogueTester)
            {
                var dialogueTester = new DialogueTester(this, () => scene);
                dialogueTester.Show();
            }

            ShanghaiEXE.Config.ToXML("option.xml");
            
            this.Closing += new CancelEventHandler(this.Game_Closing);
            this.MaximizeBox = false;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
        }

        public void InitializeData()
        {
            this.loading.LoadComplete += this.Loading_LoadComplete;

            this.UpdateLoadingText(LoadType.Data, 100);
            this.UpdateLoadingText(LoadType.Graphics, 0);
            switch (ShanghaiEXE.Config.RenderEngine)
            {
                case "DirectX9":
                    var dgRenderer = new MySlimDG(this);
                    dgRenderer.ProgressUpdated += this.TextureLoad_ProgressUpdate;
                    this.dg = dgRenderer;
                    break;
                case "OpenGL":
                    var renderControl = new OpenGLRenderer("ShaGResource.tcd", "sasanasi", "{0}.png", this.scaleFactorX, this.scaleFactorY);
                    renderControl.GetPanel().SetSize(Constants.ScreenSize);
                    renderControl.ProgressUpdated += this.TextureLoad_ProgressUpdate;
                    this.dg = renderControl;
                    var renderPanel = renderControl.GetPanel();
                    if (ShanghaiEXE.Config.Fullscreen)
                    {
                        renderPanel.SizeChanged += (s, e) =>
                        {
                            renderPanel.Location = new Point((this.Width - renderPanel.Width) / 2, (this.Height - renderPanel.Height) / 2);
                        };
                        this.SizeChanged += (s, e) =>
                        {
                            renderPanel.Location = new Point((this.Width - renderPanel.Width) / 2, (this.Height - renderPanel.Height) / 2);
                        };
                    }
                    this.Controls.Add(renderPanel);
                    this.BackColor = Color.Black;
                    break;
            }
            ShanghaiEXE.measurer = this.dg.GetTextMeasurer();

            this.UpdateLoadingText(LoadType.Audio, 25);
            switch (ShanghaiEXE.Config.AudioEngine)
            {
                case "DirectSound":
                    this.ad = new MyAudio(this.volSE);
                    break;
                case "OpenAL":
                    this.ad = new OpenALAudio(this.volSE, "ShaSResource.tcd", "sasanasi", "{0}.wav");
                    break;
            }
            this.ad.BGMVolume = this.volBGM;
            this.ad.SoundEffectVolume = this.volSE;
            this.ad.ProgressUpdated += this.AudioLoad_ProgressUpdate;
            this.UpdateLoadingText(LoadType.Audio, 100);

            this.UpdateLoadingText(LoadType.Device, 25);
            this.mk = new MyKeyBoard(this);
            this.co = new Controller(this);
            Input.FormSetting(this);
            this.UpdateLoadingText(LoadType.Device, 100);

            this.UpdateLoadingText(LoadType.Save, 25);
            this.savedata = new SaveData();
            this.savedata.Init();
            this.UpdateLoadingText(LoadType.Save, 50);
            this.savedata.Load(this);
            this.UpdateLoadingText(LoadType.Save, 100);
        }

        private void Init()
        {
            this.loadSUCCESS = this.savedata.loadSucces;
            ShanghaiEXE.scene = new FirstTitle(this.ad, this, this.savedata);
            ShanghaiEXE.scene.Init();
            this.init = true;
            this.Text = ShanghaiEXE.Translate("Common.Title").Text;
        }

        private void GetKeyData()
        {
            this.mk.GetKeyData();
            this.co.GetKeyData();
        }

        public void MainLoop()
        {
            var desiredUpdateRate = UpdateRate;
            var isTurbo = Input.IsPush(Button.Turbo);
            if (isTurbo)
            {
                desiredUpdateRate = (ShanghaiEXE.Config.AllowTurboSlowdown ?? false)
                    ? ShanghaiEXE.Config.TurboUPS.Value
                    : Math.Max(60, ShanghaiEXE.Config.TurboUPS ?? 300);
            }
            var fpsAdjustment = isTurbo || Math.Abs(this.fpsAdjustmentFactor) < double.Epsilon || double.IsNaN(this.fpsAdjustmentFactor) || double.IsInfinity(this.fpsAdjustmentFactor) ? 1 : this.fpsAdjustmentFactor;
            var updatePeriod = TimeSpan.FromMilliseconds(1000d / (desiredUpdateRate * fpsAdjustment));
            var renderPeriod = TimeSpan.FromMilliseconds(1000d / ShanghaiEXE.Config.FPS ?? 60);

            var isUpdatePaused = this.isPaused && ShanghaiEXE.Config.PausedWhenInactive;

            var queuedUpdates = 0;
            if (updatePeriod > TimeSpan.Zero)
            {
                queuedUpdates = (int)(this.updateStopwatch.ElapsedMilliseconds / updatePeriod.TotalMilliseconds);
            }
            else
            {
                // If DirectX9, dg.End() blocks execution and queuedUpdates = 1 caps speed to refresh rate
                // "Incorrect" behavior if OpenGL or when FPS set to 144 on a 60hz screen, but this is edge case anyways with essentially infinite speed
                // TODO: split render to separate thread, if safe and no race conditions from render changing things (unlikely)
                queuedUpdates = (int)(renderPeriod.TotalSeconds / (1d / desiredUpdateRate));
            }

            var isUpdating = queuedUpdates > 0;
            if (isUpdating)
            {
                this.updateStopwatch.Restart();
            }
            var isRendering = this.renderStopwatch.Elapsed >= renderPeriod;
            if (isRendering)
            {
                this.renderStopwatch.Restart();
                this.rendersSinceLastFPSUpdate++;
            }

            if (isUpdating || isRendering)
            {
                if (this.init && (this.soundLoad && this.textureLoad))
                {
                    this.Show();
                    this.loading.Dispose();
                    this.init = false;
                    this.loadend = true;
                    this.dg.AbortRenderThread();
                }
                if (this.loadend)
                {
                    if (isUpdating)
                    {
                        this.GetKeyData();
                    }

                    if (ShanghaiEXE.scene != null)
                    {
                        if (!isUpdatePaused)
                        {
                            for (var i = 0; i < queuedUpdates; i++)
                            {
                                if (i > 0)
                                {
                                    this.GetKeyData();
                                }
                                ShanghaiEXE.scene.Updata();

                                this.updatesSinceLastFPSUpdate++;
                                this.updatesSinceLastFPSAdjustment++;
                            }
                        }

                        ShanghaiEXE.rend = isRendering;
                        if (ShanghaiEXE.rend)
                        {
                            this.dg.Begin(Color.Black);
                            ShanghaiEXE.scene.Render(this.dg);
                            this.dg.End();
                        }

                        if (this.ad.MusicPlay)
                        {
                            this.ad.PlayingMusic();
                            this.ad.BGMFade();
                        }
                    }
                }
                else
                {
                    this.loading.MainLoop();
                }
                Application.DoEvents();
            }

            var timeSinceLastFPSAdjustment = this.fpsAdjustmentStopwatch.Elapsed;
            if (!isUpdatePaused && timeSinceLastFPSAdjustment > ShanghaiEXE.FPSAdjustmentPeriod)
            {
                if (this.loadend)
                {
                    var expectedUpdatesSinceLastAdjustment = ShanghaiEXE.FPSAdjustmentPeriod.TotalMilliseconds / updatePeriod.TotalMilliseconds;
                    var newFpsAdjustment = expectedUpdatesSinceLastAdjustment / this.updatesSinceLastFPSAdjustment;
                    this.fpsAdjustmentFactor = (newFpsAdjustment + (fpsAdjustment * (ShanghaiEXE.FPSAdjustmentWindow - 1))) / ShanghaiEXE.FPSAdjustmentWindow;
                }
                this.updatesSinceLastFPSAdjustment = 0;
                this.fpsAdjustmentStopwatch.Restart();
            }
            if (isUpdatePaused || isTurbo)
            {
                this.fpsAdjustmentStopwatch.Restart();
                this.updatesSinceLastFPSAdjustment = 0;
            }

            var timeSinceLastFPSUpdate = this.fpsUpdateStopwatch.Elapsed;
            if (timeSinceLastFPSUpdate > ShanghaiEXE.FPSUpdatePeriod)
            {
                if (this.updatesSinceLastFPSUpdate != 0)
                {
                    var title = ShanghaiEXE.Translate("Common.Title").Text;
                    var fpsString = $"  FPS {this.rendersSinceLastFPSUpdate / timeSinceLastFPSUpdate.TotalSeconds:0.##} ({this.updatesSinceLastFPSUpdate / timeSinceLastFPSUpdate.TotalSeconds:0.##})";
                    if (!ShanghaiEXE.Config.Fullscreen)
                    {
                        this.Text = title + fpsString;
                    }
                }

                this.fpsUpdateStopwatch.Restart();
                this.rendersSinceLastFPSUpdate = 0;
                this.updatesSinceLastFPSUpdate = 0;
            }

            if (!this.loadend || !isTurbo)
            {
                Thread.Sleep(1);
            }
        }

        public void ChangeOfSecne(Scene change)
        {
            switch (change)
            {
                case Scene.Title:
                    ShanghaiEXE.scene = new SceneTitle(this.ad, this, this.savedata);
                    break;
                case Scene.Main:
                    ShanghaiEXE.scene = new SceneMain(this.ad, this, this.savedata);
                    break;
                case Scene.GameOver:
                    ShanghaiEXE.scene = new GameOver(this.ad, this, this.savedata);
                    break;
            }
            ShanghaiEXE.scene.Init();
        }

        public void NewGame(int plus)
        {
            SceneMain scene = (SceneMain)ShanghaiEXE.scene;
            scene.mapscene.NewGame(plus);
            ShanghaiEXE.scene = scene;
        }

        public void LoadGame()
        {
            SceneMain scene = (SceneMain)ShanghaiEXE.scene;

            var retconMessages = this.savedata.RetconSave();
            if (retconMessages.Any())
            {
                scene.mapscene.eventmanager.AddEvent(new Fade(this.ad, scene.mapscene.eventmanager, 5, 255, 0, 0, 0, true, this.savedata));
                scene.mapscene.eventmanager.AddEvent(new OpenMassageWindow(this.ad, scene.mapscene.eventmanager));
                var retconListQuestion = ShanghaiEXE.Translate("Retcon.OpeningMessageQuestion");
                var retconListOptions = ShanghaiEXE.Translate("Retcon.OpeningMessageQuestionOptions");
                scene.mapscene.eventmanager.AddEvent(new Question(
                    this.ad,
                    scene.mapscene.eventmanager,
                    retconListQuestion[0],
                    retconListQuestion[1],
                    retconListOptions[0],
                    retconListOptions[1],
                    retconListQuestion.Face.Mono,
                    true,
                    retconListQuestion.Face,
                    this.savedata,
                    true));
                scene.mapscene.eventmanager.AddEvent(new BranchHead(this.ad, scene.mapscene.eventmanager, 0, this.savedata));
                scene.mapscene.eventmanager.AddEvent(new CanSkip(this.ad, scene.mapscene.eventmanager, this.savedata));
				foreach (var message in retconMessages)
				{
                    scene.mapscene.eventmanager.AddEvent(new CommandMessage(this.ad, scene.mapscene.eventmanager, message[0], message[1], message[2], message.Face, message.Face.Mono, message.Face.Auto, this.savedata));
                }
                scene.mapscene.eventmanager.AddEvent(new BranchEnd(this.ad, scene.mapscene.eventmanager, this.savedata));
                scene.mapscene.eventmanager.AddEvent(new CloseMassageWindow(this.ad, scene.mapscene.eventmanager));
                scene.mapscene.eventmanager.AddEvent(new StopSkip(this.ad, scene.mapscene.eventmanager, this.savedata));
                scene.mapscene.eventmanager.AddEvent(new Fade(this.ad, scene.mapscene.eventmanager, 15, 0, 0, 0, 0, true, this.savedata));
            }

            scene.mapscene.LoadGame();
            ShanghaiEXE.scene = scene;
        }

        private void Game_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(640, 480);
            if (!ShanghaiEXE.Config.Fullscreen)
            {
                this.Text = ShanghaiEXE.Translate("Common.Title");
            }
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
        }

        private void Game_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                this.TexClear(true);
                this.dg.Dispose();
                this.ad.Dispose();
            }
            catch
            {
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F10)
                return;
            e.Handled = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void AudioLoad_ProgressUpdate(object sender, AudioLoadProgressUpdatedEventArgs e)
        {
            if (e == null)
            {
                this.soundLoad = true;
                this.UpdateLoadingText(LoadType.Audio, 100);
                ((IAudioEngine)sender).ProgressUpdated -= this.AudioLoad_ProgressUpdate;
            }
            else
            {
                this.UpdateLoadingText(LoadType.Audio, (int)Math.Round(100 * e.UpdateProgress));
            }
        }

        private void TextureLoad_ProgressUpdate(object sender, TextureLoadProgressUpdatedEventArgs e)
        {
            if (e == null)
            {
                this.textureLoad = true;
                this.UpdateLoadingText(LoadType.Graphics, 100);
                ((IRenderer)sender).ProgressUpdated -= this.TextureLoad_ProgressUpdate;
            }
            else
            {
                this.UpdateLoadingText(LoadType.Graphics, (int)Math.Round(100 * e.UpdateProgress));
            }
        }

        private void Loading_LoadComplete(object sender, EventArgs e)
        {
            this.Init();
            ((Loading)sender).LoadComplete -= this.Loading_LoadComplete;
        }

        private void ShanghaiEXE_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.TexClear(true);
                this.dg.Dispose();
                this.ad.Dispose();
                this.loading.Close();
            }
            catch
            {
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6f, 12f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(632, 446);
            this.Name = nameof(ShanghaiEXE);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += new FormClosingEventHandler(this.ShanghaiEXE_FormClosing);
            this.Deactivate += (sender, args) => this.isPaused = true;
            this.Activated += (sender, args) => this.isPaused = false;
            this.ResumeLayout(false);
        }
    }
}
