﻿using SlimDX;
using System.Drawing;

namespace NSShanghaiEXE.ExtensionMethods
{
	public static class PointRectangleExtensionMethods
	{
		public static Point WithOffset(this Point origin, int xOff, int yOff)
		{
			return new Point(origin.X + xOff, origin.Y + yOff);
		}

		public static Rectangle WithOffset(this Rectangle origin, int xOff, int yOff)
		{
			return new Rectangle(origin.X + xOff, origin.Y + yOff, origin.Width, origin.Height);
		}

		public static Vector2 ToBattleScreenPosition(this Point origin, int xOff, int yOff)
		{
			return new Vector2(origin.X * 40 + xOff, origin.Y * 24 + yOff);
		}

		public static Vector2 ToBattleScreenPosition(this Point origin) => origin.ToBattleScreenPosition(0, 0);
	}
}
