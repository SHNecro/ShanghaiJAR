using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class SEOnEvent : EventBase
    {
        private EffectSoundType soundEffect;

        public EffectSoundType SoundEffect
        {
            get { return this.soundEffect; }
            set { this.SetValue(ref this.soundEffect, value); }
        }

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

            var newSoundEffect = this.ParseEnumOrAddError<EffectSoundType>(entries[1]);

            if (!this.HasErrors)
            {
                this.SoundEffect = newSoundEffect;
            }
        }
    }
}
