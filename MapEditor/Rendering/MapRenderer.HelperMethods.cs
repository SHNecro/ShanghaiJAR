using Common.OpenGL;
using Common.Vectors;
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

        public static IEnumerable<MapObject> RendSort(IEnumerable<MapObject> mapObjects)
        {
            var levelObjects = GetSortedLevelObjects(mapObjects);

            return levelObjects.SelectMany(kvp => kvp.Value);
        }

        private static Dictionary<int, MapObject[]> GetSortedLevelObjects(IEnumerable<MapObject> mapObjects)
        {
            var unsortedLevels = mapObjects.GroupBy(mo => mo.Level);

            var sortedLevels = new Dictionary<int, MapObject[]>();

            foreach (var levelObjects in unsortedLevels)
            {
                var objects = new List<MapObject>();

                var rendTypes = levelObjects.GroupBy(mo => mo.Pages.SelectedEventPage.RendType).OrderBy(gr => gr.Key);
                foreach (var rendType in rendTypes)
                {
                    var sortedRendType = TopologicalRenderSort(rendType.ToList());

                    objects.AddRange(sortedRendType);
                }
                sortedLevels[levelObjects.Key] = objects.ToArray();
            }

            return sortedLevels;
        }

        private static IList<MapObject> TopologicalRenderSort(IList<MapObject> unsorted)
        {
            var graph = Enumerable.Range(0, unsorted.Count).ToDictionary(i => i, i => new List<int>());
            for (var i = 0; i < unsorted.Count; i++)
            {
                for (var ii = i+1; ii < unsorted.Count; ii++)
                {
                    var sortValue = IsBehind(unsorted[i], unsorted[ii]);
                    // If algorithm supports order-doesn't-matter, null value means no edge
                    if (sortValue == true)
                    {
                        graph[ii].Add(i);
                    }
                    else if (sortValue == false)
                    {
                        graph[i].Add(ii);
                    }
                }
            }

            var sortedNodeIndices = new List<int>();

            var lowValues = new Dictionary<int, int>();
            var discoveryDepth = 0;

            Action<int> recursiveTarjan = i => { };
            recursiveTarjan = (i) =>
            {
                lowValues[i] = discoveryDepth;
                foreach (var child in graph[i])
                {
                    if (!lowValues.ContainsKey(child))
                    {
                        discoveryDepth++;
                        recursiveTarjan(child);
                    }
                    lowValues[i] = Math.Min(lowValues[i], lowValues[child]);
                }
                sortedNodeIndices.Add(i);
            };

            var unsortedNode = graph.FirstOrDefault(kvp => !lowValues.ContainsKey(kvp.Key));
            while (unsortedNode.Value != default(List<int>))
            {
                recursiveTarjan(unsortedNode.Key);
                unsortedNode = graph.FirstOrDefault(kvp => !lowValues.ContainsKey(kvp.Key));
            }

            return sortedNodeIndices.Select(i => unsorted[i]).ToList();
        }

        private static bool? IsBehind(MapObject mo1, MapObject mo2)
        {
            if (mo1 == mo2) return null;

            // REQUIRES TOPOLOGICAL SORT
            var page1 = mo1.Pages?.SelectedEventPage;
            var page2 = mo2.Pages?.SelectedEventPage;

            Vector2 center1 = new Vector2(mo1.MapPosition.X, mo1.MapPosition.Y), size1 = Vector2.Zero;
            Vector2 center2 = new Vector2(mo2.MapPosition.X, mo2.MapPosition.Y), size2 = Vector2.Zero;

            if (page1 != null)
            {
                if (!page1.IsCharacter && (page1.TexW == 0 || page1.TexH == 0)) return null;

                center1 += new Vector2(page1.HitShift.X, page1.HitShift.Y);
                var width1 = (page1.HitForm == HitFormType.Square ? page1.HitRange.Width : page1.HitRange.Width * 2);
                var height1 = (page1.HitForm == HitFormType.Square ? page1.HitRange.Height : page1.HitRange.Height * 2);
                size1 = new Vector2(width1, height1);
                if (page1.HitForm == HitFormType.Square)
                {
                    center1 += new Vector2(-8, -8);
                }
                if (mo1 is MapMystery)
                {
                    center1 += new Vector2(-10, -10);
                }
            }

            if (page2 != null)
            {
                if (!page2.IsCharacter && (page2.TexW == 0 || page2.TexH == 0)) return null;

                center2 += new Vector2(page2.HitShift.X, page2.HitShift.Y);
                var width2 = (page2.HitForm == HitFormType.Square ? page2.HitRange.Width : page2.HitRange.Width * 2);
                var height2 = (page2.HitForm == HitFormType.Square ? page2.HitRange.Height : page2.HitRange.Height * 2);
                size2 = new Vector2(width2, height2);
                if (page2.HitForm == HitFormType.Square)
                {
                    center2 += new Vector2(-8, -8);
                }
                if (mo2 is MapMystery)
                {
                    center2 += new Vector2(-10, -10);
                }
            }

            var front1 = center1 + size1 / 2;
            var back1 = center1 - size1 / 2;

            var front2 = center2 + size2 / 2;
            var back2 = center2 - size2 / 2;

            var rect1 = RectangleF.FromLTRB(back1.X, back1.Y, front1.X, front1.Y);
            var rect2 = RectangleF.FromLTRB(back2.X, back2.Y, front2.X, front2.Y);

            // Hitboxes do not intersect
            if (!rect1.IntersectsWith(rect2))
            {
                var screenFront1 = ToScreenPosition(front1);
                var screenPosition1 = ToScreenPosition(new Vector2(mo1.X, mo1.Y));
                var leftmost1 = front1 - new Vector2(size1.X, 0);
                var rightmost1 = front1 - new Vector2(0, size1.Y);
                var screenLeftmost1 = ToScreenPosition(leftmost1);
                var screenRightmost1 = ToScreenPosition(rightmost1);

                var screenFront2 = ToScreenPosition(front2);
                var screenPosition2 = ToScreenPosition(new Vector2(mo2.X, mo2.Y));
                var leftmost2 = front2 - new Vector2(size2.X, 0);
                var rightmost2 = front2 - new Vector2(0, size2.Y);
                var screenLeftmost2 = ToScreenPosition(leftmost2);
                var screenRightmost2 = ToScreenPosition(rightmost2);

                // If 2 is to the right of 1 entirely
                if (screenLeftmost2.X >= screenRightmost1.X)
                {
                    var graphicRightSideWidth1 = (page1.IsCharacter ? 64 : page1.TexW) / 4;
                    var graphicLeftSideWidth2 = (page2.IsCharacter ? 64 : page2.TexW) / 4;

                    // Only check if sprite extends past hitbox (fixed length for effects, which have no defined width)
                    // Prevents mis-sorting when Tarjan's algorithm 'cuts' a cycle and an off-screen object is "in front"
                    if (screenPosition1.X + graphicRightSideWidth1 > screenPosition2.X - graphicLeftSideWidth2)
                    {
                        return screenLeftmost2.Y > screenRightmost1.Y;
                    }

                    return null;
                }

                // If 2 is to the left of 1 entirely
                if (screenRightmost2.X <= screenLeftmost1.X)
                {
                    var graphicLeftSideWidth1 = (page1.IsCharacter ? 64 : page1.TexW) / 4;
                    var graphicRightSideWidth2 = (page2.IsCharacter ? 64 : page2.TexW) / 4;

                    if (screenPosition2.X + graphicRightSideWidth2 > screenPosition1.X - graphicLeftSideWidth1)
                    {
                        return screenLeftmost2.Y > screenRightmost1.Y;
                    }

                    return null;
                }

                // If objects overlap in screen-Y, trivial since boxes do not intersect
                if (back1.X >= front2.X) return false;
                if (back2.X >= front1.X) return true;

                if (back1.Y >= front2.Y) return false;
                if (back2.Y >= front1.Y) return true;

                // Impossible case, does not intersect
                return null;
            }
            else
            {
                // If completely within, compare center to diagonal
                var rect1InRect2 = rect2.Contains(rect1);
                var rect2InRect1 = rect1.Contains(rect2);
                if (rect1InRect2 || rect2InRect1)
                {
                    return ToScreenPosition(center1).Y < ToScreenPosition(center2).Y;
                    //Broken: If 3 objects A contains B contains C, and A is long/B is wide, B can be in front of A, but C is in front of B but behind A
                    //var containerCenter = rect1InRect2 ? center1 : center2;
                    //var containerSize = rect1InRect2 ? size1 : size2;
                    //var containedCenter = rect1InRect2 ? center2 : center1;
                    //// https://stackoverflow.com/questions/1560492/how-to-tell-whether-a-point-is-to-the-right-or-left-side-of-a-line
                    //var ab = (containerCenter + (new Vector2(containerSize.X, -containerSize.Y) / 2)) - containerCenter;
                    //var am = containedCenter - containerCenter;

                    //return (ab.X * am.Y) - (ab.Y * am.X) > 0;
                }

                if (rect1.Contains(new PointF(back2.X, back2.Y)))
                {
                    // If back but not front inside (otherwise would be completely inside), is in front
                    return true;
                }
                else
                {
                    // Only left point inside
                    if (rect1.Contains(new PointF(back2.X, front2.Y)))
                    {
                        var rightmostRect1 = ToScreenPosition(front1 - new Vector2(0, size1.Y));
                        var leftmostRect2 = ToScreenPosition(front2 - new Vector2(size2.X, 0));
                        return rightmostRect1.Y < leftmostRect2.Y;
                    }
                    // Check not needed, all other cases handled (front is outside, back is inside, left not inside, is intersecting)
                    else // if (rect1.Contains(new PointF(front2.X, back2.Y)))
                    {
                        var leftmostRect1 = ToScreenPosition(front1 - new Vector2(size1.X, 0));
                        var rightmostRect2 = ToScreenPosition(front2 - new Vector2(0, size2.Y));
                        return leftmostRect1.Y < rightmostRect2.Y;
                    }
                }
            }
        }

        private static Vector2 ToScreenPosition(Vector2 gamePosition)
        {
            return new Vector2(gamePosition.X - gamePosition.Y, (gamePosition.X + gamePosition.Y) / 2);
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
