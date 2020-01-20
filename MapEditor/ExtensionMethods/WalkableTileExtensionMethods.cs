using MapEditor.Models.Elements.Enums;
using System;
using System.Drawing;

namespace MapEditor.ExtensionMethods
{
    public static class WalkableTileExtensionMethods
    {
        public static Point ToTileOffset(this WalkableTileType tile)
        {
            switch (tile)
            {
                case WalkableTileType.RightRamp:
                case WalkableTileType.LeftRightRampTop:
                    return new Point(1, 0);
                case WalkableTileType.DownRamp:
                case WalkableTileType.UpDownRampTop:
                    return new Point(0, 1);
                case WalkableTileType.LeftRamp:
                case WalkableTileType.LeftRightRampBottom:
                    return new Point(-1, 0);
                case WalkableTileType.UpRamp:
                case WalkableTileType.UpDownRampBottom:
                    return new Point(0, -1);
                default:
                    return new Point(0, 0);
            }
        }

		public static bool IsDualStair(this WalkableTileType tile)
		{
			switch (tile)
			{
				case WalkableTileType.LeftRightRampTop:
				case WalkableTileType.UpDownRampTop:
				case WalkableTileType.LeftRightRampBottom:
				case WalkableTileType.UpDownRampBottom:
					return true;
				default:
					return false;
			}
		}

		public static int ToDualStairLevelOffset(this WalkableTileType tile)
		{
			switch (tile)
			{
				case WalkableTileType.LeftRightRampTop:
				case WalkableTileType.UpDownRampTop:
					return -1;
				case WalkableTileType.LeftRightRampBottom:
				case WalkableTileType.UpDownRampBottom:
					return 1;
				default:
					return 0;
			}
		}

		public static Point ToDualStairTileOffset(this WalkableTileType tile, int floorHeight)
		{
			var rampLength = (int)Math.Ceiling(floorHeight / 16.0);
			switch (tile)
			{
				case WalkableTileType.LeftRightRampTop:
					return new Point(rampLength, 0);
				case WalkableTileType.UpDownRampTop:
					return new Point(0, rampLength);
				case WalkableTileType.LeftRightRampBottom:
					return new Point(-rampLength, 0);
				case WalkableTileType.UpDownRampBottom:
					return new Point(0, -rampLength);
				default:
					return new Point(0, 0);
			}
		}

		public static WalkableTileType ToDualStairStart(this WalkableTileType tile)
		{
			switch (tile)
			{
				case WalkableTileType.LeftRightRampTop:
					return WalkableTileType.RightRamp;
				case WalkableTileType.UpDownRampTop:
					return WalkableTileType.DownRamp;
				case WalkableTileType.LeftRightRampBottom:
					return WalkableTileType.LeftRamp;
				case WalkableTileType.UpDownRampBottom:
					return WalkableTileType.UpRamp;
				default:
					return WalkableTileType.Empty;
			}
		}

		public static WalkableTileType ToDualStairEnd(this WalkableTileType tile)
		{
			switch (tile)
			{
				case WalkableTileType.LeftRightRampTop:
					return WalkableTileType.LeftRamp;
				case WalkableTileType.UpDownRampTop:
					return WalkableTileType.UpRamp;
				case WalkableTileType.LeftRightRampBottom:
					return WalkableTileType.RightRamp;
				case WalkableTileType.UpDownRampBottom:
					return WalkableTileType.DownRamp;
				default:
					return WalkableTileType.Empty;
			}
		}
	}
}
