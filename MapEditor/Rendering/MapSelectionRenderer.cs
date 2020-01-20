using MapEditor.Controls;
using Common.OpenGL;
using MapEditor.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using MapEditor.ViewModels;
using Common.EncodeDecode;

namespace MapEditor.Rendering
{
    public static partial class MapSelectionRenderer
    {
        private const int RenderPassPadding = 5;

        private static string currentMapName;

        static MapSelectionRenderer()
        {
            var mapRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            MapSelectionRenderer.MapRenderer = mapRendererPanel;
            MapSelectionRenderer.MapControl = mapRendererPanel;

            SpriteRendererPanel.TexturesReloaded += () =>
            {
                mapRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
            };
        }

        public static WindowsFormsHost MapControlHost
        {
            get
            {
                var renderingControl = MapSelectionRenderer.MapControl.GetControl();
                var scrollFormsHost = new ScrollViewerWindowsFormsHost { Child = renderingControl };
                renderingControl.Paint += (s, e) => MapSelectionRenderer.DrawMap();
                renderingControl.Paint += (s, e) =>
                {
                    var maxSize = default(Size?);
                    for (int i = 1; i < MapSelectionRenderer.CurrentMap.Header.Levels * 2; i++)
                    {
                        var filename = $"{MapSelectionRenderer.CurrentMap.Header.ImagePrefix}{i}";
                        var textureSize = MapSelectionRenderer.MapRenderer.GetTextureSize(filename);
                        if (textureSize.HasValue)
                        {
                            var width = Math.Max(maxSize?.Width ?? 0, textureSize?.Width ?? 0);
                            var height = Math.Max(maxSize?.Height ?? 0, textureSize?.Height ?? 0);
                            maxSize = new Size(width, height);
                        }
                    }
                    if (maxSize.HasValue)
                    {
                        scrollFormsHost.Width = maxSize.Value.Width;
                        scrollFormsHost.Height = maxSize.Value.Height;
                    }
                };
                renderingControl.MouseWheel += (sender, args) =>
                {
                    var sv = scrollFormsHost.ParentScrollViewer;
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        sv.ScrollToHorizontalOffset(sv.HorizontalOffset - args.Delta);
                    }
                    else
                    {
                        sv.ScrollToVerticalOffset(sv.VerticalOffset - args.Delta);
                    }
                };
                return scrollFormsHost;
            }
        }

        public static string CurrentMapName
        {
            get
            {
                return MapSelectionRenderer.currentMapName;
            }

            set
            {
                MapSelectionRenderer.currentMapName = value;

                var filePath = $"{LoadingWindowViewModel.Settings.MapDataFolder}/{MapSelectionRenderer.CurrentMapName}.she";

                if (!File.Exists(filePath))
                {
                    filePath = $"{LoadingWindowViewModel.Settings.MapDataFolder}/{MapSelectionRenderer.MapFiles.First()}.she";
                }

                var mapContents = TCDEncodeDecode.ReadTextFile(filePath, true);

                MapSelectionRenderer.CurrentMap = new Map { StringValue = mapContents, Name = MapSelectionRenderer.CurrentMapName };

                MapSelectionRenderer.DrawMapImages();
            }
        }

        public static Map CurrentMap { get; set; }

        public static IEnumerable<string> MapFiles
        {
            get
            {
                return Directory.GetFiles(LoadingWindowViewModel.Settings.MapDataFolder)
                    .Where(f => f.EndsWith(".she", StringComparison.InvariantCultureIgnoreCase))
                    .Select(f => Path.GetFileNameWithoutExtension(f));
            }
        }

        private static IMouseInteractionControl MapControl { get; }
        private static ISpriteRenderer MapRenderer { get; }

        private static void DrawMap()
        {
            MapSelectionRenderer.DrawMapImages();
            MapSelectionRenderer.DrawObjectsAndHitBoxes();
            MapSelectionRenderer.DrawSelectedEventMarkers();
            MapSelectionRenderer.MapRenderer.Render();
        }
    }
}
