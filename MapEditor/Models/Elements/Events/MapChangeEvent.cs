using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;
using MapEditor.ViewModels;

namespace MapEditor.Models.Elements.Events
{
    public class MapChangeEvent : EventBase
    {
        private string targetMap;
        private int x;
        private int y;
        private int z;
        private int angle;

        public static MapChangeEvent ConvertedFromEvent(EventBase eventBase)
        {
            if (eventBase is JackInDirectEvent jide)
            {
                return new MapChangeEvent { TargetMap = jide.TargetMap, X = jide.X, Y = jide.Y, Z = jide.Z, Angle = (int)AngleTypeNumber.SouthEast };
            }
            if (eventBase is JackInEvent jie)
            {
                return new MapChangeEvent { TargetMap = jie.TargetMap, X = jie.X, Y = jie.Y, Z = jie.Z, Angle = jie.Angle };
            }
            if (eventBase is MapWarpEvent mwe)
            {
                return new MapChangeEvent { TargetMap = mwe.TargetMap, X = mwe.X, Y = mwe.Y, Z = mwe.Z, Angle = mwe.Angle };
            }
            if (eventBase is WarpEvent we)
            {
                return new MapChangeEvent { TargetMap = we.TargetMap, X = we.X, Y = we.Y, Z = we.Z, Angle = we.Angle };
            }

            return new MapChangeEvent { TargetMap = MainWindowViewModel.GetCurrentMap().Name, X = 0, Y = 0, Z = 0, Angle = (int)AngleTypeNumber.SouthEast };
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

        public int Angle
        {
            get { return this.angle; }
            set { this.SetValue(ref this.angle, value); }
        }

        public override string Info => "Go to the position on the given map with no effects.";

        public override string Name
        {
            get
            {
                var angleString = new EnumDescriptionTypeConverter(typeof(AngleTypeNumber)).ConvertToString((AngleTypeNumber)this.Angle);
                return $"Map Change: \"{this.TargetMap}\" ({this.X},{this.Y},{this.Z}) ({angleString})";
            }
        }

        protected override string GetStringValue()
        {
            return $"mapChange:{this.TargetMap}:{this.X}:{this.Y}:{this.Z}:{this.Angle}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed map change event \"{value}\".", e => e.Length == 6 && e[0] == "mapChange"))
            {
                return;
            }

            var newTargetMap = entries[1];
            var newX = this.ParseIntOrAddError(entries[2]);
            var newY = this.ParseIntOrAddError(entries[3]);
            var newZ = this.ParseIntOrAddError(entries[4], () => this.Z, z => z >= 0, z => $"Invalid floor \"{z}\" (>= 0)");
            var newAngle = this.ParseIntOrAddError(entries[5]);

            this.TargetMap = newTargetMap;
            this.X = newX;
            this.Y = newY;
            this.Z = newZ;
            this.Angle = newAngle;
        }
    }
}
