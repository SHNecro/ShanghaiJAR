using MapEditor.Models;
using MapEditor.Models.Elements.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MapEditor.Rendering
{
    public static partial class MapSelectionRenderer
    {
        private static int GetCPosition(int z) => z * MapSelectionRenderer.CurrentMap.Header.FloorHeight / 2;

        private static Color GetLevelColor(int z)
        {
            var colorByte = 255 << (8 * z);
            return Color.FromArgb((colorByte & 0xff0000) >> 16, (colorByte & 0xff00) >> 8, colorByte & 0xff);
        }

        private static Point GetImagePosition(Point mapPos, int c) => MapSelectionRenderer.GetImagePosition(mapPos.X, mapPos.Y, c);

        private static Point GetImagePosition(int a, int b, int c)
        {
            var w = MapSelectionRenderer.CurrentMap.Header.ImageSize.Width;
            var ox = MapSelectionRenderer.CurrentMap.Header.Offset.X;
            var oy = MapSelectionRenderer.CurrentMap.Header.Offset.Y;
            var x = (w + 4 * a - 4 * b) / 2 + ox;
            var y = a + b + c - 8 + oy;
            return new Point(x, y);
        }

        private static Point GetCharacterImagePosition(Point mapPos, int c) => MapSelectionRenderer.GetCharacterImagePosition(mapPos.X, mapPos.Y, c);

        private static Point GetCharacterImagePosition(int a, int b, int c) => MapSelectionRenderer.OffsetPoint(MapSelectionRenderer.GetImagePosition(a, b, c), 0, -20);

        private static Point OffsetPoint(Point p, int x, int y) => new Point(p.X + x, p.Y + y);

        private static Point GetMapPosition(Point imagePos, int c) => MapSelectionRenderer.GetMapPosition(imagePos.X, imagePos.Y, c);

        private static Point GetMapPosition(int x, int y, int c)
        {
            var w = MapSelectionRenderer.CurrentMap.Header.ImageWidth;
            var ox = MapSelectionRenderer.CurrentMap.Header.Offset.X;
            var oy = MapSelectionRenderer.CurrentMap.Header.Offset.Y;
            var mapX = (x + 2 * y + 16) / 4 - (w + 4 * c + 4 * oy + 2 * ox) / 8;
            var mapY = (4 * y + w + 2 * ox + 32) / 8 - (x + 2 * c + 2 * oy) / 4;
            return new Point(mapX, mapY);
        }
    }
}
