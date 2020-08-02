using MapEditor.Core;
using MapEditor.Models.Elements.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Input;

namespace MapEditor.Models.Elements.Events
{
    public class SEOnEvent : EventBase
    {
        private readonly Timer reselectTimer;

        private string soundEffect;

        public SEOnEvent()
        {
            this.reselectTimer = new Timer { Interval = 100, AutoReset = false, Enabled = false };
            this.reselectTimer.Elapsed += this.ReloadSoundEffects;
        }

        public string SoundEffect
        {
            get
            {
                return this.soundEffect;
            }
            set
            {
                if (value == null)
                {
                    this.reselectTimer.Stop();
                    this.reselectTimer.Start();
                }
                else
                {
                    this.SetValue(ref this.soundEffect, value);
                }
            }
        }

        public ICommand PlayCommand => new RelayCommand(this.PlaySoundEffectCommand);

        public ICommand StopCommand => new RelayCommand(this.StopSoundEffectCommand);

        public override string Info => "Plays a sound effect.";

        public override string Name => $"Sound: {this.SoundEffect}";

        protected override string GetStringValue()
        {
            return $"seon:{this.SoundEffect}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed sound effect event \"{value}\".", e => e.Length == 2 && e[0] == "seon"))
            {
                return;
            }

            var newSoundEffect = entries[1];
            this.Validate(newSoundEffect, () => this.SoundEffect, s => $"Sound \"{s}.wav\" not found", s => s == "none" || (Constants.SoundLoadStrategy?.CanProvideFile(s) ?? true));

            this.SoundEffect = newSoundEffect;
        }

        private void PlaySoundEffectCommand()
        {
            if (this.SoundEffect == "none")
            {
                return;
            }

            Constants.AudioEngine.WavPlay(Constants.SoundLoadStrategy.ProvideSound(this.SoundEffect));
        }

        private void StopSoundEffectCommand()
        {
            Constants.AudioEngine.WavStop();
        }

        private void ReloadSoundEffects(object sender, ElapsedEventArgs args)
        {
            var originalSoundEffect = this.SoundEffect;
            this.SoundEffect = Constants.SoundEffects.LastOrDefault();
            this.SoundEffect = Constants.SoundEffects[Constants.SoundEffects.IndexOf(originalSoundEffect)];
        }
    }
}
