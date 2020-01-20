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

        private static IEnumerable<MapObject> RendSort(IEnumerable<MapObject> mapObjects)
        {
            //foreach (MapEffect mapEffect in this.effect)
            //	circleObjects.Add(mapEffect);
            //circleObjects.Add(parent.Player);
            List<MapObject> orderedCircleObjects = mapObjects.Where(o => o.Pages.SelectedEventPage != null && o.Pages.SelectedEventPage.HitForm == HitFormType.Circle).OrderBy(c => c.X + c.Y).ToList();
            List<MapObject> orderedSquareObjects = mapObjects.Where(o => o.Pages.SelectedEventPage != null && o.Pages.SelectedEventPage.HitForm != HitFormType.Circle).OrderBy(c => c.X + c.Y).ToList();
            var objectIndex = orderedCircleObjects.Select((x, i) => new Tuple<int, MapObject>(i, x)).ToDictionary(t => new Tuple<int, int>(t.Item1, 0), t => t.Item2);
            foreach (MapObject mapObject in orderedSquareObjects)
            {
                int placedIndex = orderedCircleObjects.Count;
                for (int i = 0; i < orderedCircleObjects.Count; ++i)
                {
                    if (ObjectLine(mapObject, orderedCircleObjects[i]))
                    {
                        placedIndex = i;
                        break;
                    }
                }
                var bucketNum = 0;
                while (objectIndex.ContainsKey(new Tuple<int, int>(placedIndex, bucketNum)))
                {
                    bucketNum++;
                }
                objectIndex[new Tuple<int, int>(placedIndex, bucketNum)] = mapObject;
            }
            return objectIndex.OrderBy(kvp => kvp.Key.Item1).ThenBy(kvp => kvp.Key.Item2).Select(kvp => kvp.Value);
        }

        private static bool ObjectLine(MapObject lowerObject, MapObject higherObject)
        {
            if (lowerObject.Level < higherObject.Level)
                return true;
            if (lowerObject.Level > higherObject.Level)
                return false;
            MapEventPage lowerObjectPage = lowerObject.Pages.SelectedEventPage;
            if (lowerObjectPage.HitRange.Width == 0)
                return true;
            double num1 = -(lowerObjectPage.HitRange.Width / lowerObjectPage.HitRange.Width);
            double num2 = lowerObject.Y + lowerObjectPage.HitShift.Y - num1 * (lowerObject.X + lowerObjectPage.HitShift.X);
            return higherObject.X * num1 + num2 < higherObject.Y;
        }
    }
}
