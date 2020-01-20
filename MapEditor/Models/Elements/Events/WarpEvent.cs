﻿using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;
using MapEditor.ViewModels;

namespace MapEditor.Models.Elements.Events
{
    public class WarpEvent : EventBase
    {
        private string targetMap;
        private int x;
        private int y;
        private int z;
        private int angle;

        public static WarpEvent ConvertedFromEvent(EventBase eventBase)
        {
            if (eventBase is JackInDirectEvent jide)
            {
                return new WarpEvent { TargetMap = jide.TargetMap, X = jide.X, Y = jide.Y, Z = jide.Z, Angle = (int)AngleTypeNumber.North };
            }
            if (eventBase is JackInEvent jie)
            {
                return new WarpEvent { TargetMap = jie.TargetMap, X = jie.X, Y = jie.Y, Z = jie.Z, Angle = jie.Angle };
            }
            if (eventBase is MapChangeEvent mce)
            {
                var adjustedAngle = mce.Angle;
                switch (mce.Angle)
                {
                    case (int)AngleTypeNumber.NorthEast:
                    case (int)AngleTypeNumber.NorthWest:
                    case (int)AngleTypeNumber.SouthEast:
                    case (int)AngleTypeNumber.SouthWest:
                        adjustedAngle = (int)AngleTypeNumber.SouthEast;
                        break;
                }
                return new WarpEvent { TargetMap = mce.TargetMap, X = mce.X, Y = mce.Y, Z = mce.Z, Angle = adjustedAngle };
            }
            if (eventBase is MapWarpEvent mwe)
            {
                return new WarpEvent { TargetMap = mwe.TargetMap, X = mwe.X, Y = mwe.Y, Z = mwe.Z, Angle = mwe.Angle };
            }

            return new WarpEvent { TargetMap = MainWindowViewModel.GetCurrentMap().Name, X = 0, Y = 0, Z = 0, Angle = (int)AngleTypeNumber.North };
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

        public override string Info => "Warp to the position on the given map, fading out and moving 16 units.";

        public override string Name
        {
            get
            {
                var angleString = this.Angle == (int)AngleTypeNumber.SouthEast ? "None" : new EnumDescriptionTypeConverter(typeof(AngleTypeNumber)).ConvertToString((AngleTypeNumber)this.Angle);
                return $"Warp (Out of Map): \"{this.TargetMap}\" ({this.X},{this.Y},{this.Z}) ({angleString})";
            }
        }

        protected override string GetStringValue()
        {
            return $"warp:{this.TargetMap}:{this.X}:{this.Y}:{this.Z}:{this.Angle}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed warp event \"{value}\".", e => e.Length == 6 && e[0] == "warp"))
            {
                return;
            }

            var newTargetMap = entries[1];
            var newX = this.ParseIntOrAddError(entries[2]);
            var newY = this.ParseIntOrAddError(entries[3]);
            var newZ = this.ParseIntOrAddError(entries[4], z => z >= 0, z => $"Invalid floor \"{z}\" (>= 0)");
            var newAngle = this.ParseIntOrAddError(entries[5]);

            if (!this.HasErrors)
            {
                this.TargetMap = newTargetMap;
                this.X = newX;
                this.Y = newY;
                this.Z = newZ;
                this.Angle = newAngle;
            }
        }
    }
}
