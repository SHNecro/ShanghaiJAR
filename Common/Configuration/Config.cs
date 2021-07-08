using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Common.Config
{
    [Serializable]
    [Obfuscation(Exclude = true)]
    public class Config
    {
        private double scaleFactor = 1;

        public Config()
        {
            this.ApplyNewFieldDefaults();
        }
        
        public Mappings ControllerMapping { get; set; } = new Mappings
        {
            Up = 100, Right = 102, Down = 101, Left = 103,
            A = 0, B = 1, L = 4, R = 5,
            Start = 7, Select = 6,
            Turbo = 8
        };
        
        public Mappings KeyboardMapping { get; set; } = new Mappings
        {
            Up = 132, Right = 118, Down = 50, Left = 76,
            A = 35, B = 33, L = 10, R = 28,
            Start = 117, Select = 126,
            Turbo = 78
        };

        public double ScaleFactor {
            get
            {
                return scaleFactor;
            }
            set
            {
                if (value > 0)
                {
                    scaleFactor = value;
                }
            }
        }

        public bool Fullscreen { get; set; } = false;

        public double VolumeBGM { get; set; } = 100;

        public double VolumeSE { get; set; } = 100;

        public bool PausedWhenInactive { get; set; } = false;

        public string Language { get; set; } = "en-US";

        public bool ShowDialogueTester { get; set; } = false;

        #region New

        [OptionalField(VersionAdded = 2)]
        private int? fps;
        [OptionalField(VersionAdded = 2)]
        private bool? allowTurboSlowdown;
        [OptionalField(VersionAdded = 2)]
        private int? turboUPS;
        [OptionalField(VersionAdded = 3)]
        private bool? disableBGMOverride;
        [OptionalField(VersionAdded = 4)]
        private bool? fixEngrish;
        [OptionalField(VersionAdded = 5)]
        private bool? stretchFullscreen;
        [OptionalField(VersionAdded = 6)]
        private string audioEngine;

        public string RenderEngine { get; set; }

        public string AudioEngine { get => this.audioEngine; set => this.audioEngine = value; }

        public int? FPS
        {
            get
            {
                return this.fps;
            }
            set
            {
                this.fps = Math.Max(1, value ?? 144);
            }
        }

        public bool? AllowTurboSlowdown { get => this.allowTurboSlowdown; set => this.allowTurboSlowdown = value; }

        public int? TurboUPS
        {
            get
            {
                return this.turboUPS;
            }
            set
            {
                this.turboUPS = this.AllowTurboSlowdown.HasValue && this.AllowTurboSlowdown.Value ? value : Math.Max(60, value ?? 300);
            }
        }

        public bool? DisableBGMOverride { get => this.disableBGMOverride; set => this.disableBGMOverride = value; }

        public bool? FixEngrish { get => this.fixEngrish; set => this.fixEngrish = value; }

        public bool? StretchFullscreen { get => this.stretchFullscreen; set => this.stretchFullscreen = value; }

        private void ApplyNewFieldDefaults()
        {
            this.RenderEngine = this.RenderEngine ?? "OpenGL";
            this.AudioEngine = this.AudioEngine ?? "DirectSound";
            this.KeyboardMapping.Turbo = this.KeyboardMapping.Turbo ?? 78;
            this.ControllerMapping.Turbo = this.ControllerMapping.Turbo ?? 8;
            this.FPS = this.FPS ?? (this.FPS30 ? 30 : 60);
            this.AllowTurboSlowdown = this.AllowTurboSlowdown ?? false;
            this.TurboUPS = this.TurboUPS ?? 300;
            this.DisableBGMOverride = this.DisableBGMOverride ?? false;
            this.FixEngrish = this.FixEngrish ?? true;
            this.StretchFullscreen = this.StretchFullscreen ?? true;
        }

        #endregion

        #region Deprecated

        private bool fps30;

        public bool FPS30 { get => this.FPS < 45; set => this.fps30 = value; }

        #endregion

        public void ToXML(string filename)
        {
            var serializer = new XmlSerializer(typeof(Config));
            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, this);
            }
        }

        public static Config FromXML(string filename)
        {
            try
            {
                using (var configReader = new StreamReader(filename))
                {
                    var deserializer = new XmlSerializer(typeof(Config));
                    var newConfig = (Config)deserializer.Deserialize(configReader);
                    newConfig.ApplyNewFieldDefaults();
                    return newConfig;
                }
            }
            catch
            {
                return null;
            }
        }

        public static Config FromCFG(string filename)
        {
            try
            {
                using (var configReader = new StreamReader(filename))
                {
                    var config = new Config();
                    config.ControllerMapping.Up = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.Right = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.Down = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.Left = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.A = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.B = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.L = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.R = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.Start = int.Parse(configReader.ReadLine());
                    config.ControllerMapping.Select = int.Parse(configReader.ReadLine());

                    config.KeyboardMapping.Up = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.Right = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.Down = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.Left = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.A = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.B = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.L = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.R = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.Start = int.Parse(configReader.ReadLine());
                    config.KeyboardMapping.Select = int.Parse(configReader.ReadLine());

                    config.ScaleFactor = int.Parse(configReader.ReadLine());
                    config.Fullscreen = configReader.ReadLine() == "1";
                    config.VolumeBGM = double.Parse(configReader.ReadLine()) * 10;
                    config.VolumeSE = double.Parse(configReader.ReadLine()) * 10;
                    config.PausedWhenInactive = configReader.ReadLine() == "1";
                    config.FPS30 = configReader.ReadLine() == "1";
                    config.Language = configReader.ReadLine() == "1" ? "en-US" : "ja-JP";

                    config.ApplyNewFieldDefaults();
                    return config;
                }
            }
            catch
            {
                return null;
            }
        }
    }

    [Serializable]
    [Obfuscation(Exclude = true)]
    public class Mappings
    {
        [OptionalField(VersionAdded = 2)]
        private int? turbo;

        public int Up { get; set; }
        public int Right { get; set; }
        public int Down { get; set; }
        public int Left { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int L { get; set; }
        public int R { get; set; }
        public int Start { get; set; }
        public int Select { get; set; }
        public int? Turbo { get => this.turbo; set => this.turbo = value; }
    }
}
