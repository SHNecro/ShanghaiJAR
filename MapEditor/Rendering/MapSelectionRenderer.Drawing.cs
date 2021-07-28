using Common.OpenGL;
using Common.Vectors;
using Data;
using MapEditor.Models.Elements.Events;
using MapEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapEditor.Rendering
{
    public static partial class MapSelectionRenderer
    {
        public static void DrawMapImages()
        {
            for (int i = 1; i < MapSelectionRenderer.CurrentMap.Header.Levels * 2; i++)
            {
                var renderPass = RenderPassPadding * ((MapSelectionRenderer.CurrentMap.Header.Levels * 2) - (i % 2 == 0 ? i - 1 : i));
                var filename = $"{MapSelectionRenderer.CurrentMap.Header.ImagePrefix}{i}";
                MapSelectionRenderer.MapRenderer.DrawLevel(0, 0, $"{MapSelectionRenderer.CurrentMap.Header.ImagePrefix}{i}", renderPass);
            }
        }

        public static void DrawObjectsAndHitBoxes()
        {
            var allObjects = MapSelectionRenderer.CurrentMap.MapObjects.MapObjects;
            var sortedObjects = MapEditor.Rendering.MapRenderer.RendSort(allObjects);
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
                    var zPosition = MapSelectionRenderer.GetCPosition(mapObject.Level);

                    var renderPass = RenderPassPadding * ((MapSelectionRenderer.CurrentMap.Header.Levels - mapObject.Level) * 2) + mapObjectPage.RendType;
                    var mapObjectPosition = MapSelectionRenderer.GetImagePosition(mapObject.MapPosition, zPosition);

                    // Objects
                    if (!mapObjectPage.IsCharacter)
                    {
                        mapObjectPosition = MapSelectionRenderer.GetCharacterImagePosition(mapObject.MapPosition, zPosition);
                        MapSelectionRenderer.MapRenderer.Draw(new Sprite
                        {
                            Position = mapObjectPosition,
                            TexX = mapObjectPage.TexX,
                            TexY = mapObjectPage.TexY,
                            Width = mapObjectPage.TexW,
                            Height = mapObjectPage.TexH,
                            Texture = $"body{mapObjectPage.GraphicsIndex}"
                        }, renderPass);
                    }
                    else
                    {
                        bool reversed = mapObjectPage.Angle == 3 || mapObjectPage.Angle == 7;
                        var noShadow = CharacterInfo.IsNoShadowCharacter(mapObjectPage.GraphicsIndex, mapObjectPage.CharacterIndex);
                        if (!noShadow)
                        {
                            var shadowPosition = mapObjectPosition;
                            shadowPosition.Offset(0, -16);
                            MapSelectionRenderer.MapRenderer.Draw(new Sprite
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
                        MapSelectionRenderer.MapRenderer.Draw(new Sprite
                        {
                            Position = mapObjectPosition,
                            TexX = mapObjectPage.TexX,
                            TexY = mapObjectPage.TexY,
                            Width = 64,
                            Height = 96,
                            Texture = $"charachip{mapObjectPage.GraphicsIndex}",
                            Scale = new Vector2(reversed ? -1 : 1, 1)
                        }, renderPass);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
                }
            }
        }

        public static void DrawSelectedEventMarkers()
        {
            var color = Color.FromArgb(128, Color.Red);
            var selectedMapObject = MainWindowViewModel.GetCurrentMap()?.MapObjects?.SelectedObject;
            var selectedEvent = selectedMapObject?.Pages?.SelectedEventPage?.Events?.SelectedEvent?.Instance;

            if (selectedEvent == null)
            {
                return;
            }

            var mapObjects = MapSelectionRenderer.CurrentMap.MapObjects.MapObjects;
            
            var position = default(Tuple<Point, int>);
            var quads = new List<Quad>();

            if (selectedEvent is MapChangeEvent || selectedEvent is MapWarpEvent || selectedEvent is WarpEvent || selectedEvent is JackInEvent || selectedEvent is JackInDirectEvent)
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
                else if (selectedEvent is MapWarpEvent)
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

                if (targetMap == MapSelectionRenderer.CurrentMap.Name)
                {
                    position = Tuple.Create(new Point(x - 10, y - 10), z);
                }
            }

            if (position != null)
            {
                var renderPass = RenderPassPadding * ((MapSelectionRenderer.CurrentMap.Header.Levels - position.Item2) * 2) + 3;

                var markerSizes = new[] { 2, 4 };
                foreach (var radius in markerSizes)
                {
                    var topLeft = MapSelectionRenderer.GetImagePosition(new Point(position.Item1.X - radius, position.Item1.Y - radius), MapSelectionRenderer.GetCPosition(position.Item2));
                    var topRight = MapSelectionRenderer.GetImagePosition(new Point(position.Item1.X + radius, position.Item1.Y - radius), MapSelectionRenderer.GetCPosition(position.Item2));
                    var bottomRight = MapSelectionRenderer.GetImagePosition(new Point(position.Item1.X + radius, position.Item1.Y + radius), MapSelectionRenderer.GetCPosition(position.Item2));
                    var bottomLeft = MapSelectionRenderer.GetImagePosition(new Point(position.Item1.X - radius, position.Item1.Y + radius), MapSelectionRenderer.GetCPosition(position.Item2));

                    MapSelectionRenderer.MapRenderer.DrawQuad(new Quad
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
                var renderPass = RenderPassPadding * ((MapSelectionRenderer.CurrentMap.Header.Levels) * 2) + 1;
                MapSelectionRenderer.MapRenderer.DrawQuad(quad, renderPass);
            }
        }
    }
}
