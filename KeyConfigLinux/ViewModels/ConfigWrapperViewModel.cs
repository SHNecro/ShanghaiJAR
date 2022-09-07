using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Common.Config;
using KeyConfigLinux.Common;
using KeyConfigLinux.Converters;

namespace KeyConfigLinux.ViewModels
{
    public class ConfigWrapperViewModel : ViewModelBase
    {
        private readonly Config config;
        
        public ConfigWrapperViewModel(Config config)
        {
            this.config = config;
            this.selectedRenderEngineEntry = this.RenderEngineEntries.FirstOrDefault(e => e.Item2 == this.RenderEngine);
            this.selectedAudioEngineEntry = this.AudioEngineEntries.FirstOrDefault(e => e.Item2 == this.AudioEngine);
            this.selectedLanguageEntry = RegionToTranslatedConverter.Locales.FirstOrDefault(e => e.Item2 == this.Language);
        }

        public double ScaleFactor
        {
            get => this.config.ScaleFactor;
            set
            {
                this.SetValue(val => this.config.ScaleFactor = val, value);
                this.OnPropertyChanged(nameof(this.IsCustomScaleFactor));
            }
        }

        public bool IsCustomScaleFactor => !((IList) new double[] {1, 2, 3, 4}).Contains(this.ScaleFactor);
        
        public bool? StretchFullscreen 
        {
            get => this.config.StretchFullscreen;
            set => this.SetValue(val => this.config.StretchFullscreen = val, value);
        }
        
        public bool Fullscreen
        {
            get => this.config.Fullscreen;
            set => this.SetValue(val => this.config.Fullscreen = val, value);
        }

        public bool PausedWhenInactive
        {
            get => this.config.PausedWhenInactive;
            set => this.SetValue(val => this.config.PausedWhenInactive = val, value);
        }

        public int? FPS
        {
            get => this.config.FPS;
            set => this.SetValue(val => this.config.FPS = val, value);
        }

        public int? TurboUPS
        {
            get => this.config.TurboUPS;
            set => this.SetValue(val => this.config.TurboUPS = val, value);
        }

        public double VolumeBGM
        {
            get => this.config.VolumeBGM;
            set => this.SetValue(val => this.config.VolumeBGM = val, value);
        }

        public double VolumeSE
        {
            get => this.config.VolumeSE;
            set => this.SetValue(val => this.config.VolumeSE = val, value);
        }

        public string RenderEngine
        {
            get => this.config.RenderEngine;
            set => this.SetValue(val => this.config.RenderEngine = val, value);
        }

        public string AudioEngine
        {
            get => this.config.AudioEngine;
            set => this.SetValue(val => this.config.AudioEngine = val, value);
        }
        
        private static RegionToTranslatedConverter translationConverter = new RegionToTranslatedConverter();
        private static string Translate(string language, string key) => translationConverter.Convert(language, typeof(string), key, CultureInfo.CurrentCulture) as string;

        public List<Tuple<string, string>> RenderEngineEntries => new List<Tuple<string, string>>
        {
            Tuple.Create(Translate(this.Language, "OpenGL"), "OpenGL"),
            Tuple.Create(Translate(this.Language, "DirectX9"), "DirectX9"),
        };

        private Tuple<string, string> selectedRenderEngineEntry;
        public Tuple<string, string> SelectedRenderEngineEntry
        {
            get => this.selectedRenderEngineEntry;
            set
            {
                this.selectedRenderEngineEntry = value;
                this.RenderEngine = value.Item2;
            }
        }

        public IList<Tuple<string, string>> AudioEngineEntries => new List<Tuple<string, string>>
        {
            Tuple.Create(Translate(this.Language, "DirectSound"), "DirectSound"),
            Tuple.Create(Translate(this.Language, "OpenAL"), "OpenAL"),
        };

        private Tuple<string, string> selectedAudioEngineEntry;
        public Tuple<string, string> SelectedAudioEngineEntry
        {
            get => this.selectedAudioEngineEntry;
            set
            {
                this.selectedAudioEngineEntry = value;
                this.AudioEngine = value.Item2;
            }
        }

        public string Language
        {
            get => this.config.Language;
            set => this.SetValue(val => this.config.Language = val, value);
        }

        private Tuple<string, string> selectedLanguageEntry;
        public Tuple<string, string> SelectedLanguageEntry
        {
            get => this.selectedLanguageEntry;
            set
            {
                this.selectedLanguageEntry = value;
                this.Language = value.Item2;
                this.OnPropertyChanged(nameof(this.RenderEngineEntries));
                this.OnPropertyChanged(nameof(this.AudioEngineEntries));
            }
        }

        public bool? AllowTurboSlowdown
        {
            get => this.config.AllowTurboSlowdown;
            set => this.SetValue(val => this.config.AllowTurboSlowdown = val, value);
        }

        public bool? DisableBGMOverride
        {
            get => this.config.DisableBGMOverride;
            set => this.SetValue(val => this.config.DisableBGMOverride = val, value);
        }

        public bool ShowDialogueTester
        {
            get => this.config.ShowDialogueTester;
            set => this.SetValue(val => this.config.ShowDialogueTester = val, value);
        }

        public bool? FixEngrish
        {
            get => this.config.FixEngrish;
            set => this.SetValue(val => this.config.FixEngrish = val, value);
        }

        public Mappings KeyboardMapping
        {
            get => this.config.KeyboardMapping;
            set => this.SetValue(val => this.config.KeyboardMapping = val, value);
        }

        public Mappings ControllerMapping
        {
            get => this.config.ControllerMapping;
            set => this.SetValue(val => this.config.ControllerMapping = val, value);
        }
    }
}