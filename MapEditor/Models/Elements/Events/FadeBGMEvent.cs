namespace MapEditor.Models.Elements.Events
{
    public class FadeBGMEvent : EventBase
    {
        private int endVolume;
        private int fadeTime;
        private bool isWaiting;

        public int EndVolume
        {
            get { return this.endVolume; }
            set { this.SetValue(ref this.endVolume, value); }
        }

        public int FadeTime
        {
            get { return this.fadeTime; }
            set { this.SetValue(ref this.fadeTime, value); }
        }

        public bool IsWaiting
        {
            get { return this.isWaiting; }
            set { this.SetValue(ref this.isWaiting, value); }
        }

        public override string Info => "Fades the volume of the BGM, optionally waiting until fade completes before the next event.";

        public override string Name
        {
            get
            {
                var isWaitingString = this.IsWaiting ? " (Wait)" : string.Empty;
                return $"Fade BGM: {this.EndVolume}% over {this.FadeTime} frames{isWaitingString}";
            }
        }

        protected override string GetStringValue()
        {
            var isWaitingString = this.IsWaiting ? "True" : "False";
            return $"bgmfade:{this.EndVolume}:{this.FadeTime}:{isWaitingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed BGM fade event \"{value}\".", e => e.Length == 4 && e[0] == "bgmfade"))
            {
                return;
            }

            var newEndVolume = this.ParseIntOrAddError(entries[1], () => this.EndVolume, (ev) => ev >= 0 && ev <= 100, (ev) => $"Invalid ending volume {ev} (0-100)");
            var newFadeTime = this.ParseIntOrAddError(entries[2], () => this.FadeTime, (ft) => ft >= 0, (ft) => $"Invalid fade time {ft} (>= 0)");
            var newIsWaiting = this.ParseBoolOrAddError(entries[3]);

            this.EndVolume = newEndVolume;
            this.FadeTime = newFadeTime;
            this.IsWaiting = newIsWaiting;
        }
    }
}
