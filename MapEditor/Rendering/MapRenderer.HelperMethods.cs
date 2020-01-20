using Common.OpenGL;
using MapEditor.Models;
using MapEditor.Models.Elements.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MapEditor.Rendering
{
    public static partial class MapRenderer
    {
        private static int GetCPosition(int z) => z * MapRenderer.CurrentMap.Header.FloorHeight / 2;

        private static Color GetLevelColor(int z)
        {
            var colorByte = 255 << (8 * z);
            return Color.FromArgb((colorByte & 0xff0000) >> 16, (colorByte & 0xff00) >> 8, colorByte & 0xff);
        }

        private static Point GetImagePosition(Point mapPos, int c) => MapRenderer.GetImagePosition(mapPos.X, mapPos.Y, c);

        private static Point GetImagePosition(int a, int b, int c)
        {
            var w = MapRenderer.CurrentMap.Header.ImageSize.Width;
            var ox = MapRenderer.CurrentMap.Header.Offset.X;
            var oy = MapRenderer.CurrentMap.Header.Offset.Y;
            var x = (w + 4 * a - 4 * b) / 2 + ox;
            var y = a + b + c - 8 + oy;
            return new Point(x, y);
		}

		private static Point GetCharacterImagePosition(Point mapPos, int c) => MapRenderer.GetCharacterImagePosition(mapPos.X, mapPos.Y, c);

		private static Point GetCharacterImagePosition(int a, int b, int c) => MapRenderer.OffsetPoint(MapRenderer.GetImagePosition(a, b, c), 0, -20);

		private static Point OffsetPoint(Point p, int x, int y) => new Point(p.X + x, p.Y + y);

		private static Point GetMapPosition(Point imagePos, int c) => MapRenderer.GetMapPosition(imagePos.X, imagePos.Y, c);

        private static Point GetMapPosition(int x, int y, int c)
        {
            var w = MapRenderer.CurrentMap.Header.ImageWidth;
            var ox = MapRenderer.CurrentMap.Header.Offset.X;
            var oy = MapRenderer.CurrentMap.Header.Offset.Y;
            var mapX = (x + 2 * y + 16) / 4 - (w + 4 * c + 4 * oy + 2 * ox) / 8;
            var mapY = (4 * y + w + 2 * ox + 32) / 8 - (x + 2 * c + 2 * oy) / 4;
            return new Point(mapX, mapY);
        }

        private static Point GetTileMapPosition(Point mapPos, Func<double, double> RoundingFunc) => GetTileMapPosition(mapPos.X, mapPos.Y, RoundingFunc, RoundingFunc);

        private static Point GetTileMapPosition(int a, int b, Func<double, double> RoundingFunc) => GetTileMapPosition(a, b, RoundingFunc, RoundingFunc);

        private static Point GetTileMapPosition(Point mapPos, Func<double, double> XRoundingFunc, Func<double, double> YRoundingFunc) => GetTileMapPosition(mapPos.X, mapPos.Y, XRoundingFunc, YRoundingFunc);

        private static Point GetTileMapPosition(int a, int b, Func<double, double> XRoundingFunc, Func<double, double> YRoundingFunc)
        {
            return new Point((int)XRoundingFunc(a / 8.0), (int)YRoundingFunc(b / 8.0));
        }

        private static Point TileMapToMapPosition(Point mapPos) => TileMapToMapPosition(mapPos.X, mapPos.Y);
        private static Point TileMapToMapPosition(int a, int b)
        {
            return new Point(a * 8, b * 8);
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

        private static void Iterate2D(Point p1, Point p2, Action<int, int> act)
        {
            var rect = RectangleFromPoints(p1, p2);

            for (int x = rect.Left; x < rect.Right; x++)
            {
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    act.Invoke(x, y);
                }
            }
        }

        private static Rectangle RectangleFromPoints(Point p1, Point p2)
        {
            int leftX, rightX, topY, botY;
            if (p1.X < p2.X)
            {
                leftX = p1.X;
                rightX = p2.X;
            }
            else
            {
                leftX = p2.X;
                rightX = p1.X;
            }
            if (p1.Y < p2.Y)
            {
                topY = p1.Y;
                botY = p2.Y;
            }
            else
            {
                topY = p2.Y;
                botY = p1.Y;
            }

            return new Rectangle(leftX, topY, rightX - leftX, botY - topY);
        }

        private static double InvertFloor(double val) => Math.Floor(val) + 1;

        private static double InvertCeiling(double val) => Math.Ceiling(val) - 1;

        private static void DrawAndExpandMap(Sprite sprite, int renderPass)
        {
            MapRenderer.LevelRenderer.Draw(sprite, renderPass);
            if (!(sprite.Width == 0 && sprite.Height == 0))
            {
                MapRenderer.ExpandMap(new Point((int)sprite.X, (int)sprite.Y), MapRenderer.MapXBuffer, MapRenderer.MapYBuffer);
            }
        }

        private static void DrawLevelAndExpandMap(int x, int y, string texture, int renderPass)
        {
            MapRenderer.LevelRenderer.DrawLevel(x, y, texture, renderPass);
            MapRenderer.ExpandMap(new Point(MapRenderer.CurrentMap.Header.ImageSize), MapRenderer.MapXBuffer, MapRenderer.MapYBuffer);
        }

        private static void DrawQuadAndExpandMap(Quad q, int renderPass)
        {
            MapRenderer.LevelRenderer.DrawQuad(q, renderPass);
            if (!(q.A == q.B && q.A == q.C && q.A == q.D))
            {
                MapRenderer.ExpandMap(new Point(q.A.X, q.A.Y), MapRenderer.MapXBuffer, MapRenderer.MapYBuffer);
                MapRenderer.ExpandMap(new Point(q.B.X, q.B.Y), MapRenderer.MapXBuffer, MapRenderer.MapYBuffer);
                MapRenderer.ExpandMap(new Point(q.C.X, q.C.Y), MapRenderer.MapXBuffer, MapRenderer.MapYBuffer);
                MapRenderer.ExpandMap(new Point(q.D.X, q.D.Y), MapRenderer.MapXBuffer, MapRenderer.MapYBuffer);
            }
        }

        private static void ExpandMap(Point p, int xBuffer, int yBuffer)
        {
            if (!MapRenderer.WindowOrigin.HasValue || p.X < MapRenderer.WindowOrigin.Value.X)
            {
                MapRenderer.SetViewPort(new Point(p.X, MapRenderer.WindowOrigin?.Y ?? p.Y), new Size(MapRenderer.WindowSize?.Width ?? 0, MapRenderer.WindowSize?.Height ?? 0));
            }
            if (!MapRenderer.WindowOrigin.HasValue || !MapRenderer.WindowSize.HasValue || p.X > MapRenderer.WindowSize.Value.Width + MapRenderer.WindowOrigin.Value.X)
            {
                MapRenderer.SetViewPort(MapRenderer.WindowOrigin ?? p, new Size(p.X - MapRenderer.WindowOrigin?.X ?? (int)Math.Max(0, p.X), MapRenderer.WindowSize?.Height ?? (int)Math.Max(0, p.X)));
            }
            if (!MapRenderer.WindowOrigin.HasValue || p.Y < MapRenderer.WindowOrigin.Value.Y)
            {
                MapRenderer.SetViewPort(new Point(MapRenderer.WindowOrigin?.X ?? p.X, p.Y), new Size(MapRenderer.WindowSize?.Width ?? (int)Math.Max(0, p.X), MapRenderer.WindowSize?.Height ?? (int)Math.Max(0, p.Y)));
            }
            if (!MapRenderer.WindowOrigin.HasValue || !MapRenderer.WindowSize.HasValue || p.Y > MapRenderer.WindowSize.Value.Height + MapRenderer.WindowOrigin.Value.Y)
            {
                MapRenderer.SetViewPort(MapRenderer.WindowOrigin ?? p, new Size(MapRenderer.WindowSize?.Width ?? (int)Math.Max(0, p.X), p.Y - MapRenderer.WindowOrigin?.Y ?? (int)Math.Max(0, p.Y)));
            }
        }
    }
}
