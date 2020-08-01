namespace MapEditor.Models.Elements.Events
{
    public class CameraEvent : EventBase
    {
        private int x;
        private int y;
        private int moveTime;
        private bool isAbsolute;
        
        public int X
        {
            get
            {
                return this.x;
            }

            set
            {
                this.SetValue(ref this.x, value);
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }

            set
            {
                this.SetValue(ref this.y, value);
            }
        }

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

        public bool IsAbsolute
        {
            get
            {
                return this.isAbsolute;
            }

            set
            {
                this.SetValue(ref this.isAbsolute, value);
            }
        }

        public override string Info => "Moves the camera to a specified offset or position, over N frames.";

        public override string Name
        {
            get
            {
                var absoluteString = !this.IsAbsolute ? "+" : string.Empty;
                return $"Camera {absoluteString}({this.X},{this.Y}): {this.MoveTime} frames";
            }
        }

        protected override string GetStringValue()
        {
            var absoluteString = this.IsAbsolute ? "True" : "False";
            return $"camera:{this.X}:{this.Y}:{this.MoveTime}:{absoluteString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed camera event \"{value}\".", e => e.Length == 5 && e[0] == "camera"))
            {
                return;
            }

            var newTargetX = this.ParseIntOrAddError(entries[1]);
            var newTargetY = this.ParseIntOrAddError(entries[2]);
            var newMoveTime = this.ParseIntOrAddError(entries[3]);
            var newIsRelative = this.ParseBoolOrAddError(entries[4]);

            this.X = newTargetX;
            this.Y = newTargetY;
            this.MoveTime = newMoveTime;
            this.IsAbsolute = newIsRelative;
        }
    }
}
