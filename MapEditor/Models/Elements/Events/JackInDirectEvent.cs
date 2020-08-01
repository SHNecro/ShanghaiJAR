using MapEditor.ViewModels;

namespace MapEditor.Models.Elements.Events
{
    public class JackInDirectEvent : EventBase
    {
        private string targetMap;
        private int x;
        private int y;
        private int z;

        public static JackInDirectEvent ConvertedFromEvent(EventBase eventBase)
        {
            if (eventBase is JackInEvent jie)
            {
                return new JackInDirectEvent { TargetMap = jie.TargetMap, X = jie.X, Y = jie.Y, Z = jie.Z };
            }
            if (eventBase is MapChangeEvent mce)
            {
                return new JackInDirectEvent { TargetMap = mce.TargetMap, X = mce.X, Y = mce.Y, Z = mce.Z };
            }
            if (eventBase is MapWarpEvent mwe)
            {
                return new JackInDirectEvent { TargetMap = mwe.TargetMap, X = mwe.X, Y = mwe.Y, Z = mwe.Z };
            }
            if (eventBase is WarpEvent we)
            {
                return new JackInDirectEvent { TargetMap = we.TargetMap, X = we.X, Y = we.Y, Z = we.Z };
            }

            return new JackInDirectEvent { TargetMap = MainWindowViewModel.GetCurrentMap().Name, X = 0, Y = 0, Z = 0 };
        }

        public string TargetMap
        {
            get { return this.targetMap; }
            set { this.SetValue(ref this.targetMap, value); }
        }

        public int X
        {
            get { return this.x; }
            set { this.SetValue(ref this.x, value); }
        }

        public int Y
        {
            get { return this.y; }
            set { this.SetValue(ref this.y, value); }
        }

        public int Z
        {
            get { return this.z; }
            set { this.SetValue(ref this.z, value); }
        }

        public override string Info => "Jack in to position on the given map, without any effects or initial movement. Unknown behavior if already jacked in.";

        public override string Name => $"Jack in: \"{this.TargetMap}\" ({this.X},{this.Y},{this.Z}) (Silent)";

        protected override string GetStringValue()
        {
            return $"plugInNO:{this.TargetMap}:{this.X}:{this.Y}:{this.Z}:0";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed silent jack in event \"{value}\".", e => e.Length == 6 && e[0] == "plugInNO"))
            {
                return;
            }

            var newTargetMap = entries[1];
            var newX = this.ParseIntOrAddError(entries[2]);
            var newY = this.ParseIntOrAddError(entries[3]);
            var newZ = this.ParseIntOrAddError(entries[4], () => this.Z, z => z >= 0, z => $"Invalid floor \"{z}\" (>= 0)");

            this.TargetMap = newTargetMap;
            this.X = newX;
            this.Y = newY;
            this.Z = newZ;
        }
    }
}
