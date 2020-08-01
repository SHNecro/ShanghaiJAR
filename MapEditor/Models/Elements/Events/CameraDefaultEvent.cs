namespace MapEditor.Models.Elements.Events
{
    public class CameraDefaultEvent : EventBase
    {
        private int moveTime;

        public int MoveTime
        {
            get
            {
                return this.moveTime;
            }

            set
            {
                this.SetValue(ref this.moveTime, value);
            }
        }

        public override string Info => "Returns the camera to the default position, over N frames.";

        public override string Name => $"Camera Default: {this.moveTime} frames";

        protected override string GetStringValue()
        {
            return $"CameraDefault:{this.MoveTime}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed camera default event \"{value}\".", e => e.Length == 2 && e[0] == "CameraDefault"))
            {
                return;
            }

            var newMoveTime = this.ParseIntOrAddError(entries[1]);

            this.MoveTime = newMoveTime;
        }
    }
}
