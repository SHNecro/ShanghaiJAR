namespace MapEditor.Models.Elements.Events
{
    public class ShakeEvent : EventBase
    {
        private int magnitude;
        private int durationFrames;

        public int Magnitude
        {
            get { return this.magnitude; }
            set { this.SetValue(ref this.magnitude, value); }
        }

        public int DurationFrames
        {
            get { return this.durationFrames; }
            set { this.SetValue(ref this.durationFrames, value); }
        }

        public override string Info => "Applies screenshake for a number of frames or indefinitely, or ends screenshake.";

        public override string Name
        {
            get
            {
                var durationString = this.DurationFrames <= 0 ? "(indefinite)" : $"({this.DurationFrames} frames)";
                return $"Shake: Magnitude {this.Magnitude} {durationString}";
            }
        }

        protected override string GetStringValue()
        {
            return $"shake:{this.Magnitude}:{this.DurationFrames}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed screenshake event \"{value}\".", e => e.Length == 3 && e[0] == "shake"))
            {
                return;
            }

            var newMagnitude = this.ParseIntOrAddError(entries[1], () => this.Magnitude, m => m >= 1, m => $"Invalid magnitude {m} (>= 1)");
            var newDurationFrames = this.ParseIntOrAddError(entries[2], () => this.DurationFrames, df => df >= 0, df => $"Invalid duration {df} (>= 0)");

            this.Magnitude = newMagnitude;
            this.DurationFrames = newDurationFrames;
        }
    }
}
