using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.EncodeDecode;
using Common.OpenAL;
using NSShanghaiEXE.InputOutput.Audio.MIDI;
using NSShanghaiEXE.InputOutput.Audio.XAudio2;

namespace NSShanghaiEXE.InputOutput.Audio.OpenAL
{
    public class OpenALAudio : IAudioEngine
    {
        private const string FilePath = "music/looppoint.txt";
        private const string MusicPathFormat = "music/{0}.ogg";

        private const string BGMVolumeGroup = "bgm";
        private const string SEVolumeGroup = "se";

        private readonly AudioEngine audio;
        private readonly PianoNotePlayer pianoNotePlayer;

        private List<OggData> bgmList;
        private List<OggData> bgmListAlternate;
        private ISoundLoadStrategy loadStrategy;

        private float baseVolume;
        private float percentVolume = 100;
        private int fadeDuration = -1;
        private int fadeCurrentProgress = -1;
        private float fadeInitialVolume;
        private float fadeEndVolume;

        public event EventHandler<AudioLoadProgressUpdatedEventArgs> ProgressUpdated;

        public OpenALAudio(float soundEffectVolume, string tcdFile, string password, string graphicsFormat)
        {
            this.audio = AudioEngine.Instance;
            this.audio.SetVolume(0.5f, OpenALAudio.BGMVolumeGroup);
            this.audio.SetVolume(soundEffectVolume, OpenALAudio.SEVolumeGroup);

            this.pianoNotePlayer = new PianoNotePlayer();

            var loopPointContents = File.ReadAllText(FilePath, Encoding.GetEncoding("Shift_JIS"));

            //TODO: 
            this.bgmListAlternate = new List<OggData>();

            this.bgmList = loopPointContents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Where(res => res != string.Empty)
                .Select(res => ReadOggData(res)).ToList();

            this.loadStrategy = new TCDLoadStrategy(tcdFile, password, graphicsFormat);

            var loadThread = new Thread(new ThreadStart(this.Init));
            loadThread.Start();
        }

        public bool MusicPlay { get; private set; }

        public string CurrentBGM { get; private set; }

        public float BGMVolume
        {
            get
            {
                return this.baseVolume;
            }

            set
            {
                this.baseVolume = value;
                this.UpdateAudioBGMVolume(true);
            }
        }

        public float SoundEffectVolume
        {
            get
            {
                return this.audio.GetVolume(OpenALAudio.SEVolumeGroup);
            }

            set
            {
                this.audio.SetVolume(value, OpenALAudio.SEVolumeGroup);
            }
        }

        public bool IsPlayingNote => this.pianoNotePlayer.IsPlayingNote;

        public void BGMFade()
        {
            this.UpdateCurrentFadeBGMVolume();
        }

        public void BGMFadeStart(int flame, int endparsent)
        {
            this.fadeDuration = flame;
            this.fadeCurrentProgress = flame;
            this.fadeInitialVolume = this.percentVolume;
            this.fadeEndVolume = endparsent;

            if (this.fadeDuration == 0)
            {
                this.percentVolume = endparsent;
                this.UpdateCurrentFadeBGMVolume();
                this.UpdateAudioBGMVolume(true);
            }
        }

        public void BGMVolumeSet(int volume)
        {
            this.BGMFadeStart(0, volume);
        }

        public void Dispose()
        {
            this.audio.Dispose();
            this.pianoNotePlayer.Dispose();
        }

        public void Init()
        {
            this.loadStrategy.ProgressUpdated += this.Load_ProgressUpdate;
            this.loadStrategy.Load();
        }

        public void PlayingMusic()
        {
            this.UpdateAudioBGMVolume(false);
        }

        public void PlayNote(Note note, int volume, int tickDuration)
        {
            var adjustedVolume = (int)Math.Min(Math.Round(volume * this.SoundEffectVolume * 4), 127);
            this.pianoNotePlayer.PlayNote(note, adjustedVolume, tickDuration);
        }

        public void PlaySE(SoundEffect sname)
        {
            this.audio.WavStop(sname.ToString());
            this.audio.WavPlay(this.loadStrategy.ProvideSound(sname.ToString()), SEVolumeGroup, sname.ToString());
        }

        public void ReStartBGM()
        {
            this.MusicPlay = true;
        }

        public void SetBGM(string name)
        {
            this.StartBGM(name);
        }

        public void StartBGM(string name)
        {
            if (name == this.CurrentBGM)
            {
                return;
            }

            this.StopBGM();

            this.CurrentBGM = name;

            this.MusicPlay = true;

            var bgmEntry = this.bgmListAlternate.FirstOrDefault(o => Path.GetFileNameWithoutExtension(o.File) == name)
                ?? this.bgmList.FirstOrDefault(o => Path.GetFileNameWithoutExtension(o.File) == name);

            if (bgmEntry != null)
            {
                // TODO: race condition? does not start if turbo, needs time to init?
                this.audio.OggPlay(Path.Combine("music", bgmEntry.File + ".ogg"), true, bgmEntry.LoopStart, bgmEntry.LoopEnd, BGMVolumeGroup);
            }
        }

        public void StopBGM()
        {
            this.CurrentBGM = "none_";
            this.MusicPlay = false;
            this.BGMVolumeSet(100);

            this.audio.OggStop();
        }

        public void StopSE(SoundEffect sname)
        {
            this.audio.WavStop(sname.ToString());
        }

        public void UpdateNoteTick()
        {
            this.pianoNotePlayer.UpdateNoteTick();
        }

        private void UpdateCurrentFadeBGMVolume()
        {
            if (this.fadeDuration < 0)
            {
                return;
            }

            if (this.fadeCurrentProgress <= 0)
            {
                this.fadeDuration = -1;
                this.fadeCurrentProgress = -1;
                this.fadeEndVolume = this.percentVolume;
                this.fadeInitialVolume = this.percentVolume;
            }
            else
            {
                this.fadeCurrentProgress--;
            }
        }

        private void UpdateAudioBGMVolume(bool forceUpdate)
        {
            if (this.fadeDuration >= 0)
            {
                var progress = this.fadeDuration == 0 ? 1f : (1 - (float)this.fadeCurrentProgress / this.fadeDuration);
                this.percentVolume = this.fadeInitialVolume + (this.fadeEndVolume - this.fadeInitialVolume) * progress;
            }

            if (this.fadeDuration >= 0 || forceUpdate)
            {
                this.audio.SetVolume((this.BGMVolume / 100f) * (this.percentVolume / 100f), BGMVolumeGroup);
            }
        }

        private void Load_ProgressUpdate(object sender, LoadProgressUpdatedEventArgs e)
        {
            if (e == null)
            {
                this.ProgressUpdated?.Invoke(this, null);
                ((TCDLoadStrategy)sender).ProgressUpdated -= this.Load_ProgressUpdate;
            }
            else
            {
                this.ProgressUpdated?.Invoke(this, new AudioLoadProgressUpdatedEventArgs(e.UpdateLabel, e.UpdateProgress));
            }
        }

        private static OggData ReadOggData(string loopPointEntry)
        {
            var oggData = new OggData();

            var entries = loopPointEntry.Split(',');
            if (!Validate(entries, "Malformed BGM entry.", e => e.Length == 4))
            {
                return null;
            }

            var newLoopStart = ParseLongOrAddError(entries[0], () => oggData.LoopStart, i => i >= 0, (i) => "Loop start must be >= 0");
            var newLoopEnd = ParseLongOrAddError(entries[1], () => oggData.LoopEnd, i => i >= 0, (i) => "Loop end must be >= 0");
            var newName = entries[2];
            var newFile = entries[3];
            var filePath = Path.Combine("music", newFile + ".ogg");

            Validate(newFile, () => oggData.File, s => $"Missing bgm file {s}", s => System.IO.File.Exists($"music/{s}.ogg"));

            long totalSamples = 0;
            try
            {
                AudioEngine.LoadOggInfo(filePath, out _, out totalSamples);
                Validate(newLoopStart, () => oggData.LoopStart, $"Loop start past loop end or .ogg sample range {totalSamples}", l => l <= newLoopEnd && l <= totalSamples);
                Validate(newLoopEnd, () => oggData.LoopEnd, $"Loop end before loop start or past .ogg sample range {totalSamples}", l => l >= newLoopStart && l <= totalSamples);
            }
            catch (InvalidOperationException)
            {
                Validate(
                    newFile,
                    () => Path.Combine("music", oggData.File + ".ogg"),
                    s => $"Invalid .ogg file \"{s}\"",
                    s =>
                    {
                        try
                        {
                            AudioEngine.LoadOggInfo(Path.Combine("music", s + ".ogg"), out _, out totalSamples);
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    });
            }
            
            oggData.LoopStart = newLoopStart;
            oggData.LoopEnd = newLoopEnd;

            Validate(
                0,
                () => 0,
                val => "",
                val => newLoopEnd >= Math.Max(0, newLoopStart) && newLoopEnd <= totalSamples &&
                newLoopStart >= 0 && newLoopStart <= Math.Min(newLoopEnd, totalSamples));

            oggData.Name = newName;
            oggData.File = newFile;

            return oggData;
        }

        private static bool Validate<TVar>(TVar value, Func<TVar> getterFunc, Func<TVar, string> errorFunc, Func<TVar, bool> validateFunc)
        {
            if (validateFunc(value))
            {
                return true;
            }
            else
            {
                MessageBox.Show(errorFunc(value), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                throw new InvalidOperationException(errorFunc(value));
            }
        }
        private static bool Validate<TVar>(TVar value, Func<TVar> getterFunc, string error, Func<TVar, bool> validateFunc)
            => Validate(value, getterFunc, (s) => error, validateFunc);

        private static bool Validate<TVar>(TVar value, Func<TVar, string> errorFunc, Func<TVar, bool> validateFunc)
            => Validate(value, null, errorFunc, validateFunc);

        private static bool Validate<TVar>(TVar value, string error, Func<TVar, bool> validateFunc)
            => Validate(value, (s) => error, validateFunc);

        private static long ParseLongOrAddError(string s, Func<long> getterFunc, Func<long, bool> validateFunc, Func<long, string> errorFunc)
        {
            if (long.TryParse(s, out long parsed))
            {
                if (validateFunc == null || validateFunc(parsed))
                {
                    return parsed;
                }
                else
                {
                    MessageBox.Show((errorFunc ?? (i => $"Invalid parameter \"{i}\""))(parsed), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                    throw new InvalidOperationException(errorFunc(parsed));
                }
            }
            else
            {
                MessageBox.Show($"Failed to parse \"{s}\" as long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                throw new InvalidOperationException(errorFunc(parsed));
            }
        }

        private static long ParseLongOrAddError(string s) => ParseLongOrAddError(s, null, null, null);
        private static long ParseLongOrAddError(string s, Func<long, bool> validateFunc, Func<long, string> errorFunc) => ParseLongOrAddError(s, null, validateFunc, errorFunc);
    }
}
