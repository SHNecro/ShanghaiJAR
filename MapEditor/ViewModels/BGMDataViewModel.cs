using Common.OpenAL;
using MapEditor.Core;
using MapEditor.ExtensionMethods;
using MapEditor.Models.Elements;
using Microsoft.WindowsAPICodePack.Dialogs;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class BGMDataViewModel : StringRepresentation
    {
        private const string FilePath = "music/looppoint.txt";
        private const string MusicPathFormat = "music/{0}.ogg";

        public static ObservableCollection<BGMDefinition> BGMDefinitions;

        static BGMDataViewModel()
        {
            var loopPointContents = File.ReadAllText(FilePath, Encoding.GetEncoding("Shift_JIS"));
            BGMDataViewModel.Instance = new BGMDataViewModel { StringValue = loopPointContents };

            BGMDataViewModel.BGMDefinitions = new ObservableCollection<BGMDefinition>();
            foreach (var bgmViewModel in BGMDataViewModel.Instance.BGM)
            {
                BGMDataViewModel.BGMDefinitions.Add(new BGMDefinition { Name = bgmViewModel.Name, File = bgmViewModel.File });
            }
        }

        public static BGMDataViewModel Instance { get; }

        private readonly AudioEngine audio;

        private ObservableCollection<BGMViewModel> bgm;
        private BGMViewModel selectedBGM;

        private bool isPlaying;
        private bool isLooping;
        private OggPlaybackEventArgs oggProgress;
        private BGMViewModel playingBGM;
        private bool isPaused;
        private bool nextAudioStopIsPause;

        private int lastSelectedIndex;

        private string originalStringValue;

        private bool oggDataLoaded;
        private bool playingWhenSeek;
        private Timer seekPlayDelay;

        private BGMDataViewModel()
        {
            this.audio = AudioEngine.Instance;
            this.audio.OggPlayback += this.OggPlayback;
            this.audio.SetVolume(0.5f);

            this.isLooping = true;

            this.seekPlayDelay = new Timer { Interval = 400, AutoReset = false, Enabled = false };
            this.seekPlayDelay.Elapsed += (sender, args) =>
            {
                if (!this.IsPlaying && this.playingWhenSeek)
                {
                    this.PlayPause();
                }
                this.playingWhenSeek = false;
            };
        }

        public ObservableCollection<BGMViewModel> BGM
        {
            get
            {
                return this.bgm;
            }
            set
            {
                if (this.BGM != null)
                {
                    this.BGM.CollectionChanged -= this.BGMCollectionChanged;

                    foreach (var bgmViewModel in this.BGM)
                    {
                        bgmViewModel.PropertyChanged -= this.BGMViewModelPropertyChanged;
                    }
                }

                this.SetValue(ref this.bgm, value);

                this.BGM.CollectionChanged += this.BGMCollectionChanged;
                foreach (var bgmViewModel in this.BGM)
                {
                    bgmViewModel.PropertyChanged += this.BGMViewModelPropertyChanged;
                }

                this.SelectedBGM = this.BGM.FirstOrDefault();
            }
        }

        public BGMViewModel SelectedBGM
        {
            get
            {
                return this.selectedBGM;
            }
            set
            {
                if (value != null || this.BGM.Count == 0)
                {
                    this.SetValue(ref this.selectedBGM, value);

                    if (!this.IsPlaying && !this.isPaused && this.SelectedBGM != null)
                    {
                        this.oggDataLoaded = false;
                        this.seekPlayDelay.Stop();
                        this.audio.OggStop();
                        this.PlayingBGM = this.SelectedBGM;
                        var filePath = string.Format(MusicPathFormat, this.PlayingBGM.File);
                        this.audio.OggInitialize(filePath);
                    }

                    this.lastSelectedIndex = this.BGM.IndexOf(this.SelectedBGM);
                }
            }
        }

        public bool IsPlaying
        {
            get { return this.isPlaying; }
            set { this.SetValue(ref this.isPlaying, value); }
        }

        public OggPlaybackEventArgs OggProgress
        {
            get
            {
                return this.oggProgress;
            }
            set
            {
                this.SetValue(ref this.oggProgress, value);
            }
        }

        public bool IsLooping
        {
            get { return this.isLooping; }
            set { this.SetValue(ref this.isLooping, value); }
        }

        public float Volume
        {
            get
            {
                return this.audio.GetVolume(AudioEngine.DefaultVolumeGroup);
            }
            set
            {
                this.audio.SetVolume(value, AudioEngine.DefaultVolumeGroup);
                this.OnPropertyChanged(nameof(this.Volume));
            }
        }

        public BGMViewModel PlayingBGM
        {
            get { return this.playingBGM; }
            set { this.SetValue(ref this.playingBGM, value); }
        }

        public bool IsDirty => this.StringValue != this.originalStringValue;

        public bool CanSave => this.IsDirty && !this.BGM.Any(bgm => !bgm.CanSave);

        public ICommand PlayPauseCommand => new RelayCommand(this.PlayPause);

        public ICommand StopCommand => new RelayCommand(this.Stop);

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand UndoCommand => new RelayCommand(this.Undo);

        public ICommand AddBGMEntryCommand => new RelayCommand(this.AddBGMEntry);

        public void Remove()
        {
            this.audio.OggStop();
        }

        public void SeekToPercent(double percent)
        {
            if (this.IsPlaying)
            {
                this.playingWhenSeek = true;
                this.PlayPause();
            }

            this.seekPlayDelay.Stop();
            this.seekPlayDelay.Start();

            var progressSamples = (long)(percent * this.OggProgress.TotalSamples);

            this.audio.OggSeek(progressSamples);

            if (!this.oggDataLoaded)
            {
                var filePath = string.Format(MusicPathFormat, this.PlayingBGM.File);
                AudioEngine.LoadOggInfo(filePath, out long sampleRate, out long totalSamples);
                this.OggProgress = new OggPlaybackEventArgs
                {
                    ProgressSamples = progressSamples,
                    SampleRate = sampleRate,
                    TotalSamples = totalSamples
                };

                this.oggDataLoaded = true;
            }
            else
            {
                this.OggProgress = new OggPlaybackEventArgs
                {
                    ProgressSamples = progressSamples,
                    SampleRate = this.OggProgress.SampleRate,
                    TotalSamples = this.OggProgress.TotalSamples
                };
            }
        }

        protected override string GetStringValue()
        {
            return string.Join("\r\n", this.BGM.Select(re => re.StringValue));
        }

        protected override void SetStringValue(string value)
        {
            var newBgm = value.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Where(res => res != string.Empty)
                .Select(res => new BGMViewModel { StringValue = res }).ToList();

            var originalSelectedBGM = this.SelectedBGM?.StringValue;
            this.BGM = new ObservableCollection<BGMViewModel>(newBgm);
            this.SelectedBGM = this.BGM.FirstOrDefault(bgm => bgm.StringValue == originalSelectedBGM) ?? bgm.FirstOrDefault();

            this.originalStringValue = this.StringValue;
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            if (this.BGM == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }
            else
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>(this.BGM.SelectMany(sr => this.UpdateChildErrorStack(sr)));
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            base.OnPropertyChanged(nameof(this.IsDirty));
            base.OnPropertyChanged(nameof(this.CanSave));
        }

        private void PlayPause()
        {
            if (!this.IsPlaying)
            {
                var filePath = string.Format(MusicPathFormat, this.PlayingBGM.File);
                this.audio.OggPlay(filePath, this.IsLooping, this.PlayingBGM.LoopStart, this.PlayingBGM.LoopEnd);
                this.isPaused = false;
                this.oggDataLoaded = true;
            }
            else
            {
                this.audio.OggPause();
                this.nextAudioStopIsPause = true;
            }

            this.IsPlaying = !this.IsPlaying;
        }

        private void Stop()
        {
            this.audio.OggStop();

            this.isPaused = false;
            this.PlayingBGM = this.SelectedBGM;
            var filePath = string.Format(MusicPathFormat, this.PlayingBGM.File);
            this.audio.OggInitialize(filePath);
        }

        private void Save()
        {
            File.WriteAllText(FilePath, this.StringValue, Encoding.GetEncoding("Shift_JIS"));
            this.originalStringValue = this.StringValue;
            this.OnPropertyChanged(nameof(this.IsDirty));
            this.OnPropertyChanged(nameof(this.CanSave));

            while (BGMDataViewModel.BGMDefinitions.Any())
            {
                BGMDataViewModel.BGMDefinitions.RemoveAt(0);
            }

            foreach (var bgmViewModel in this.BGM)
            {
                BGMDataViewModel.BGMDefinitions.Add(new BGMDefinition { Name = bgmViewModel.Name, File = bgmViewModel.File });
            }
        }

        private void Undo()
        {
            this.StringValue = this.originalStringValue;
            this.OnPropertyChanged(nameof(this.IsDirty));
            this.OnPropertyChanged(nameof(this.CanSave));
        }

        private void AddBGMEntry()
        {
            try
            {
                var musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), Path.GetDirectoryName(MusicPathFormat));

                var selectFileDialog = new CommonOpenFileDialog
                {
                    RestoreDirectory = false,
                    InitialDirectory = Directory.GetCurrentDirectory() + "\\music\\",
                    EnsureFileExists = true,
                    Multiselect = false,
                    Title = "Add .ogg"
                };
                selectFileDialog.Filters.Add(new CommonFileDialogFilter("Ogg Vorbis File", "*.ogg"));
                selectFileDialog.Filters.Add(new CommonFileDialogFilter("All Files", "*.*"));

                var dialogSuccess = selectFileDialog.ShowDialog();
                if (dialogSuccess == CommonFileDialogResult.Ok)
                {
                    AudioEngine.LoadOggInfo(selectFileDialog.FileName, out _, out long totalSamples);
                    var fileName = Path.GetFileNameWithoutExtension(selectFileDialog.FileName);
                    var newBGM = new BGMViewModel { StringValue = $"{0},{totalSamples},{fileName},{fileName}" };

                    var selectedBGMIndex = this.BGM.IndexOf(this.SelectedBGM);
                    this.BGM.Insert(selectedBGMIndex, newBGM);

                    this.OnPropertyChanged(nameof(this.IsDirty));
                    this.OnPropertyChanged(nameof(this.CanSave));
                }
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show(Application.Current.MainWindow, e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OggPlayback(object sender, OggPlaybackEventArgs e)
        {
            switch (e.StateChange)
            {
                case ALSourceState.Initial:
                case null:
                    if (!this.seekPlayDelay.Enabled)
                    {
                        this.OggProgress = e;
                    }
                    break;
                case ALSourceState.Stopped:
                    this.IsPlaying = false;
                    if (this.nextAudioStopIsPause)
                    {
                        this.isPaused = true;
                        this.nextAudioStopIsPause = false;
                    }
                    break;
            }
        }

        private void BGMCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this.BGM.Contains(this.SelectedBGM))
            {
                this.SelectedBGM = this.lastSelectedIndex < this.BGM.Count && this.lastSelectedIndex >= 0
                    ? this.BGM[this.lastSelectedIndex] : this.BGM.LastOrDefault();
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newObject in e.NewItems)
                {
                    var newMessage = newObject as MessageViewModel;
                    if (newMessage != null)
                    {
                        newMessage.PropertyChanged += this.BGMViewModelPropertyChanged;
                    }
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldObject in e.OldItems)
                {
                    var oldMessage = oldObject as MessageViewModel;
                    if (oldMessage != null)
                    {
                        oldMessage.PropertyChanged -= this.BGMViewModelPropertyChanged;
                    }
                }
            }

            this.OnPropertyChanged(nameof(this.BGM));
            this.OnPropertyChanged(nameof(this.IsDirty));
        }

        private void BGMViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.IsDirty));
        }
    }
}
