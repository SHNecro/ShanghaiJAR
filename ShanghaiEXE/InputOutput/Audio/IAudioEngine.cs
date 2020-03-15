using NSShanghaiEXE.InputOutput.Audio.XAudio2;
using System;

namespace NSShanghaiEXE.InputOutput.Audio
{
    public interface IAudioEngine : IDisposable
    {
        event EventHandler<AudioLoadProgressUpdatedEventArgs> ProgressUpdated;

        bool MusicPlay { get; }
        string CurrentBGM { get; }
        float BGMVolume { get; set; }
        float SoundEffectVolume { get; set; }

        void BGMFade();
        void BGMFadeStart(int flame, int endparsent);
        void BGMVolumeSet(int volume);
        void Init();
        void PlayingMusic();
        void PlaySE(SoundEffect sname);
        void ReStartBGM();
        void SetBGM(string name);
        void StartBGM(string name);
        void StopBGM();
        void StopSE(SoundEffect sname);

        void PlayNote(Note note, int volume, int frameDuration);
        void UpdateNoteTick();
        bool IsPlayingNote { get; }
    }
}
