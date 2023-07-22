using MapEditor.Core;
using Common.OpenGL;
using Common.Vectors;
using MapEditor.ExtensionMethods;
using MapEditor.Models;
using MapEditor.Models.Elements;
using MapEditor.Models.Elements.Enums;
using MapEditor.Models.Elements.Events;
using MapEditor.Models.Elements.Terms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MapEditor.Rendering
{
    public static partial class MapRenderer
    {
        public static void DrawBackground()
        {
            var selectedBg = default(BackgroundDefinition);
            if (Constants.BackgroundDefinitions.TryGetValue(MapRenderer.CurrentMap.Header.BackgroundNumber, out selectedBg))
            {
                if (selectedBg.Size.IsEmpty)
                {
                    MapRenderer.LevelRenderer.SetClearColor(selectedBg.BackColor);
                }
                else
                {
                    var bgSize = selectedBg.Size;
                    var offsetX = selectedBg.ScrollSpeed.X == 0 ? 0 : (MapRenderer.Frame / selectedBg.ScrollSpeed.X) % bgSize.Width;
                    var offsetY = selectedBg.ScrollSpeed.Y == 0 ? 0 : (MapRenderer.Frame / selectedBg.ScrollSpeed.Y) % bgSize.Height;
                    var bgFrame = selectedBg.GetBGFrame(MapRenderer.Frame);

                    var backgroundTile = new Tile
                    {
                        OffsetX = offsetX,
                        OffsetY = offsetY,
                        Texture = selectedBg.PictureName,
                        TextureFrame = bgFrame,
                        TileWidth = bgSize.Width
                    };
                    MapRenderer.LevelRenderer.DrawTiledBackground(backgroundTile);
                }
            }
        }

        public static void DrawMapImages()
        {
            for (int i = 1; i < MapRenderer.CurrentMap.Header.Levels * 2; i++)
            {
                var levelIndex = i / 2;
                if (!MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowMapImages)
                {
                    continue;
                }

                var renderPass = RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels * 2) - (i % 2 == 0 ? i - 1 : i));
                MapRenderer.DrawLevelAndExpandMap(0, 0, $"{MapRenderer.CurrentMap.Header.ImagePrefix}{i}", renderPass);
            }
        }

        public static void DrawWalkable()
        {
            var size = 8;

            var combinedTiles = new Dictionary<Tuple<int, int, int, int>, Rectangle>();

            for (int z = MapRenderer.CurrentMap.Header.Levels - 1; z >= 0; z--)
            {
                var levelIndex = z;
                if (MapRenderer.CurrentTool != EditToolType.DrawTool && (!MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowWalkable))
                {
                    continue;
                }

                var renderPass = (RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels - z) * 2)) - (RenderPassPadding - 1);
                var zPosition = MapRenderer.GetCPosition(z);
                var color = MapRenderer.GetLevelColor(z);

                for (int y = 0; y < MapRenderer.CurrentMap.Header.WalkableSize.Height; y++)
                {
                    var top = size * (y);
                    var bottom = size * (y + 1);
                    Rectangle? currentRect = null;
                    for (int x = 0; x < MapRenderer.CurrentMap.Header.WalkableSize.Width; x++)
                    {
                        var left = size * (x);
                        var right = size * (x + 1);
                        var tileType = MapRenderer.CurrentMap.WalkableMap[z, y, x];
						var tuplePosition = new Tuple<int, int, int>(z, y, x);
						if (MapRenderer.WalkablePreview.ContainsKey(tuplePosition))
						{
							tileType = MapRenderer.WalkablePreview[tuplePosition];
						}

                        // Delayed creation to merge cells into larger rectangles
                        // For full size tiles, conveyors
                        if (tileType == 1 || tileType == 14 || tileType == 15 || tileType == 16 || tileType == 17)
                        {
                            currentRect = !currentRect.HasValue
                                ? new Rectangle { X = left, Width = size, Y = top, Height = size }
                                : new Rectangle { X = currentRect.Value.Left, Width = currentRect.Value.Width + size, Y = top, Height = size };
                        }
                        // Slanted Sections Majority (2: missing bottom-left, 3: missing top-right, 4: missing top-left, 5: missing bottom-right)
                        if (tileType == 2 || tileType == 3 || tileType == 4 || tileType == 5)
                        {
                            var quarterSize = (int)(size * 0.25);
                            var threeQuarterSize = (int)(size * 0.75);

                            var topRight = new[] { new Point(right, top), new Point(right, top) };
                            var bottomRight = new[] { new Point(right, bottom), new Point(right, bottom) };
                            var bottomLeft = new[] { new Point(left, bottom), new Point(left, bottom) };
                            var topLeft = new[] { new Point(left, top), new Point(left, top) };

                            switch (tileType)
                            {
                                case 2:
                                    bottomLeft[0] = topLeft[0];
                                    topRight[1] = bottomRight[1];
                                    bottomRight[1].X -= quarterSize;
                                    bottomLeft[1].Y -= threeQuarterSize;
                                    break;
                                case 3:
                                    topRight[0] = bottomRight[0];
                                    bottomLeft[1] = topLeft[1];
                                    topLeft[1].X += quarterSize;
                                    topRight[1].Y += threeQuarterSize;
                                    break;
                                case 4:
                                    topLeft[0] = topRight[0];
                                    bottomRight[1] = bottomLeft[1];
                                    bottomLeft[1].Y -= quarterSize;
                                    topLeft[1].X += threeQuarterSize;
                                    break;
                                case 5:
                                    bottomRight[0] = bottomLeft[0];
                                    topLeft[1] = topRight[1];
                                    topRight[1].Y += quarterSize;
                                    bottomRight[1].X -= threeQuarterSize;
                                    break;
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                MapRenderer.DrawQuadAndExpandMap(new Quad
                                {
                                    A = MapRenderer.GetImagePosition(topRight[i], zPosition),
                                    B = MapRenderer.GetImagePosition(bottomRight[i], zPosition),
                                    C = MapRenderer.GetImagePosition(bottomLeft[i], zPosition),
                                    D = MapRenderer.GetImagePosition(topLeft[i], zPosition),
                                    Color = Color.FromArgb(96, color),
                                    Type = DrawType.Fill
                                }, renderPass);
                            }
                        }
                        // Slanted Sections Fillers (6: bottom-left, 7: top-right, 8: top-left, 9: bottom-right)
                        if (tileType == 6 || tileType == 7 || tileType == 8 || tileType == 9)
                        {
                            var threeQuarterSize = (int)(size * 0.75);

                            var topRight = new Point(right, top);
                            var bottomRight = new Point(right, bottom);
                            var bottomLeft = new Point(left, bottom);
                            var topLeft = new Point(left, top);

                            switch (tileType)
                            {
                                case 6:
                                    bottomRight.Y -= threeQuarterSize;
                                    bottomLeft = bottomRight;
                                    topLeft.X += threeQuarterSize;
                                    break;
                                case 7:
                                    topLeft.Y += threeQuarterSize;
                                    topRight = topLeft;
                                    bottomRight.X -= threeQuarterSize;
                                    break;
                                case 8:
                                    bottomLeft.X += threeQuarterSize;
                                    topLeft = bottomLeft;
                                    topRight.Y += threeQuarterSize;
                                    break;
                                case 9:
                                    topRight.X -= threeQuarterSize;
                                    bottomRight = topRight;
                                    bottomLeft.Y -= threeQuarterSize;
                                    break;
                            }
                            MapRenderer.DrawQuadAndExpandMap(new Quad
                            {
                                A = MapRenderer.GetImagePosition(topRight, zPosition),
                                B = MapRenderer.GetImagePosition(bottomRight, zPosition),
                                C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
                                D = MapRenderer.GetImagePosition(topLeft, zPosition),
                                Color = Color.FromArgb(96, color),
                                Type = DrawType.Fill
                            }, renderPass);
                        }
                        // Ramps (10: down-east, 11: down-south, 12: up-west, 13: up-north)
                        if (tileType == 10 || tileType == 11 || tileType == 12 || tileType == 13)
                        {
                            var upperLevelZ = zPosition - MapRenderer.CurrentMap.Header.FloorHeight / 2;
                            var lowerLevelZ = zPosition + MapRenderer.CurrentMap.Header.FloorHeight / 2;
                            var shift = (MapRenderer.CurrentMap.Header.FloorHeight / 2) - size;

                            var topRight = new Point(right, top);
                            var bottomRight = new Point(right, bottom);
                            var bottomLeft = new Point(left, bottom);
                            var topLeft = new Point(left, top);

                            int topRightZPosition, bottomRightZPosition, bottomLeftZPosition, topLeftZPosition;
                            topRightZPosition = bottomRightZPosition = bottomLeftZPosition = topLeftZPosition = zPosition;

                            switch (tileType)
                            {
                                case 10:
                                    topRightZPosition = lowerLevelZ;
                                    bottomRightZPosition = lowerLevelZ;
                                    topRight.X += shift;
                                    bottomRight.X += shift;
                                    break;
                                case 11:
                                    bottomRightZPosition = lowerLevelZ;
                                    bottomLeftZPosition = lowerLevelZ;
                                    bottomRight.Y += shift;
                                    bottomLeft.Y += shift;
                                    break;
                                case 12:
                                    bottomLeftZPosition = upperLevelZ;
                                    topLeftZPosition = upperLevelZ;
                                    bottomLeft.X -= shift;
                                    topLeft.X -= shift;
                                    break;
                                case 13:
                                    topRightZPosition = upperLevelZ;
                                    topLeftZPosition = upperLevelZ;
                                    topRight.Y -= shift;
                                    topLeft.Y -= shift;
                                    break;
                            }

                            MapRenderer.DrawQuadAndExpandMap(new Quad
                            {
                                A = MapRenderer.GetImagePosition(topRight, topRightZPosition),
                                B = MapRenderer.GetImagePosition(bottomRight, bottomRightZPosition),
                                C = MapRenderer.GetImagePosition(bottomLeft, bottomLeftZPosition),
                                D = MapRenderer.GetImagePosition(topLeft, topLeftZPosition),
                                Color = Color.FromArgb(128, color),
                                Type = DrawType.Fill
                            }, tileType == 12 || tileType == 13 ? renderPass + (2 * RenderPassPadding) : renderPass);
                        }
                        // Conveyors (14: north, 15: south, 16: west, 17: east)
                        if (tileType == 14 || tileType == 15 || tileType == 16 || tileType == 17)
                        {
                            var halfSize = size / 2;
                            var topRight = new Point(tileType == 14 ? right - halfSize : right, tileType == 17 ? top + halfSize : top);
                            var bottomRight = new Point(tileType == 15 ? right - halfSize : right, tileType == 17 ? bottom - halfSize : bottom);
                            var bottomLeft = new Point(tileType == 15 ? left + halfSize : left, tileType == 16 ? bottom - halfSize : bottom);
                            var topLeft = new Point(tileType == 14 ? left + halfSize : left, tileType == 16 ? top + halfSize : top);
                            MapRenderer.DrawQuadAndExpandMap(new Quad
                            {
                                A = MapRenderer.GetImagePosition(topRight, zPosition),
                                B = MapRenderer.GetImagePosition(bottomRight, zPosition),
                                C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
                                D = MapRenderer.GetImagePosition(topLeft, zPosition),
                                Color = Color.FromArgb(128, color),
                                Type = DrawType.Fill | DrawType.Outline
                            }, renderPass);
                        }

                        // Create new row or merge with existing of same length above
                        if (currentRect.HasValue && (tileType != 1 || x + 1 == MapRenderer.CurrentMap.Header.WalkableSize.Width))
                        {
                            var mergableRectAbove = new Tuple<int, int, int, int>(z, currentRect.Value.Left, currentRect.Value.Width, currentRect.Value.Top);
                            if (combinedTiles.ContainsKey(mergableRectAbove))
                            {
                                var newRect = new Tuple<int, int, int, int>(z, currentRect.Value.Left, currentRect.Value.Width, currentRect.Value.Bottom);
                                combinedTiles[newRect] = new Rectangle
                                {
                                    X = currentRect.Value.Left,
                                    Width = currentRect.Value.Width,
                                    Y = combinedTiles[mergableRectAbove].Top,
                                    Height = combinedTiles[mergableRectAbove].Height + currentRect.Value.Height
                                };
                                combinedTiles.Remove(mergableRectAbove);
                            }
                            else
                            {
                                var newRect = new Tuple<int, int, int, int>(z, currentRect.Value.Left, currentRect.Value.Width, currentRect.Value.Bottom);
                                combinedTiles[newRect] = currentRect.Value;
                            }
                            currentRect = null;
                        }
                    }
                }
            }

            foreach (var kvp in combinedTiles)
            {
                var z = kvp.Key.Item1;
                var renderPass = (RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels - z) * 2)) - (RenderPassPadding - 1);
                var zPosition = MapRenderer.GetCPosition(z);

                var rect = kvp.Value;
                var left = rect.Left;
                var right = rect.Right;
                var top = rect.Top;
                var bottom = rect.Bottom;

                var topRight = new Point(right, top);
                var bottomRight = new Point(right, bottom);
                var bottomLeft = new Point(left, bottom);
                var topLeft = new Point(left, top);
                MapRenderer.DrawQuadAndExpandMap(new Quad
                {
                    A = MapRenderer.GetImagePosition(topRight, zPosition),
                    B = MapRenderer.GetImagePosition(bottomRight, zPosition),
                    C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
                    D = MapRenderer.GetImagePosition(topLeft, zPosition),
                    Color = Color.FromArgb(96, GetLevelColor(z)),
                    Type = DrawType.Fill
                }, renderPass);
            }
        }

        public static void DrawWalkableOutline()
        {
            for (int z = MapRenderer.CurrentMap.Header.Levels - 1; z >= 0; z--)
            {
                var levelIndex = z;
                if (!MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowWalkableOutline)
                {
                    continue;
                }

                var renderPass = (RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels - z) * 2)) - (RenderPassPadding - 1);
                var zPosition = MapRenderer.GetCPosition(z);
                var color = MapRenderer.GetLevelColor(z);
                
                var topLeft = new Point(0, 0);
                var bottomRight = TileMapToMapPosition(MapRenderer.CurrentMap.Header.WalkableSize.Width, MapRenderer.CurrentMap.Header.WalkableSize.Height);
                var levelQuad = new Quad()
                {
                    A = GetImagePosition(new Point(bottomRight.X, topLeft.Y), zPosition),
                    B = GetImagePosition(bottomRight, zPosition),
                    C = GetImagePosition(new Point(topLeft.X, bottomRight.Y), zPosition),
                    D = GetImagePosition(topLeft, zPosition),
                    Color = color,
                    Type = DrawType.Outline
                };
                MapRenderer.DrawQuadAndExpandMap(levelQuad, renderPass - 1);
            }
        }

        public static void DrawObjectsAndHitBoxes()
        {
            var allObjects = MapRenderer.CurrentMap.MapObjects.MapObjects;
            MapRenderer.RenderSortedObjects = RendSort(allObjects).ToList();
            var sortedObjects = MapRenderer.RenderSortedObjects;
            foreach (var mapObject in sortedObjects)
            {
                try
                {
                    var levelIndex = mapObject.Level;
                    var mapObjectPage = mapObject.Pages.SelectedEventPage;
                    if (mapObjectPage == null)
                    {
                        continue;
                    }
                    var zPosition = MapRenderer.GetCPosition(mapObject.Level);

                    var renderPass = RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels - mapObject.Level) * 2) + mapObjectPage.RendType;
                    var mapObjectPosition = MapRenderer.GetImagePosition(mapObject.MapPosition, zPosition);

                    // Objects
                    var isShowingObject = !MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowObjects;
                    if (!isShowingObject)
                    {
                        if (!mapObjectPage.IsCharacter)
                        {
                            var animFrame = mapObjectPage.Frames > 0 ? (int)((MapRenderer.Frame / (mapObjectPage.Speed / 2.0)) % mapObjectPage.Frames) : 0;
							mapObjectPosition = MapRenderer.GetCharacterImagePosition(mapObject.MapPosition, zPosition);
							MapRenderer.LevelRenderer.Draw(new Sprite
                            {
                                Position = mapObjectPosition,
                                TexX = mapObjectPage.TexX + (mapObjectPage.TexW * animFrame),
                                TexY = mapObjectPage.TexY,
                                Width = mapObjectPage.TexW,
                                Height = mapObjectPage.TexH,
                                Texture = $"body{mapObjectPage.GraphicsIndex}"
                            }, renderPass);
                        }
                        else
                        {
                            var animFrame = (int)((MapRenderer.Frame / (6 / 2.0)) % 7);
                            bool reversed = mapObjectPage.Angle == 3 || mapObjectPage.Angle == 7;
                            var floating = Constants.IsFloatingCharacter(mapObjectPage.GraphicsIndex, mapObjectPage.CharacterIndex);
                            var noShadow = Constants.IsNoShadowCharacter(mapObjectPage.GraphicsIndex, mapObjectPage.CharacterIndex);
                            if (!noShadow)
                            {
                                var shadowPosition = mapObjectPosition;
                                shadowPosition.Offset(0, -16);
                                MapRenderer.LevelRenderer.Draw(new Sprite
                                {
                                    Position = shadowPosition,
                                    TexX = 0,
                                    TexY = 384,
                                    Width = 32,
                                    Height = 48,
                                    Texture = $"charachip1"
                                }, renderPass - 1);
                            }

                            mapObjectPosition.Offset(0, -40);
                            MapRenderer.LevelRenderer.Draw(new Sprite
                            {
                                Position = mapObjectPosition,
                                TexX = floating ? mapObjectPage.TexX + 64 * animFrame : mapObjectPage.TexX,
                                TexY = mapObjectPage.TexY,
                                Width = 64,
                                Height = 96,
                                Texture = $"charachip{mapObjectPage.GraphicsIndex}",
                                Scale = new Vector2(reversed ? -1 : 1, 1)
                            }, renderPass);
                        }

                        if (MapRenderer.MapDisplayOptions.IsShowingIDs)
                        {
                            var labelPosition = mapObjectPosition;
                            if (!mapObjectPage.IsCharacter)
                            {
                                labelPosition.Offset(0, mapObjectPage.TexH / 2);
                            }
                            else
                            {
                                labelPosition.Offset(0, 48);
                            }
                            var centerXOffset = -Constants.TextMeasurer.MeasureText(mapObject.ID, Constants.Fonts[FontType.Micro]).Width / 2;
                            labelPosition.Offset(centerXOffset, 0);

                            var textColor = Color.Yellow;
                            var textRenderPass = renderPass;
                            if (mapObject == MapRenderer.CurrentMapObject)
                            {
                                textColor = Color.Red;
                                textRenderPass += 2;
                            }
                            else if (mapObject == MapRenderer.ListHoveredMapObject || mapObject == MapRenderer.MapHoveredMapObject)
                            {
                                textColor = Color.GreenYellow;
                                textRenderPass++;
                            }

                            for (var shadowX = -1; shadowX <= 1; shadowX += 1)
                            {
                                for (var shadowY = -1; shadowY <= 1; shadowY += 1)
                                {
                                    MapRenderer.LevelRenderer.DrawText(new Text
                                    {
                                        Content = mapObject.ID,
                                        Font = Constants.Fonts[FontType.Micro],
                                        Position = new Point(labelPosition.X + shadowX, labelPosition.Y + shadowY),
                                        Color = shadowX == 0 && shadowY == 0 ? textColor : Color.Black
                                    }, shadowX == 0 && shadowY == 0 ? renderPass + 1 : renderPass);
                                }
                            }
                        }
                    }

                    // Hitboxes
                    var isShowingHitbox = !MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowHitboxes;
                    if (!isShowingHitbox)
                    {
                        Color color;
                        var hitWidth = mapObjectPage.HitRange.Width;
                        var hitHeight = mapObjectPage.HitRange.Height;
                        int hitShiftPosX, hitShiftPosY;
                        if (mapObjectPage.HitForm == HitFormType.Circle)
                        {
                            var adjustedHitShift = mapObjectPage.HitShift;
                            if (mapObject is MapMystery || mapObjectPage.IsWarp)
                            {
                                if (mapObjectPage.IsWarp)
                                {
                                    hitWidth = 6;
                                }
                                adjustedHitShift = new Point(mapObjectPage.HitShift.X - 10, mapObjectPage.HitShift.Y - 10);
                            }
                            color = Color.FromArgb(128, 0xFF, 0xFF, 0);
                            hitShiftPosX = mapObject.X + adjustedHitShift.X - hitWidth;
                            hitShiftPosY = mapObject.Y + adjustedHitShift.Y - hitWidth;
                            hitWidth *= 2;
                            hitHeight = hitWidth;
                        }
                        else if (mapObjectPage.StartTerms == StartTermType.Touch)
                        {
                            color = Color.FromArgb(128, 0, 0xFF, 0xFF);
                            // TODO: figure out real equation
                            hitShiftPosX = mapObject.X + mapObjectPage.HitShift.X - (int)(hitWidth / Math.Sqrt(2)) - 8;
                            hitShiftPosY = mapObject.Y + mapObjectPage.HitShift.Y - (int)(hitHeight / Math.Sqrt(2)) - 8;
                        }
                        else
                        {
                            color = Color.FromArgb(128, 0xFF, 0, 0xFF);
                            hitShiftPosX = mapObject.X + mapObjectPage.HitShift.X - hitWidth / 2 - 8;
                            hitShiftPosY = mapObject.Y + mapObjectPage.HitShift.Y - hitHeight / 2 - 8;
                        }
                        var topRight = new Point(hitShiftPosX + hitWidth, hitShiftPosY);
                        var bottomRight = new Point(hitShiftPosX + hitWidth, hitShiftPosY + hitHeight);
                        var bottomLeft = new Point(hitShiftPosX, hitShiftPosY + hitHeight);
                        var topLeft = new Point(hitShiftPosX, hitShiftPosY);
                        MapRenderer.DrawQuadAndExpandMap(new Quad
                        {
                            A = MapRenderer.GetImagePosition(topRight, zPosition),
                            B = MapRenderer.GetImagePosition(bottomRight, zPosition),
                            C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
                            D = MapRenderer.GetImagePosition(topLeft, zPosition),
                            Color = color,
                            Type = DrawType.Fill
                        }, renderPass - 1);
                    }
                }
				catch (Exception e)
				{
					Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
				}
			}
        }

        public static void DrawSelectedObjectOutlines()
        {
            if (!MapRenderer.MapDisplayOptions.IsOutlining)
            {
                return;
            }

            var outlinedObjects = new List<Tuple<Color, MapObject>>
            {
                new Tuple<Color, MapObject>(Color.FromArgb(196, Color.AliceBlue), MapRenderer.CurrentMapObject),
                new Tuple<Color, MapObject>(Color.FromArgb(128, Color.AliceBlue), MapRenderer.ListHoveredMapObject),
                new Tuple<Color, MapObject>(Color.FromArgb(128, Color.AliceBlue), MapRenderer.MapHoveredMapObject)
            };
            outlinedObjects.AddRange(MapRenderer.CurrentMap.MapObjects.MapObjects.Where(mo => mo.HasErrors).Select(mo => Tuple.Create(Color.FromArgb(128, Color.Red), mo)));

            foreach (var outlined in outlinedObjects)
            {
                MapRenderer.DrawObjectOutline(outlined.Item1, outlined.Item2);
            }
		}

        public static void DrawObjectOutline(Color color, MapObject mapObject)
        {
            if (mapObject == null)
            {
                return;
            }

            var levelIndex = mapObject.Level;
            var mapObjectPage = mapObject.Pages.SelectedEventPage;
            if (mapObjectPage == null)
            {
                return;
            }
            var zPosition = MapRenderer.GetCPosition(mapObject.Level);

            var renderPass = RenderPassPadding * MapRenderer.CurrentMap.Header.Levels * 2 + 3;
            var mapObjectPosition = MapRenderer.GetImagePosition(mapObject.MapPosition, zPosition);

            // Objects
            var isShowingObject = !MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowObjects;
            if (!isShowingObject)
            {
                if (!mapObjectPage.IsCharacter)
                {
                    mapObjectPosition = MapRenderer.GetCharacterImagePosition(mapObject.MapPosition, zPosition);
                    var topLeft = new Point(mapObjectPosition.X - mapObjectPage.TexW / 2, mapObjectPosition.Y - mapObjectPage.TexH / 2);
                    MapRenderer.DrawQuadAndExpandMap(new Quad
                    {
                        A = new Point(topLeft.X + mapObjectPage.TexW, topLeft.Y),
                        B = new Point(topLeft.X + mapObjectPage.TexW, topLeft.Y + mapObjectPage.TexH),
                        C = new Point(topLeft.X, topLeft.Y + mapObjectPage.TexH),
                        D = new Point(topLeft.X, topLeft.Y),
                        Color = color,
                        Type = DrawType.Outline
                    }, renderPass);
                }
                else
                {
                    var characterSize = new Size(64, 96);
                    var sizeAdjustment = new Point(0, -32);
                    var topLeft = new Point(mapObjectPosition.X - characterSize.Width / 2, mapObjectPosition.Y - characterSize.Height / 2 - 40);
                    MapRenderer.DrawQuadAndExpandMap(new Quad
                    {
                        A = new Point(topLeft.X + characterSize.Width + sizeAdjustment.X / 2, topLeft.Y - sizeAdjustment.Y),
                        B = new Point(topLeft.X + characterSize.Width + sizeAdjustment.X / 2, topLeft.Y + characterSize.Height),
                        C = new Point(topLeft.X - sizeAdjustment.X / 2, topLeft.Y + characterSize.Height),
                        D = new Point(topLeft.X - sizeAdjustment.X / 2, topLeft.Y - sizeAdjustment.Y),
                        Color = color,
                        Type = DrawType.Outline
                    }, renderPass);
                }
            }

            // Hitboxes
            var isShowingHitbox = !MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowHitboxes;
            if (!isShowingHitbox)
            {
                var hitWidth = mapObjectPage.HitRange.Width;
                var hitHeight = mapObjectPage.HitRange.Height;
                int hitShiftPosX, hitShiftPosY;
                if (mapObjectPage.HitForm == HitFormType.Circle)
                {
                    var adjustedHitShift = mapObjectPage.HitShift;
                    if (mapObject is MapMystery || mapObjectPage.IsWarp)
                    {
                        if (mapObjectPage.IsWarp)
                        {
                            hitWidth = 6;
                        }
                        adjustedHitShift = new Point(mapObjectPage.HitShift.X - 10, mapObjectPage.HitShift.Y - 10);
                    }
                    hitShiftPosX = mapObject.X + adjustedHitShift.X - hitWidth;
                    hitShiftPosY = mapObject.Y + adjustedHitShift.Y - hitWidth;
                    hitWidth *= 2;
                    hitHeight = hitWidth;
                }
                else if (mapObjectPage.StartTerms == StartTermType.Touch)
                {
                    // TODO: figure out real equation
                    hitShiftPosX = mapObject.X + mapObjectPage.HitShift.X - (int)(hitWidth / Math.Sqrt(2)) - 8;
                    hitShiftPosY = mapObject.Y + mapObjectPage.HitShift.Y - (int)(hitHeight / Math.Sqrt(2)) - 8;
                }
                else
                {
                    hitShiftPosX = mapObject.X + mapObjectPage.HitShift.X - hitWidth / 2 - 8;
                    hitShiftPosY = mapObject.Y + mapObjectPage.HitShift.Y - hitHeight / 2 - 8;
                }
                var topRight = new Point(hitShiftPosX + hitWidth, hitShiftPosY);
                var bottomRight = new Point(hitShiftPosX + hitWidth, hitShiftPosY + hitHeight);
                var bottomLeft = new Point(hitShiftPosX, hitShiftPosY + hitHeight);
                var topLeft = new Point(hitShiftPosX, hitShiftPosY);
                MapRenderer.DrawQuadAndExpandMap(new Quad
                {
                    A = MapRenderer.GetImagePosition(topRight, zPosition),
                    B = MapRenderer.GetImagePosition(bottomRight, zPosition),
                    C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
                    D = MapRenderer.GetImagePosition(topLeft, zPosition),
                    Color = color,
                    Type = DrawType.Outline
                }, renderPass);
            }
        }

		public static void DrawSelectedObjectMoves()
        {
            if (!MapRenderer.MapDisplayOptions.IsShowingMoves)
            {
                return;
            }

            var drawnMoveObjects = new List<Tuple<Color, MapObject>>
			{
				new Tuple<Color, MapObject>(Color.FromArgb(64, Color.AliceBlue), MapRenderer.CurrentMapObject),
				new Tuple<Color, MapObject>(Color.FromArgb(48, Color.AliceBlue), MapRenderer.ListHoveredMapObject),
				new Tuple<Color, MapObject>(Color.FromArgb(48, Color.AliceBlue), MapRenderer.MapHoveredMapObject)
			};

			foreach (var drawnMoveObject in drawnMoveObjects)
			{
                MapRenderer.DrawObjectMove(drawnMoveObject.Item1, drawnMoveObject.Item2, drawnMoveObject.Item2?.Pages?.SelectedEventPage?.Moves);
			}
		}

        public static void DrawObjectMove(Color color, MapObject mapObject, MoveCollection moves)
        {
            if (mapObject == null || moves == null)
            {
                return;
            }
            
            var angleColor = Color.FromArgb(128, color);
            var originalColor = color;

            var levelIndex = mapObject.Level;
            var mapObjectPage = mapObject.Pages.SelectedEventPage;
            if (mapObjectPage == null)
            {
                return;
            }
            var zPosition = MapRenderer.GetCPosition(mapObject.Level);

            var renderPass = RenderPassPadding * MapRenderer.CurrentMap.Header.Levels * 2;
            var mapObjectPosition = MapRenderer.GetImagePosition(mapObject.MapPosition, zPosition);

            var isShowingObject = !MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowObjects;
            if (!isShowingObject)
            {
                var currentPoint = new Point(mapObject.MapPosition.X + mapObjectPage.HitShiftX, mapObject.MapPosition.Y + mapObjectPage.HitShiftY);
                var currentAngle = (AngleTypeNumber)mapObjectPage.Angle;
                var locked = false;
                foreach (var move in moves.Moves)
                {
                    if (MapRenderer.ListHoveredMove == null && mapObject == MapRenderer.CurrentMapObject && move == moves.SelectedMove)
                    {
                        color = Color.FromArgb(96, Color.Crimson);
                        angleColor = Color.FromArgb(144, color);
                        renderPass = RenderPassPadding * MapRenderer.CurrentMap.Header.Levels * 2 + 1;
                    }
                    else if (move == MapRenderer.ListHoveredMove)
                    {
                        color = Color.FromArgb(64, Color.Crimson);
                        angleColor = Color.FromArgb(128, color);
                        renderPass = RenderPassPadding * MapRenderer.CurrentMap.Header.Levels * 2 + 3;
                    }
                    else
                    {
                        color = originalColor;
                        angleColor = Color.FromArgb(128, color);
                        renderPass = RenderPassPadding * MapRenderer.CurrentMap.Header.Levels * 2;
                    }
                    var isWalk = move.Type == MoveType.WalkUp || move.Type == MoveType.WalkDown || move.Type == MoveType.WalkRight || move.Type == MoveType.WalkLeft;

                    var startPoint = currentPoint;
                    var endPoint = currentPoint;
                    if (move.Type == MoveType.WalkUp || move.Type == MoveType.WarpUp)
                    {
                        endPoint = new Point(startPoint.X, startPoint.Y - move.Distance);
                        if (!locked)
                        {
                            currentAngle = AngleTypeNumber.North;
                        }

                        if (isWalk)
                        {
                            MapRenderer.DrawMovePath(4, startPoint, endPoint, zPosition, color, renderPass);
                            MapRenderer.DrawMovePathArrows(currentAngle, startPoint, endPoint, zPosition, angleColor, renderPass);
                        }
                        else
                        {
                            MapRenderer.DrawMovePath(2, startPoint, endPoint, zPosition, color, renderPass);
                        }
                    }
                    else if (move.Type == MoveType.WalkDown || move.Type == MoveType.WarpDown)
                    {
                        endPoint = new Point(startPoint.X, startPoint.Y + move.Distance);
                        if (!locked)
                        {
                            currentAngle = AngleTypeNumber.South;
                        }
                        var topRight = new Point(currentPoint.X + 4, currentPoint.Y - 4);
                        var bottomRight = new Point(endPoint.X + 4, endPoint.Y + 4);
                        var bottomLeft = new Point(endPoint.X - 4, endPoint.Y + 4);
                        var topLeft = new Point(currentPoint.X - 4, currentPoint.Y - 4);

                        if (isWalk)
                        {
                            MapRenderer.DrawMovePath(4, startPoint, endPoint, zPosition, color, renderPass);
                            MapRenderer.DrawMovePathArrows(currentAngle, startPoint, endPoint, zPosition, angleColor, renderPass);
                        }
                        else
                        {
                            MapRenderer.DrawMovePath(2, startPoint, endPoint, zPosition, color, renderPass);
                        }
                    }
                    else if (move.Type == MoveType.WalkLeft || move.Type == MoveType.WarpLeft)
                    {
                        endPoint = new Point(startPoint.X - move.Distance, startPoint.Y);
                        if (!locked)
                        {
                            currentAngle = AngleTypeNumber.West;
                        }
                        var topRight = new Point(currentPoint.X + 4, currentPoint.Y - 4);
                        var bottomRight = new Point(currentPoint.X + 4, currentPoint.Y + 4);
                        var bottomLeft = new Point(endPoint.X - 4, endPoint.Y + 4);
                        var topLeft = new Point(endPoint.X - 4, endPoint.Y - 4);

                        if (isWalk)
                        {
                            MapRenderer.DrawMovePath(4, startPoint, endPoint, zPosition, color, renderPass);
                            MapRenderer.DrawMovePathArrows(currentAngle, startPoint, endPoint, zPosition, angleColor, renderPass);
                        }
                        else
                        {
                            MapRenderer.DrawMovePath(2, startPoint, endPoint, zPosition, color, renderPass);
                        }
                    }
                    else if (move.Type == MoveType.WalkRight || move.Type == MoveType.WarpRight)
                    {
                        endPoint = new Point(startPoint.X + move.Distance, startPoint.Y);
                        if (!locked)
                        {
                            currentAngle = AngleTypeNumber.East;
                        }
                        var topRight = new Point(endPoint.X + 4, endPoint.Y - 4);
                        var bottomRight = new Point(endPoint.X + 4, endPoint.Y + 4);
                        var bottomLeft = new Point(currentPoint.X - 4, currentPoint.Y + 4);
                        var topLeft = new Point(currentPoint.X - 4, currentPoint.Y - 4);

                        if (isWalk)
                        {
                            MapRenderer.DrawMovePath(4, startPoint, endPoint, zPosition, color, renderPass);
                            MapRenderer.DrawMovePathArrows(currentAngle, startPoint, endPoint, zPosition, angleColor, renderPass);
                        }
                        else
                        {
                            MapRenderer.DrawMovePath(2, startPoint, endPoint, zPosition, color, renderPass);
                        }
                    }
                    else if (move.Type == MoveType.AngleUp)
                    {
                        if (!locked)
                        {
                            currentAngle = AngleTypeNumber.North;
                            MapRenderer.DrawMovePathArrows(currentAngle, startPoint, endPoint, zPosition, angleColor, renderPass);
                        }
                    }
                    else if (move.Type == MoveType.AngleDown)
                    {
                        if (!locked)
                        {
                            currentAngle = AngleTypeNumber.South;
                            MapRenderer.DrawMovePathArrows(currentAngle, startPoint, endPoint, zPosition, angleColor, renderPass);
                        }
                    }
                    else if (move.Type == MoveType.AngleLeft)
                    {
                        if (!locked)
                        {
                            currentAngle = AngleTypeNumber.West;
                            MapRenderer.DrawMovePathArrows(currentAngle, startPoint, endPoint, zPosition, angleColor, renderPass);
                        }
                    }
                    else if (move.Type == MoveType.AngleRight)
                    {
                        if (!locked)
                        {
                            currentAngle = AngleTypeNumber.East;
                            MapRenderer.DrawMovePathArrows(currentAngle, startPoint, endPoint, zPosition, angleColor, renderPass);
                        }
                    }
                    else if (move.Type == MoveType.AngleLock)
                    {
                        locked = true;
                        continue;
                    }
                    else if (move.Type == MoveType.AngleUnlock)
                    {
                        locked = false;
                        continue;
                    }
                    else if (move.Type == MoveType.Jump)
                    {
                        MapRenderer.DrawMovePath(4, startPoint, endPoint, zPosition, color, renderPass);

                        var topRight = new Point(startPoint.X - 3, startPoint.Y - 3);
                        var bottomRight = new Point(startPoint.X + 3, startPoint.Y - 3);
                        var bottomLeft = new Point(startPoint.X - 3, startPoint.Y + 3);
                        var topLeft = new Point(startPoint.X - 3, startPoint.Y - 3);

                        MapRenderer.DrawQuadAndExpandMap(new Quad
                        {
                            A = MapRenderer.GetImagePosition(topRight, zPosition),
                            B = MapRenderer.GetImagePosition(bottomRight, zPosition),
                            C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
                            D = MapRenderer.GetImagePosition(topLeft, zPosition),
                            Color = angleColor,
                            Type = DrawType.Fill
                        }, renderPass);
                    }
                    else if (move.Type == MoveType.Wait)
                    {
                        MapRenderer.DrawMovePath(4, startPoint, endPoint, zPosition, color, renderPass);
                    }

                    currentPoint = endPoint;
                }
            }
        }

        public static void DrawSelectedEventMarkers()
        {
            var color = Color.FromArgb(128, Color.Red);
            var selectedEvent = MapRenderer.CurrentMapObject?.Pages?.SelectedEventPage?.Events?.SelectedEvent?.Instance;

            if (selectedEvent == null)
            {
                return;
            }

            var mapObjects = MapRenderer.CurrentMap.MapObjects.MapObjects;

            var outlinedObject = default(MapObject);
            var moves = default(MoveCollection);
            var position = default(Tuple<Point, int>);
            var quads = new List<Quad>();

            if (selectedEvent is EventMoveEvent)
            {
                var e = (EventMoveEvent)selectedEvent;
                outlinedObject = e.IsMapIndex
                    ? (e.MapIndex >= 0 && e.MapIndex < mapObjects.Count ? mapObjects[e.MapIndex] : null)
                    : mapObjects.FirstOrDefault(mo => mo.ID == e.ObjectID);
                moves = e.Moves;
            }
            else if (selectedEvent is MapChangeEvent || selectedEvent is MapWarpEvent || selectedEvent is WarpEvent || selectedEvent is JackInEvent || selectedEvent is JackInDirectEvent)
            {
                var targetMap = default(string);
                int x, y, z;
                x = y = z = default(int);
                if (selectedEvent is MapChangeEvent)
                {
                    var e = (MapChangeEvent)selectedEvent;
                    targetMap = e.TargetMap;
                    x = e.X;
                    y = e.Y;
                    z = e.Z;
                }
                if (selectedEvent is MapWarpEvent)
                {
                    var e = (MapWarpEvent)selectedEvent;
                    targetMap = e.TargetMap;
                    x = e.X;
                    y = e.Y;
                    z = e.Z;
                }
                else if (selectedEvent is WarpEvent)
                {
                    var e = (WarpEvent)selectedEvent;
                    targetMap = e.TargetMap;
                    x = e.X;
                    y = e.Y;
                    z = e.Z;
                }
                else if (selectedEvent is JackInEvent)
                {
                    var e = (JackInEvent)selectedEvent;
                    targetMap = e.TargetMap;
                    x = e.X;
                    y = e.Y;
                    z = e.Z;
                }
                else if (selectedEvent is JackInDirectEvent)
                {
                    var e = (JackInDirectEvent)selectedEvent;
                    targetMap = e.TargetMap;
                    x = e.X;
                    y = e.Y;
                    z = e.Z;
                }

                if (targetMap == MapRenderer.CurrentMap.Name)
                {
                    position = Tuple.Create(new Point(x - 10, y - 10), z);
                }
            }
            else if (selectedEvent is CameraEvent || selectedEvent is CameraDefaultEvent)
            {
                var mapObject = MapRenderer.CurrentMapObject;
                var mapObjectPage = mapObject.Pages.SelectedEventPage;
                var hitWidth = mapObjectPage.HitRange.Width;
                var hitHeight = mapObjectPage.HitRange.Height;
                int hitShiftPosX, hitShiftPosY;
                if (mapObjectPage.StartTerms == StartTermType.Touch)
                {
                    if (mapObjectPage.Terms.Terms.All(t => t.Instance is NoneTerm))
                    {
                        // TODO: figure out real equation
                        hitShiftPosX = (mapObject.X + mapObjectPage.HitShift.X - (int)(hitWidth / Math.Sqrt(2)) - 8) >> 1;
                        hitShiftPosY = (mapObject.Y + mapObjectPage.HitShift.Y - (int)(hitHeight / Math.Sqrt(2)) - 8) >> 1;
                    }
                    else
                    {
                        // TODO: figure out real equation
                        hitShiftPosX = 0;
                        hitShiftPosY = 0;
                    }
                }
                else
                {
                    hitShiftPosX = mapObject.X + mapObjectPage.HitShift.X - hitWidth / 2 - 8;
                    hitShiftPosY = mapObject.Y + mapObjectPage.HitShift.Y - hitHeight / 2 - 8;
                }

                var order = 0;
                var duplicatePositions = new Dictionary<Point, int>();
                var basePos = MapRenderer.GetImagePosition(MapRenderer.CurrentMapObject.X, MapRenderer.CurrentMapObject.Y, MapRenderer.GetCPosition(MapRenderer.CurrentMapObject.Level));
                basePos.Offset(hitShiftPosX, -hitShiftPosY);
                if (mapObjectPage.StartTerms == StartTermType.Touch)
                {
                    quads.Add(new Quad
                    {
                        A = new Point(basePos.X - 2, basePos.Y - 2),
                        B = new Point(basePos.X + 2, basePos.Y - 2),
                        C = new Point(basePos.X + 2, basePos.Y + 2),
                        D = new Point(basePos.X - 2, basePos.Y + 2),
                        Color = Color.FromArgb(255, Color.LightBlue),
                        Type = DrawType.Outline | DrawType.Fill
                    });
                    quads.Add(new Quad
                    {
                        A = new Point(basePos.X - 120, basePos.Y - 80),
                        B = new Point(basePos.X + 120, basePos.Y - 80),
                        C = new Point(basePos.X + 120, basePos.Y + 80),
                        D = new Point(basePos.X - 120, basePos.Y + 80),
                        Color = Color.FromArgb(64, Color.LightBlue),
                        Type = DrawType.Outline
                    });
                    var renderPass = RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels) * 2) + 1;
                    MapRenderer.LevelRenderer.DrawText(new Text
                    {
                        Content = $"{order}",
                        Color = Color.Yellow,
                        Position = new Point(basePos.X - 120, basePos.Y - 80 + ((duplicatePositions.ContainsKey(basePos) ? duplicatePositions[basePos] : 0) * 15)),
                        Font = Constants.Fonts[FontType.Micro]
                    }, renderPass);
                    order++;
                    duplicatePositions[basePos] = duplicatePositions.ContainsKey(basePos) ? duplicatePositions[basePos]++ : 1;
                }

                var outOfMap = false;
                foreach (var pageEvent in MapRenderer.CurrentMapObject.Pages.SelectedEventPage.Events.Events)
                {
                    if (pageEvent.Instance is MapChangeEvent ||
                        pageEvent.Instance is WarpEvent ||
                        pageEvent.Instance is MapWarpEvent ||
                        pageEvent.Instance is WarpPlugOutEvent ||
                        pageEvent.Instance is JackInDirectEvent ||
                        pageEvent.Instance is JackOutEvent ||
                        pageEvent.Instance is JackInEvent)
                    {
                        switch (pageEvent.Instance)
                        {
                            case MapChangeEvent e:
                                outOfMap = e.TargetMap != MapRenderer.CurrentMap.Name;
                                break;
                            case WarpEvent e:
                                outOfMap = e.TargetMap != MapRenderer.CurrentMap.Name;
                                break;
                            case MapWarpEvent e:
                                outOfMap = e.TargetMap != MapRenderer.CurrentMap.Name;
                                break;
                            default:
                                outOfMap = true;
                                break;
                        }
                        if (outOfMap)
                        {
                            continue;
                        }
                    }
                    else if (pageEvent.Instance is CameraEvent ce)
                    {
                        if (ce.IsAbsolute)
                        {
                            basePos = new Point(ce.X, ce.Y);
                            basePos.Offset(MapRenderer.CurrentMap.Header.Offset.X, MapRenderer.CurrentMap.Header.Offset.Y);
                            quads.Add(new Quad
                            {
                                A = new Point(basePos.X - 2, basePos.Y - 2),
                                B = new Point(basePos.X + 2, basePos.Y - 2),
                                C = new Point(basePos.X + 2, basePos.Y + 2),
                                D = new Point(basePos.X - 2, basePos.Y + 2),
                                Color = Color.FromArgb(255, Color.Wheat),
                                Type = DrawType.Outline | DrawType.Fill
                            });
                        }
                        else
                        {
                            var movePos = new Point(basePos.X, basePos.Y);
                            basePos.Offset(ce.X, ce.Y);
                            quads.Add(new Quad
                            {
                                A = movePos,
                                B = movePos,
                                C = basePos,
                                D = basePos,
                                Color = Color.FromArgb(255, Color.Wheat),
                                Type = DrawType.Outline
                            });
                        }
                        quads.Add(new Quad
                        {
                            A = new Point(basePos.X - 120, basePos.Y - 80),
                            B = new Point(basePos.X + 120, basePos.Y - 80),
                            C = new Point(basePos.X + 120, basePos.Y + 80),
                            D = new Point(basePos.X - 120, basePos.Y + 80),
                            Color = ce == selectedEvent ? color : Color.FromArgb(64, Color.Wheat),
                            Type = DrawType.Outline
                        });
                        var renderPass = RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels) * 2) + 1;
                        MapRenderer.LevelRenderer.DrawText(new Text
                        {
                            Content = $"{order}",
                            Color = Color.Yellow,
                            Position = new Point(basePos.X - 120, basePos.Y - 80 + ((duplicatePositions.ContainsKey(basePos) ? duplicatePositions[basePos] : 0) * 15)),
                            Font = Constants.Fonts[FontType.Micro]
                        }, renderPass);
                        order++;
                        duplicatePositions[basePos] = duplicatePositions.ContainsKey(basePos) ? duplicatePositions[basePos]++ : 1;
                    }
                    else if (pageEvent.Instance is CameraDefaultEvent cde && mapObjectPage.StartTerms == StartTermType.Touch)
                    {
                        basePos = MapRenderer.GetImagePosition(MapRenderer.CurrentMapObject.X, MapRenderer.CurrentMapObject.Y, MapRenderer.GetCPosition(MapRenderer.CurrentMapObject.Level));
                        //basePos.Offset(hitShiftPosX, -hitShiftPosY);
                        quads.Add(new Quad
                        {
                            A = new Point(basePos.X - 2, basePos.Y - 2),
                            B = new Point(basePos.X + 2, basePos.Y - 2),
                            C = new Point(basePos.X + 2, basePos.Y + 2),
                            D = new Point(basePos.X - 2, basePos.Y + 2),
                            Color = Color.FromArgb(255, Color.Wheat),
                            Type = DrawType.Outline | DrawType.Fill
                        });
                        quads.Add(new Quad
                        {
                            A = new Point(basePos.X - 120, basePos.Y - 80),
                            B = new Point(basePos.X + 120, basePos.Y - 80),
                            C = new Point(basePos.X + 120, basePos.Y + 80),
                            D = new Point(basePos.X - 120, basePos.Y + 80),
                            Color = cde == selectedEvent ? color : Color.FromArgb(64, Color.Wheat),
                            Type = DrawType.Outline
                        });
                        var textColor = Color.Yellow;
                        var renderPass = RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels) * 2) + 1;
                        MapRenderer.LevelRenderer.DrawText(new Text
                        {
                            Content = $"{order}",
                            Color = textColor,
                            Position = new Point(basePos.X - 120, basePos.Y - 80 + ((duplicatePositions.ContainsKey(basePos) ? duplicatePositions[basePos] : 0) * 15)),
                            Font = Constants.Fonts[FontType.Micro]
                        }, renderPass);
                        order++;
                        duplicatePositions[basePos] = duplicatePositions.ContainsKey(basePos) ? duplicatePositions[basePos]++ : 1;
                    }
                    else if (pageEvent.Instance is MapWarpEvent mwe)
                    {
                        // TODO: find out cross-event interaction
                    }
                }
            }
            else if (selectedEvent is EventRunEvent)
            {
                var e = (EventRunEvent)selectedEvent;
                outlinedObject = mapObjects.FirstOrDefault(mo => mo.ID == e.ID);
            }
            else if (selectedEvent is EffectEvent)
            {
                var e = (EffectEvent)selectedEvent;
                switch (e.LocationType)
                {
                    case (int)EffectLocationTypeNumber.Position:
                        position = Tuple.Create(new Point(e.X, e.Y), e.Z);
                        break;
                    case (int)EffectLocationTypeNumber.OtherObject:
                        outlinedObject = mapObjects.FirstOrDefault(mo => mo.ID == e.TargetName);
                        break;
                    case (int)EffectLocationTypeNumber.ParentObject:
                    case (int)EffectLocationTypeNumber.Variable:
                        break;
                }
            }
            else if (selectedEvent is EffectDeleteEvent)
            {
                var e = (EffectDeleteEvent)selectedEvent;
                outlinedObject = mapObjects.FirstOrDefault(mo => mo.Pages.SelectedEventPage.Events.Events.Any(ee =>
                {
                    if (ee.Instance is EffectEvent eee)
                    {
                        return eee.ID == e.ID;
                    }

                    return false;
                }));
            }
            else if (selectedEvent is EventDeleteEvent)
            {
                var e = (EventDeleteEvent)selectedEvent;
                outlinedObject = mapObjects.FirstOrDefault(mo => mo.ID == e.ID);
            }


            if (outlinedObject != null)
            {
                MapRenderer.DrawObjectOutline(color, outlinedObject);

                if (moves != null)
                {
                    MapRenderer.DrawObjectMove(Color.FromArgb(64, color), outlinedObject, moves);
                }
            }

            if (position != null)
            {
                var renderPass = RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels - position.Item2) * 2) + 3;

                var markerSizes = new[] { 2, 4 };
                foreach(var radius in markerSizes)
                {
                    var topLeft = MapRenderer.GetImagePosition(new Point(position.Item1.X - radius, position.Item1.Y - radius), MapRenderer.GetCPosition(position.Item2));
                    var topRight = MapRenderer.GetImagePosition(new Point(position.Item1.X + radius, position.Item1.Y - radius), MapRenderer.GetCPosition(position.Item2));
                    var bottomRight = MapRenderer.GetImagePosition(new Point(position.Item1.X + radius, position.Item1.Y + radius), MapRenderer.GetCPosition(position.Item2));
                    var bottomLeft = MapRenderer.GetImagePosition(new Point(position.Item1.X - radius, position.Item1.Y + radius), MapRenderer.GetCPosition(position.Item2));

                    MapRenderer.DrawQuadAndExpandMap(new Quad
                    {
                        A = topLeft,
                        B = topRight,
                        C = bottomRight,
                        D = bottomLeft,
                        Color = color,
                        Type = DrawType.Fill
                    }, renderPass);
                }
            }

            foreach (var quad in quads)
            {
                var renderPass = RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels) * 2) + 1;
                MapRenderer.DrawQuadAndExpandMap(quad, renderPass);
            }
        }

        public static void DrawWalkableEditing()
        {
            MapRenderer.WalkablePreview.Clear();

            var levelIndex = MapRenderer.CurrentLevel;

            /*
            if (!MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowWalkable)
            {
                MapRenderer.MouseDragRelease = null;
                MapRenderer.MouseClickPosition = null;
                return;
            }
            */

            var zPosition = MapRenderer.GetCPosition(MapRenderer.CurrentLevel);
            var renderPass = (RenderPassPadding * ((MapRenderer.CurrentMap.Header.Levels - MapRenderer.CurrentLevel) * 2)) - (RenderPassPadding - 1);
            if (MapRenderer.MouseDragRectangle.HasValue)
            {
                var mapDownPosition = MapRenderer.GetMapPosition(MapRenderer.MouseDragRectangle.Value.Left, MapRenderer.MouseDragRectangle.Value.Top, zPosition);
                var mapDragPosition = MapRenderer.GetMapPosition(MapRenderer.MouseDragRectangle.Value.Right, MapRenderer.MouseDragRectangle.Value.Bottom, zPosition);
                var tileDownPosition = GetTileMapPosition(
                    mapDownPosition,
                    mapDragPosition.X > mapDownPosition.X ? (Func<double, double>)Math.Floor : InvertFloor,
                    mapDragPosition.Y > mapDownPosition.Y ? (Func<double, double>)Math.Floor : InvertFloor
                );
                var tileDragPosition = GetTileMapPosition(
                    mapDragPosition,
                    mapDragPosition.X > mapDownPosition.X ? (Func<double, double>)InvertFloor : Math.Floor,
                    mapDragPosition.Y > mapDownPosition.Y ? (Func<double, double>)InvertFloor : Math.Floor
                );
                var tileSnapDownPosition = TileMapToMapPosition(tileDownPosition);
                var tileSnapDragPosition = TileMapToMapPosition(tileDragPosition);

                var adjOffset = MapRenderer.SelectedTile.ToTileOffset();

                if (!MapRenderer.SelectedTile.IsDualStair())
                {
                    Iterate2D(tileDownPosition, tileDragPosition, (x, y)
                        => MapRenderer.WalkablePreview[new Tuple<int, int, int>(MapRenderer.CurrentLevel, y + adjOffset.Y, x + adjOffset.X)] = (int)MapRenderer.SelectedTile);
                }
                else
                {
                    var adjDualStairOffset = MapRenderer.SelectedTile.ToDualStairTileOffset(MapRenderer.CurrentMap.Header.FloorHeight);
                    Iterate2D(tileDownPosition, tileDragPosition, (x, y)
                        => MapRenderer.WalkablePreview[new Tuple<int, int, int>(MapRenderer.CurrentLevel, y + adjOffset.Y, x + adjOffset.X)] = (int)(MapRenderer.SelectedTile.ToDualStairStart()));
                    Iterate2D(tileDownPosition, tileDragPosition, (x, y)
                        => MapRenderer.WalkablePreview[new Tuple<int, int, int>(MapRenderer.CurrentLevel - MapRenderer.SelectedTile.ToDualStairLevelOffset(), y + adjDualStairOffset.Y, x + adjDualStairOffset.X)] = (int)(MapRenderer.SelectedTile.ToDualStairEnd()));
                }

                var outlineQuad = new Quad
                {
                    A = MapRenderer.GetImagePosition(new Point(tileSnapDragPosition.X, tileSnapDownPosition.Y), zPosition),
                    B = MapRenderer.GetImagePosition(tileSnapDragPosition, zPosition),
                    C = MapRenderer.GetImagePosition(new Point(tileSnapDownPosition.X, tileSnapDragPosition.Y), zPosition),
                    D = MapRenderer.GetImagePosition(tileSnapDownPosition, zPosition),
                    Color = Color.FromArgb(255, GetLevelColor(MapRenderer.CurrentLevel)),
                    Type = DrawType.Outline
                };
                MapRenderer.LevelRenderer.DrawQuad(outlineQuad, renderPass);

                var tintQuad = outlineQuad.Copy();
                tintQuad.Color = Color.FromArgb(128, Color.White);
                MapRenderer.LevelRenderer.DrawQuad(tintQuad, renderPass);
            }
            else if (MapRenderer.MouseDragRelease.HasValue || MapRenderer.MouseClickPosition.HasValue)
            {
                Point mapDownPosition;
                Point mapDragPosition;
                if (MapRenderer.MouseDragRelease.HasValue)
                {
                    mapDownPosition = MapRenderer.GetMapPosition(MapRenderer.MouseDragRelease.Value.Location, zPosition);
                    mapDragPosition = MapRenderer.GetMapPosition(new Point(MapRenderer.MouseDragRelease.Value.Right, MapRenderer.MouseDragRelease.Value.Bottom), zPosition);
                }
                else
                {
                    mapDownPosition = mapDragPosition = MapRenderer.GetMapPosition(MapRenderer.MouseClickPosition.Value, zPosition);
                }

                var tileDownPosition = GetTileMapPosition(
                    mapDownPosition,
                    mapDragPosition.X > mapDownPosition.X ? (Func<double, double>)Math.Floor : InvertFloor,
                    mapDragPosition.Y > mapDownPosition.Y ? (Func<double, double>)Math.Floor : InvertFloor
                );
                var tileDragPosition = GetTileMapPosition(
                    mapDragPosition,
                    mapDragPosition.X > mapDownPosition.X ? (Func<double, double>)InvertFloor : Math.Floor,
                    mapDragPosition.Y > mapDownPosition.Y ? (Func<double, double>)InvertFloor : Math.Floor
                );
                var adjOffset = MapRenderer.SelectedTile.ToTileOffset();

                if (!MapRenderer.SelectedTile.IsDualStair())
                {
                    Iterate2D(tileDownPosition, tileDragPosition, (x, y) => MapRenderer.CurrentMap.WalkableMap[MapRenderer.CurrentLevel, y + adjOffset.Y, x + adjOffset.X] = (int)MapRenderer.SelectedTile);
                    var isConveyorTile = default(bool);
                    switch (MapRenderer.SelectedTile)
                    {
                        case WalkableTileType.ConveyorEast:
                        case WalkableTileType.ConveyorNorth:
                        case WalkableTileType.ConveyorSouth:
                        case WalkableTileType.ConveyorWest:
                            isConveyorTile = true;
                            break;
                    }
                    if (MapRenderer.IsPlacingConveyors)
                    {
                        var iteratedRectangle = RectangleFromPoints(tileDownPosition, tileDragPosition);

                        var newConveyorArea = new Rectangle((iteratedRectangle.X + 1) * 8, (iteratedRectangle.Y + 1) * 8, (iteratedRectangle.Width + 1) * 8 , (iteratedRectangle.Height + 1) * 8);
                        var overwrittenConveyors = MapRenderer.CurrentMap.MapObjects.MapObjects.Where(mo =>
                        {
                            return newConveyorArea.Contains(mo.X, mo.Y) && mo.Pages.MapEventPages.Any(mep => mo.Level == MapRenderer.CurrentLevel
                            && ((mep.Texture == "body2"  && Constants.ConveyorSpriteArea.Contains(mep.TexX, mep.TexY))
                                || (mep.Texture == "body27" && Constants.TileConveyorSpriteArea.Contains(mep.TexX, mep.TexY))));
                        }).ToList();

                        foreach (var deletedConveyor in overwrittenConveyors)
                        {
                            MapRenderer.CurrentMap.MapObjects.MapObjects.Remove(deletedConveyor);
                        }

                        Iterate2D(tileDownPosition, tileDragPosition, (x, y) =>
                        {
                            var everyOtherTileFromDrag = Math.Abs(tileDownPosition.X - x) % 2 == 1 || Math.Abs(tileDownPosition.Y - y) % 2 == 1;
                            var notFullSquare = !iteratedRectangle.Contains(new Rectangle(x, y, 2, 2));

                            if (everyOtherTileFromDrag || notFullSquare)
                            {
                                return;
                            }

                            var centerMapPosition = new Point((8 * x) + 12, (8 * y) + 12);

                            if (isConveyorTile)
                            {
                                var objectToAdd = Constants.ConveyorCreator(MapRenderer.SelectedTile, MapRenderer.ConveyorColor, centerMapPosition, MapRenderer.CurrentLevel);
                                objectToAdd.Level = MapRenderer.CurrentLevel;

                                MapRenderer.CurrentMap.MapObjects.MapObjects.Add(objectToAdd);
                            }
                        });
                    }
                }
                else
                {
                    var adjDualStairOffset = MapRenderer.SelectedTile.ToDualStairTileOffset(MapRenderer.CurrentMap.Header.FloorHeight);
                    Iterate2D(tileDownPosition, tileDragPosition, (x, y) => MapRenderer.CurrentMap.WalkableMap[MapRenderer.CurrentLevel, y + adjOffset.Y, x + adjOffset.X] = (int)(MapRenderer.SelectedTile.ToDualStairStart()));
                    Iterate2D(tileDownPosition, tileDragPosition, (x, y) => MapRenderer.CurrentMap.WalkableMap[MapRenderer.CurrentLevel - MapRenderer.SelectedTile.ToDualStairLevelOffset(), y + adjDualStairOffset.Y, x + adjDualStairOffset.X] = (int)(MapRenderer.SelectedTile.ToDualStairEnd()));
                }
                MapRenderer.MouseDragRelease = null;
                MapRenderer.MouseClickPosition = null;
            }
            else if (MapRenderer.MousePosition.HasValue)
            {
                var mapMousePos = MapRenderer.GetMapPosition(MapRenderer.MousePosition.Value.X, MapRenderer.MousePosition.Value.Y, zPosition);
                var closestTileSnap = TileMapToMapPosition(GetTileMapPosition(mapMousePos, Math.Floor));
                var furthestTileSnap = TileMapToMapPosition(GetTileMapPosition(mapMousePos, InvertFloor));

                var outlineQuad = new Quad
                {
                    A = MapRenderer.GetImagePosition(new Point(closestTileSnap.X, furthestTileSnap.Y), zPosition),
                    B = MapRenderer.GetImagePosition(closestTileSnap, zPosition),
                    C = MapRenderer.GetImagePosition(new Point(furthestTileSnap.X, closestTileSnap.Y), zPosition),
                    D = MapRenderer.GetImagePosition(furthestTileSnap, zPosition),
                    Color = Color.FromArgb(255, GetLevelColor(MapRenderer.CurrentLevel)),
                    Type = DrawType.Outline
                };
                MapRenderer.LevelRenderer.DrawQuad(outlineQuad, renderPass);

                var tintQuad = outlineQuad.Copy();
                tintQuad.Color = Color.FromArgb(128, Color.White);
                MapRenderer.LevelRenderer.DrawQuad(tintQuad, renderPass);
            }

            if (MapRenderer.MousePosition.HasValue)
            {
                MapRenderer.MapPositionUpdateFunc(MapRenderer.GetMapPosition(MapRenderer.MousePosition.Value, MapRenderer.GetCPosition(MapRenderer.CurrentLevel)), MapRenderer.CurrentLevel, MapRenderer.MapHoveredMapObject?.ID);
            }
            else
            {
                MapRenderer.MapPositionUpdateFunc(null, 0, null);
            }
        }

        public static void DrawObjectSelection()
        {
            var renderPass = RenderPassPadding * MapRenderer.CurrentMap.Header.Levels * 2 + 3;
            if (MapRenderer.MouseDragRectangle.HasValue && !MapRenderer.MouseDragRectangle.Value.Size.IsEmpty)
            {
                //var topLeft = new Point(MapRenderer.MouseDragRectangle.Value.Left, MapRenderer.MouseDragRectangle.Value.Top);
                //var bottomRight = topLeft;
                //bottomRight.Offset(new Point(MapRenderer.MouseDragRectangle.Value.Size));

                //MapRenderer.LevelRenderer.DrawQuad(new Quad
                //{
                //    A = new Point(bottomRight.X, topLeft.Y),
                //    B = new Point(bottomRight.X, bottomRight.Y),
                //    C = new Point(topLeft.X, bottomRight.Y),
                //    D = new Point(topLeft.X, topLeft.Y),
                //    Color = Color.FromArgb(196, Color.AliceBlue),
                //    Type = DrawType.Outline
                //}, renderPass);
                
                if (MapRenderer.MapHoveredMapObject != null && MapRenderer.MousePosition != null)
                {
                    var mouseMapPosition = MapRenderer.GetMapPosition(MapRenderer.MousePosition.Value, MapRenderer.MapHoveredMapObject.Level);
                    if (!MapRenderer.draggedMapObjectOffset.HasValue)
                    {
                        MapRenderer.draggedMapObjectOffset = new Point(MapRenderer.MapHoveredMapObject.MapPosition.X - mouseMapPosition.X, MapRenderer.MapHoveredMapObject.MapPosition.Y - mouseMapPosition.Y);
                    }

                    var newPosition = new Point(mouseMapPosition.X + MapRenderer.draggedMapObjectOffset.Value.X, mouseMapPosition.Y + MapRenderer.draggedMapObjectOffset.Value.Y);
                    MapRenderer.MapHoveredMapObject.SetMapPosition(newPosition, MapRenderer.MapHoveredMapObject != MapRenderer.CurrentMapObject);
                }
            }
            else if (MapRenderer.MouseClickPosition.HasValue)
            {
                MapRenderer.CurrentMapObject = MapRenderer.MapHoveredMapObject;
                MapRenderer.MouseDragRelease = null;
                MapRenderer.MouseClickPosition = null;
            }
            else if (MapRenderer.MouseDragRelease.HasValue)
            {
                if (MapRenderer.MapHoveredMapObject != null)
                {
                    MapRenderer.CurrentMapObject = MapRenderer.MapHoveredMapObject;
                    MapRenderer.MapHoveredMapObject.SetMapPosition(MapRenderer.MapHoveredMapObject.MapPosition, true);
                    MapRenderer.draggedMapObjectOffset = null;
                }
                MapRenderer.MouseDragRelease = null;
                MapRenderer.MouseClickPosition = null;
            }
            else if (MapRenderer.MousePosition.HasValue)
            {
                var hoveredObject = MapRenderer.GetObjectAtImagePoint(MapRenderer.MousePosition.Value);
                if (MapRenderer.MapHoveredMapObject != hoveredObject)
                {
                    MapRenderer.MapHoveredMapObject = hoveredObject;
                    if (!MapRenderer.HoverLayerChanged)
                    {
                        MapRenderer.HoverLayer = 0;
                    }
                    else
                    {
                        MapRenderer.HoverLayerChanged = false;
                    }
                }
            }

            if (MapRenderer.MousePosition.HasValue)
            {
                var usedLevel = MapRenderer.MapHoveredMapObject?.Level ?? MapRenderer.CurrentLevel;
                MapRenderer.MapPositionUpdateFunc(MapRenderer.GetMapPosition(MapRenderer.MousePosition.Value, MapRenderer.GetCPosition(usedLevel)), usedLevel, MapRenderer.MapHoveredMapObject?.ID);
            }
            else
            {
                MapRenderer.MapPositionUpdateFunc(null, 0, null);
            }
        }

        private static MapObject GetObjectAtImagePoint(Point p)
        {
            var allObjects = MapRenderer.CurrentMap.MapObjects.MapObjects;
            var sortedObjects = MapRenderer.RenderSortedObjects;
            var hoveredObjects = sortedObjects.Where(mapObject =>
            {
                var levelIndex = mapObject.Level;
                var mapObjectPage = mapObject.Pages.SelectedEventPage;
                if (mapObjectPage == null)
                {
                    return false;
                }
                var zPosition = MapRenderer.GetCPosition(mapObject.Level);
                
                var mapObjectPosition = MapRenderer.GetImagePosition(mapObject.MapPosition, zPosition);

                var objectHoverBox = new Rectangle();
                var inHitboxHoverBox = false;

                // Objects
                var isShowingObject = !MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowObjects;
                if (!isShowingObject)
                {
                    if (!mapObjectPage.IsCharacter)
                    {
						mapObjectPosition = MapRenderer.GetCharacterImagePosition(mapObject.MapPosition, zPosition);
						var topLeft = new Point(mapObjectPosition.X - mapObjectPage.TexW / 2, mapObjectPosition.Y - mapObjectPage.TexH / 2);

                        objectHoverBox = new Rectangle(topLeft, new Size(mapObjectPage.TexW, mapObjectPage.TexH));
                    }
                    else
                    {
                        var characterSize = new Size(64, 96);
                        var sizeAdjustment = new Point(0, -32);
                        var topLeft = new Point(mapObjectPosition.X - characterSize.Width / 2, mapObjectPosition.Y - characterSize.Height / 2 - 40);

                        var adjustedTopLeft = new Point(topLeft.X - sizeAdjustment.X / 2, topLeft.Y - sizeAdjustment.Y);
                        var adjustedBottomRight = new Point(topLeft.X + characterSize.Width + sizeAdjustment.X / 2, topLeft.Y + characterSize.Height);

                        objectHoverBox = MapRenderer.RectangleFromPoints(adjustedTopLeft, adjustedBottomRight);
                    }
                }

                // Hitboxes
                var isShowingHitbox = !MapRenderer.DisplayOptions.Any(t => t.Item1 == levelIndex) || !MapRenderer.DisplayOptions.FirstOrDefault(t => t.Item1 == levelIndex).Item2.ShowHitboxes;
                if (!isShowingHitbox)
                {
                    var hitWidth = mapObjectPage.HitRange.Width;
                    var hitHeight = mapObjectPage.HitRange.Height;
                    int hitShiftPosX, hitShiftPosY;
                    if (mapObjectPage.HitForm == HitFormType.Circle)
                    {
                        var adjustedHitShift = mapObjectPage.HitShift;
                        if (mapObject is MapMystery || mapObjectPage.IsWarp)
                        {
                            if (mapObjectPage.IsWarp)
                            {
                                hitWidth = 6;
                            }
                            adjustedHitShift = new Point(mapObjectPage.HitShift.X - 10, mapObjectPage.HitShift.Y - 10);
                        }
                        hitShiftPosX = mapObject.X + adjustedHitShift.X - hitWidth;
                        hitShiftPosY = mapObject.Y + adjustedHitShift.Y - hitWidth;
                        hitWidth *= 2;
                        hitHeight = hitWidth;
                    }
                    else if (mapObjectPage.StartTerms == StartTermType.Touch)
                    {
                        // TODO: figure out real equation
                        hitShiftPosX = mapObject.X + mapObjectPage.HitShift.X - (int)(hitWidth / Math.Sqrt(2)) - 8;
                        hitShiftPosY = mapObject.Y + mapObjectPage.HitShift.Y - (int)(hitHeight / Math.Sqrt(2)) - 8;
                    }
                    else
                    {
                        hitShiftPosX = mapObject.X + mapObjectPage.HitShift.X - hitWidth / 2 - 8;
                        hitShiftPosY = mapObject.Y + mapObjectPage.HitShift.Y - hitHeight / 2 - 8;
                    }
                    var topRight = new Point(hitShiftPosX + hitWidth, hitShiftPosY);
                    var bottomRight = new Point(hitShiftPosX + hitWidth, hitShiftPosY + hitHeight);
                    var bottomLeft = new Point(hitShiftPosX, hitShiftPosY + hitHeight);
                    var topLeft = new Point(hitShiftPosX, hitShiftPosY);
                    var hitboxQuad = new Quad
                    {
                        A = MapRenderer.GetImagePosition(topRight, zPosition),
                        B = MapRenderer.GetImagePosition(bottomRight, zPosition),
                        C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
                        D = MapRenderer.GetImagePosition(topLeft, zPosition),
                    };

                    inHitboxHoverBox = hitboxQuad.Contains(p);
                }

                return (objectHoverBox.Contains(p) || inHitboxHoverBox);
            }).ToList();

            var hoveredObjectCount = hoveredObjects.Count();
            if (hoveredObjectCount > 0)
            {
                var layerOffset = ((ushort)MapRenderer.HoverLayer) % hoveredObjectCount;
                hoveredObjects.Reverse();
                return hoveredObjects[layerOffset];
            }
            else
            {
                return null;
            }
        }

		private static void DrawMovePath(int width, Point start, Point end, int zPosition, Color color, int renderPass)
		{
			Point topRight, bottomRight, bottomLeft, topLeft;
			topRight = bottomRight = bottomLeft = topLeft = new Point();
			// North
			if (start.Y > end.Y)
			{
				topRight = new Point(end.X + width, end.Y - width);
				bottomRight = new Point(start.X + width, start.Y + width);
				bottomLeft = new Point(start.X - width, start.Y + width);
				topLeft = new Point(end.X - width, end.Y - width);
			}
			// South
			else if (start.Y < end.Y)
			{
				topRight = new Point(start.X + width, start.Y - width);
				bottomRight = new Point(end.X + width, end.Y + width);
				bottomLeft = new Point(end.X - width, end.Y + width);
				topLeft = new Point(start.X - width, start.Y - width);
			}
			// East
			else if (start.X < end.X)
			{
				topRight = new Point(end.X + width, end.Y - width);
				bottomRight = new Point(end.X + width, end.Y + width);
				bottomLeft = new Point(start.X - width, start.Y + width);
				topLeft = new Point(start.X - width, start.Y - width);
			}
			// West
			else if (start.X > end.X)
			{
				topRight = new Point(start.X + width, start.Y - width);
				bottomRight = new Point(start.X + width, start.Y + width);
				bottomLeft = new Point(end.X - width, end.Y + width);
				topLeft = new Point(end.X - width, end.Y - width);
			}
			else
			{
				topRight = new Point(start.X + width, start.Y - width);
				bottomRight = new Point(start.X + width, start.Y + width);
				bottomLeft = new Point(start.X - width, start.Y + width);
				topLeft = new Point(start.X - width, start.Y - width);
			}

			MapRenderer.DrawQuadAndExpandMap(new Quad
			{
				A = MapRenderer.GetImagePosition(topRight, zPosition),
				B = MapRenderer.GetImagePosition(bottomRight, zPosition),
				C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
				D = MapRenderer.GetImagePosition(topLeft, zPosition),
				Color = color,
				Type = DrawType.Fill
			}, renderPass);
		}

		private static void DrawMovePathArrows(AngleTypeNumber angle, Point start, Point end, int zPosition, Color color, int renderPass)
		{
			var width = 4;

			var condition = default(Func<Point, bool>);
			var iterator = default(Func<Point, Point>);
			// North
			if (start.Y > end.Y)
			{
				condition = (p) => p.Y > end.Y;
				iterator = (p) => new Point(p.X, p.Y - 8);
			}
			// South
			else if (start.Y < end.Y)
			{
				condition = (p) => p.Y < end.Y;
				iterator = (p) => new Point(p.X, p.Y + 8);
			}
			// East
			else if (start.X < end.X)
			{
				condition = (p) => p.X < end.X;
				iterator = (p) => new Point(p.X + 8, p.Y);
			}
			// West
			else if (start.X > end.X)
			{
				condition = (p) => p.X > end.X;
				iterator = (p) => new Point(p.X - 8, p.Y);
			}
			else
			{
				var drawnOnce = false;
				condition = (p) => !drawnOnce;
				iterator = (p) =>
				{
					drawnOnce = true;
					return p;
				};
			}

			for(Point p = start; condition(p); p = iterator(p))
			{
				Point topRight, bottomRight, bottomLeft, topLeft;
				topRight = bottomRight = bottomLeft = topLeft = new Point();

				switch (angle)
				{
					case AngleTypeNumber.North:
						topRight = new Point(p.X, p.Y - width);
						bottomRight = new Point(p.X + width, p.Y + width);
						bottomLeft = new Point(p.X - width, p.Y + width);
						topLeft = new Point(p.X, p.Y - width);
						break;
					case AngleTypeNumber.South:
						topRight = new Point(p.X, p.Y + width);
						bottomRight = new Point(p.X + width, p.Y - width);
						bottomLeft = new Point(p.X - width, p.Y - width);
						topLeft = new Point(p.X, p.Y + width);
						break;
					case AngleTypeNumber.East:
						topRight = new Point(p.X + width, p.Y);
						bottomRight = new Point(p.X + width, p.Y);
						bottomLeft = new Point(p.X - width, p.Y + width);
						topLeft = new Point(p.X - width, p.Y - width);
						break;
					case AngleTypeNumber.West:
						topRight = new Point(p.X + width, p.Y - width);
						bottomRight = new Point(p.X + width, p.Y + width);
						bottomLeft = new Point(p.X - width, p.Y);
						topLeft = new Point(p.X - width, p.Y);
						break;
				}

				MapRenderer.DrawQuadAndExpandMap(new Quad
				{
					A = MapRenderer.GetImagePosition(topRight, zPosition),
					B = MapRenderer.GetImagePosition(bottomRight, zPosition),
					C = MapRenderer.GetImagePosition(bottomLeft, zPosition),
					D = MapRenderer.GetImagePosition(topLeft, zPosition),
					Color = color,
					Type = DrawType.Fill
				}, renderPass);
			}
		}
	}
}
