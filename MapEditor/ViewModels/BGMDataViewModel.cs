﻿using Common.OpenAL;
using MapEditor.Core;
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
using System.Windows;
using System.Windows.Input;

namespace MapEditor.ViewModels
{
    public class BGMDataViewModel : StringRepresentation
    {
        private const string FilePath = "music/looppoint.txt";
        private const string MusicPathFormat = "music/{0}.ogg";

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

        public BGMDataViewModel()
        {
            this.audio = new AudioEngine();
            this.audio.OggPlayback += this.OggPlayback;
            this.audio.Volume = 0.5f;

            this.isLooping = true;
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
                }

                this.SetValue(ref this.bgm, value);

                this.BGM.CollectionChanged += this.BGMCollectionChanged;

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
                        this.audio.OggStop();
                        this.PlayingBGM = this.SelectedBGM;
                        var filePath = string.Format(MusicPathFormat, this.PlayingBGM.File);
                        this.audio.InitializeOgg(filePath);
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
                return this.audio.Volume;
            }
            set
            {
                this.audio.Volume = value;
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

        public void LoadFromFile()
        {
            var loopPointContents = File.ReadAllText(FilePath, Encoding.GetEncoding("Shift_JIS"));
            this.SetStringValue(loopPointContents);
        }

        public void Remove()
        {
            this.audio.OggStop();
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
            this.AddChildErrors(null, newBgm);

            var originalSelectedBGM = this.SelectedBGM?.StringValue;
            this.BGM = new ObservableCollection<BGMViewModel>(newBgm);
            this.SelectedBGM = this.BGM.FirstOrDefault(bgm => bgm.StringValue == originalSelectedBGM) ?? bgm.FirstOrDefault();

            this.originalStringValue = this.StringValue;
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
                this.audio.PlayOggCommand(filePath, this.IsLooping, this.PlayingBGM.LoopStart, this.PlayingBGM.LoopEnd);
                this.isPaused = false;
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
            this.audio.InitializeOgg(filePath);
        }

        private void Save()
        {
            File.WriteAllText(FilePath, this.StringValue, Encoding.GetEncoding("Shift_JIS"));
            this.originalStringValue = this.StringValue;
            this.OnPropertyChanged(nameof(this.IsDirty));
            this.OnPropertyChanged(nameof(this.CanSave));
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
                    this.OggProgress = e;
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

            this.OnPropertyChanged(nameof(this.BGM));
            this.OnPropertyChanged(nameof(this.IsDirty));
        }
    }
}
