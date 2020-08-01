using System.Windows.Media;

namespace MapEditor.Models.Elements.Events
{
    public class FadeEvent : EventBase
    {
        private int fadeTime;
        private Color color;
        private bool isWaiting;

        public int FadeTime
        {
            get { return this.fadeTime; }
            set { this.SetValue(ref this.fadeTime, value); }
        }

        public Color Color
        {
            get { return this.color; }
            set { this.SetValue(ref this.color, value); }
        }
        public byte A
        {
            get { return this.color.A; }
            set
            {
                this.Color = Color.FromArgb(value, this.color.R, this.color.G, this.color.B);
            }
        }

        public byte R
        {
            get { return this.color.R; }
            set
            {
                this.Color = Color.FromArgb(this.color.A, value, this.color.G, this.color.B);
            }
        }

        public byte G
        {
            get { return this.color.G; }
            set
            {
                this.Color = Color.FromArgb(this.color.A, this.color.R, value, this.color.B);
            }
        }

        public byte B
        {
            get { return this.color.B; }
            set
            {
                this.Color = Color.FromArgb(this.color.A, this.color.R, this.color.G, value);
            }
        }

        public bool IsWaiting
        {
            get { return this.isWaiting; }
            set { this.SetValue(ref this.isWaiting, value); }
        }

        public override string Info => "Applies an overlay to the screen, optionally waiting until fade completes before the next event.";

        public override string Name
        {
            get
            {
                var colorString = this.Color.ToString();
                var isWaitingString = this.IsWaiting ? " (Wait)" : string.Empty;
                return $"Fade Color: {colorString} over {this.FadeTime} frames{isWaitingString}";
            }
        }

        protected override string GetStringValue()
        {
            var isWaitingString = this.IsWaiting ? "True" : "False";
            return $"fade:{this.FadeTime}:{this.A}:{this.R}:{this.G}:{this.B}:{isWaitingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed screen fade event \"{value}\".", e => e.Length == 7 && e[0] == "fade"))
            {
                return;
            }

            var newFadeTime = this.ParseIntOrAddError(entries[1], () => this.FadeTime, (ft) => ft >= 0, (ft) => $"Invalid fade time {ft} (>= 0)");

            var newA = this.ParseIntOrAddError(entries[2], () => this.A, (b) => b >= 0 && b <= 255, (b) => $"Invalid alpha value {b} (0 - 255)");
            var newR = this.ParseIntOrAddError(entries[3], () => this.R, (b) => b >= 0 && b <= 255, (b) => $"Invalid red value {b} (0 - 255)");
            var newG = this.ParseIntOrAddError(entries[4], () => this.G, (b) => b >= 0 && b <= 255, (b) => $"Invalid green value {b} (0 - 255)");
            var newB = this.ParseIntOrAddError(entries[5], () => this.B, (b) => b >= 0 && b <= 255, (b) => $"Invalid blue value {b} (0 - 255)");

            var newIsWaiting = this.ParseBoolOrAddError(entries[6]);

            this.FadeTime = newFadeTime;
            this.A = (byte)newA;
            this.R = (byte)newR;
            this.G = (byte)newG;
            this.B = (byte)newB;
            this.IsWaiting = newIsWaiting;
        }
    }
}
