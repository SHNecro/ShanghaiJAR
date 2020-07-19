using MapEditor.Core;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        #region General Properties
        public string InitialMap
        {
            get { return LoadingWindowViewModel.Settings.InitialMap; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.InitialMap, (val) => LoadingWindowViewModel.Settings.InitialMap = val, value); }
        }
        public string MapDataFolder
        {
            get { return LoadingWindowViewModel.Settings.MapDataFolder; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.MapDataFolder, (val) => LoadingWindowViewModel.Settings.MapDataFolder = val, value); }
        }

        public string GraphicsFormat
        {
            get { return LoadingWindowViewModel.Settings.GraphicsFormat; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.GraphicsFormat, (val) => LoadingWindowViewModel.Settings.GraphicsFormat = val, value); }
        }
        public string GraphicsResourceFile
        {
            get { return LoadingWindowViewModel.Settings.GraphicsResourceFile; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.GraphicsResourceFile, (val) => LoadingWindowViewModel.Settings.GraphicsResourceFile = val, value); }
        }
        public string GraphicsResourceFilePassword
        {
            get { return LoadingWindowViewModel.Settings.GraphicsResourceFilePassword; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.GraphicsResourceFilePassword, (val) => LoadingWindowViewModel.Settings.GraphicsResourceFilePassword = val, value); }
        }
        public string GraphicsResourceFileFormat
        {
            get { return LoadingWindowViewModel.Settings.GraphicsResourceFileFormat; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.GraphicsResourceFileFormat, (val) => LoadingWindowViewModel.Settings.GraphicsResourceFileFormat = val, value); }
        }
        public bool GraphicsIsPackedResource
        {
            get { return LoadingWindowViewModel.Settings.GraphicsIsPackedResource; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.GraphicsIsPackedResource, (val) => LoadingWindowViewModel.Settings.GraphicsIsPackedResource = val, value); }
        }

        public string SoundFormat
        {
            get { return LoadingWindowViewModel.Settings.SoundFormat; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.SoundFormat, (val) => LoadingWindowViewModel.Settings.SoundFormat = val, value); }
        }
        public string SoundResourceFile
        {
            get { return LoadingWindowViewModel.Settings.SoundResourceFile; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.SoundResourceFile, (val) => LoadingWindowViewModel.Settings.SoundResourceFile = val, value); }
        }
        public string SoundResourceFilePassword
        {
            get { return LoadingWindowViewModel.Settings.SoundResourceFilePassword; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.SoundResourceFilePassword, (val) => LoadingWindowViewModel.Settings.SoundResourceFilePassword = val, value); }
        }
        public string SoundResourceFileFormat
        {
            get { return LoadingWindowViewModel.Settings.SoundResourceFileFormat; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.SoundResourceFileFormat, (val) => LoadingWindowViewModel.Settings.SoundResourceFileFormat = val, value); }
        }
        public bool SoundIsPackedResource
        {
            get { return LoadingWindowViewModel.Settings.SoundIsPackedResource; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.SoundIsPackedResource, (val) => LoadingWindowViewModel.Settings.SoundIsPackedResource = val, value); }
        }
        #endregion

        #region Advanced Properties
        public int EnemyCount
        {
            get { return LoadingWindowViewModel.Settings.EnemyCount; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.EnemyCount, (val) => LoadingWindowViewModel.Settings.EnemyCount = val, value); }
        }
        public int ChipCount
        {
            get { return LoadingWindowViewModel.Settings.ChipCount; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.ChipCount, (val) => LoadingWindowViewModel.Settings.ChipCount = val, value); }
        }
        public int AddOnCount
        {
            get { return LoadingWindowViewModel.Settings.AddOnCount; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.AddOnCount, (val) => LoadingWindowViewModel.Settings.AddOnCount = val, value); }
        }
        public int InteriorCount
        {
            get { return LoadingWindowViewModel.Settings.InteriorCount; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.InteriorCount, (val) => LoadingWindowViewModel.Settings.InteriorCount = val, value); }
        }
        public int BackgroundCount
        {
            get { return LoadingWindowViewModel.Settings.BackgroundCount; }
            set { this.SetAndSaveValue(() => LoadingWindowViewModel.Settings.BackgroundCount, (val) => LoadingWindowViewModel.Settings.BackgroundCount = val, value); }
        }
        #endregion

        #region ViewModel Properties

        public ICommand ReloadGraphicsCommand => new RelayCommand(() => { MainWindowViewModel.GetInstance().ReloadGraphicsCommand.Execute(null); });

        public ICommand ReloadSoundCommand => new RelayCommand(() => { MainWindowViewModel.GetInstance().ReloadSoundCommand.Execute(null); });

        public ICommand CloseSettingsCommand => new RelayCommand(() => { SettingsWindow.HideWindow(); });

        #endregion

        public void RefreshSettings()
        {
            LoadingWindowViewModel.Settings = (Settings)LoadingWindowViewModel.Settings.Clone();

            this.OnPropertyChanged(nameof(this.InitialMap));
            this.OnPropertyChanged(nameof(this.MapDataFolder));
            this.OnPropertyChanged(nameof(this.GraphicsFormat));
            this.OnPropertyChanged(nameof(this.GraphicsResourceFile));
            this.OnPropertyChanged(nameof(this.GraphicsResourceFilePassword));
            this.OnPropertyChanged(nameof(this.GraphicsResourceFileFormat));
            this.OnPropertyChanged(nameof(this.GraphicsIsPackedResource));
            this.OnPropertyChanged(nameof(this.SoundFormat));
            this.OnPropertyChanged(nameof(this.SoundResourceFile));
            this.OnPropertyChanged(nameof(this.SoundResourceFilePassword));
            this.OnPropertyChanged(nameof(this.SoundResourceFileFormat));
            this.OnPropertyChanged(nameof(this.SoundIsPackedResource));

            this.OnPropertyChanged(nameof(this.EnemyCount));
            this.OnPropertyChanged(nameof(this.ChipCount));
            this.OnPropertyChanged(nameof(this.AddOnCount));
            this.OnPropertyChanged(nameof(this.InteriorCount));
            this.OnPropertyChanged(nameof(this.BackgroundCount));

        }

        private void SetAndSaveValue<T>(Func<T> getAction, Action<T> setAction, T value, [CallerMemberName] string propertyName = null)
        {
            this.SetValue<T>(getAction, setAction, value, propertyName);
            LoadingWindowViewModel.Settings.ToXML("option-editor.xml");
        }
    }
}
