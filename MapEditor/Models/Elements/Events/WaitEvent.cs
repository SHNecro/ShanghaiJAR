namespace MapEditor.Models.Elements.Events
{
    public class WaitEvent : EventBase
    {
        private int waitFrames;
        private bool isBlocking;

        public int WaitFrames
        {
            get { return this.waitFrames; }
            set { this.SetValue(ref this.waitFrames, value); }
        }

        public bool IsBlocking
        {
            get { return this.isBlocking; }
            set { this.SetValue(ref this.isBlocking, value); }
        }

        public override string Info => "Pauses execution of events for the given number of frames, optionally blocking user input.";

        public override string Name
        {
            get
            {
                var isBlockingString = this.IsBlocking ? " (Block Player)" : string.Empty;
                return $"Wait: {this.WaitFrames} frames{isBlockingString}";
            }
        }

        protected override string GetStringValue()
        {
            var isBlockingString = this.IsBlocking ? "True" : "False";
            return $"wait:{this.WaitFrames}:{isBlockingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed wait event \"{value}\".", e => e.Length == 3 && e[0] == "wait"))
            {
                return;
            }

            var newWaitFrames = this.ParseIntOrAddError(entries[1]);
            var newIsBlocking = this.ParseBoolOrAddError(entries[2]);

            this.WaitFrames = newWaitFrames;
            this.IsBlocking = newIsBlocking;
        }
    }
}
