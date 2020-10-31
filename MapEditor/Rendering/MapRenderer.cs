using Common.OpenGL;
using MapEditor.Controls;
using MapEditor.Core;
using MapEditor.Models;
using MapEditor.Models.Elements.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;

namespace MapEditor.Rendering
{
    public static partial class MapRenderer
    {
        private const double MapExpandStep = 50;

        private const int MapXBuffer = 80;
        private const int MapYBuffer = 120;

        private const int RenderPassPadding = 5;

        public static event EventHandler CurrentLevelUpdated;
        public static event EventHandler CurrentToolUpdated;

        private static int currentLevel;
        private static EditToolType currentTool;
        
        private static Point? draggedMapObjectOffset;

        static MapRenderer()
        {
            var spriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            MapRenderer.LevelRenderer = spriteRendererPanel;
            MapRenderer.LevelControl = spriteRendererPanel;
            MapRenderer.DisplayOptions = new ObservableCollection<Tuple<int, LevelDisplayOptions>>();
            MapRenderer.MapDisplayOptions = new MapDisplayOptions();
            MapRenderer.Frame = 0;
            MapRenderer.WalkablePreview = new Dictionary<Tuple<int, int, int>, int>();
            MapRenderer.CurrentLevel = 0;

            MapRenderer.IsDrawing = false;

            SpriteRendererPanel.TextureLoad += () => MapRenderer.IsDrawing = true;

            MapRenderer.LevelControl.ScaledMouseDown += MapRenderer.LevelControlMouseDown;
            MapRenderer.LevelControl.ScaledMouseMove += MapRenderer.LevelControlMouseMove;
            MapRenderer.LevelControl.ScaledMouseUp += MapRenderer.LevelControlMouseUp;
            MapRenderer.LevelControl.ScaledMouseLeave += MapRenderer.LevelControlMouseLeave;

            SpriteRendererPanel.TexturesReloaded += () =>
            {
                spriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
            };
        }

        public static MapObject CurrentMapObject
        {
            get
            {
                return MapRenderer.CurrentMap.MapObjects.SelectedObject;
            }
            set
            {
                MapRenderer.CurrentMap.MapObjects.SelectedObject = value;
                if (MapRenderer.CurrentMapObject?.Pages.SelectedEventPage != null && SpriteSelectionWindow.HasBeenShown)
                {
                    SpriteSelectionWindow.SetPageSetterAction((page) =>
                    {
                        MapRenderer.CurrentMapObject.Pages.SelectedEventPage = page;
                    });
                    SpriteSelectionWindow.SetWindowEnable(!(MapRenderer.CurrentMapObject is MapMystery));
                    SpriteSelectionWindow.RefreshWindow(MapRenderer.CurrentMapObject.Pages.SelectedEventPage);
                }
            }
        }

        public static Action<Point?, int, string> MapPositionUpdateFunc { get; set; }

        public static MapObject MapHoveredMapObject { get; set; }
        public static MapObject ListHoveredMapObject { get; set; }
        public static List<MapObject> RenderSortedObjects { get; set; }
        public static int HoverLayer { get; set; }
        public static bool HoverLayerChanged { get; set; }
        public static Move ListHoveredMove { get; set; }

		public static ISpriteRenderer LevelRenderer { get; }
        public static IMouseInteractionControl LevelControl { get; }

        public static Map CurrentMap { get; set; }
        public static ObservableCollection<Tuple<int, LevelDisplayOptions>> DisplayOptions { get; set; }
        public static MapDisplayOptions MapDisplayOptions { get; set; }

        public static int CurrentLevel
        {
            get
            {
                return MapRenderer.currentLevel;
            }
            set
            {
                MapRenderer.currentLevel = value;
                MapRenderer.CurrentLevelUpdated?.Invoke(null, null);
            }
        }
        public static EditToolType CurrentTool
        {
            get
            {
                return MapRenderer.currentTool;
            }
            set
            {
                MapRenderer.currentTool = value;
                MapRenderer.CurrentToolUpdated?.Invoke(null, null);
            }
        }

        public static WalkableTileType SelectedTile { get; set; }


        public static bool IsPlacingConveyors { get; set; }

        public static ConveyorColorType ConveyorColor { get; set; }

        public static Point? WindowOrigin { get; set; }
        public static Size? WindowSize { get; set; }

        public static WindowsFormsHost MapControlHost
        {
            get
            {
                var renderingControl = MapRenderer.LevelControl.GetControl();
                var scrollFormsHost = new ScrollViewerWindowsFormsHost { Child = renderingControl };
                MapRenderer.LevelRenderer.OriginChanged += (args) =>
                {
                    var sv = scrollFormsHost.ParentScrollViewer;
                    if (sv == null)
                    {
                        return;
                    }

                    var horizDelta = args.NewOrigin.X - args.PreviousOrigin.X;
                    if (horizDelta != 0)
                    {
                        sv.ScrollToHorizontalOffset(sv.HorizontalOffset - horizDelta);
                    }

                    var vertDelta = args.NewOrigin.Y - args.PreviousOrigin.Y;
                    if (vertDelta != 0)
                    {
                        sv.ScrollToVerticalOffset(sv.VerticalOffset - vertDelta);
                    }
                };
                renderingControl.MouseWheel += (sender, args) =>
                {
                    var sv = scrollFormsHost.ParentScrollViewer;
                    var shiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                    var ctrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                    if (ctrlDown && shiftDown)
                    {

                        if (args.Delta < 0 && MapRenderer.LevelRenderer.RenderScale > 1)
                        {
                            MapRenderer.LevelRenderer.RenderScale--;
                        }
                        else if (args.Delta > 0)
                        {
                            MapRenderer.LevelRenderer.RenderScale++;
                        }
                    }
                    else if (shiftDown)
                    {
                        sv.ScrollToHorizontalOffset(sv.HorizontalOffset - args.Delta);
                    }
                    else if (ctrlDown)
                    {
                        Func<int> levelGetter = null;
                        Action<int> levelSetter = null;

                        switch (MapRenderer.CurrentTool)
                        {
                            case EditToolType.SelectionTool:
                                levelGetter = () => MapRenderer.HoverLayer;
                                levelSetter = (value) => 
                                {
                                    MapRenderer.HoverLayer = value;
                                    MapRenderer.HoverLayerChanged = true;
                                };
                                break;
                            case EditToolType.DrawTool:
                                levelGetter = () => MapRenderer.CurrentLevel;
                                levelSetter = (value) =>
                                {
                                    if (value >= 0 && value <= MapRenderer.CurrentMap.Header.Levels)
                                    {
                                        MapRenderer.CurrentLevel = value;
                                    }
                                };
                                break;
                        }

                        if (args.Delta > 0)
                        {
                            levelSetter(levelGetter() - 1);
                        }
                        else if (args.Delta < 0)
                        {
                            levelSetter(levelGetter() + 1);
                        }
                    }
                    else
                    {
                        sv.ScrollToVerticalOffset(sv.VerticalOffset - args.Delta);
                    }
                };
                renderingControl.PreviewKeyDown += (sender, args) =>
                {
                    switch (args.KeyCode)
                    {
                        case Keys.Right:
                        case Keys.Left:
                        case Keys.Up:
                        case Keys.Down:
                        case Keys.Shift | Keys.Right:
                        case Keys.Shift | Keys.Left:
                        case Keys.Shift | Keys.Up:
                        case Keys.Shift | Keys.Down:
                            args.IsInputKey = true;
                            break;
                    }
                };

                renderingControl.KeyDown += (sender, args) =>
                {
                    var nudgeDistance = 1;

                    if (args.Modifiers.HasFlag(Keys.Shift))
                    {
                        nudgeDistance *= 8;
                    }

                    if (args.KeyCode == Keys.Delete)
                    {
                        Constants.DeleteItemCommand.Execute(new object[] { MapRenderer.CurrentMap?.MapObjects?.MapObjects, MapRenderer.CurrentMapObject });
                    }
                    else if (args.KeyCode == Keys.D && args.Modifiers.HasFlag(Keys.Control))
                    {
                        Constants.DuplicateItemCommand.Execute(new object[] { MapRenderer.CurrentMap?.MapObjects?.MapObjects, MapRenderer.CurrentMapObject });
                    }
                    else if (args.KeyCode == Keys.Up)
                    {
                        var position = MapRenderer.CurrentMapObject?.MapPosition;
                        if (position != null)
                        {
                            var newPosition = args.Modifiers.HasFlag(Keys.Alt)
                                ? new Point(position.Value.X - nudgeDistance, position.Value.Y - nudgeDistance)
                                : new Point(position.Value.X, position.Value.Y - nudgeDistance);
                            MapRenderer.CurrentMapObject.SetMapPosition(newPosition, false);
                        }
                    }
                    else if (args.KeyCode == Keys.Down)
                    {
                        var position = MapRenderer.CurrentMapObject?.MapPosition;
                        if (position != null)
                        {
                            var newPosition = args.Modifiers.HasFlag(Keys.Alt)
                                ? new Point(position.Value.X + nudgeDistance, position.Value.Y + nudgeDistance)
                                : new Point(position.Value.X, position.Value.Y + nudgeDistance);
                            MapRenderer.CurrentMapObject.SetMapPosition(newPosition, false);
                        }
                    }
                    else if (args.KeyCode == Keys.Left)
                    {
                        var position = MapRenderer.CurrentMapObject?.MapPosition;
                        if (position != null)
                        {
                            var newPosition = args.Modifiers.HasFlag(Keys.Alt)
                                ? new Point(position.Value.X - nudgeDistance, position.Value.Y + nudgeDistance)
                                : new Point(position.Value.X - nudgeDistance, position.Value.Y);
                            MapRenderer.CurrentMapObject.SetMapPosition(newPosition, false);
                        }
                    }
                    else if (args.KeyCode == Keys.Right)
                    {
                        var position = MapRenderer.CurrentMapObject?.MapPosition;
                        if (position != null)
                        {
                            var newPosition = args.Modifiers.HasFlag(Keys.Alt)
                                ? new Point(position.Value.X + nudgeDistance, position.Value.Y - nudgeDistance)
                                : new Point(position.Value.X + nudgeDistance, position.Value.Y);
                            MapRenderer.CurrentMapObject.SetMapPosition(newPosition, false);
                        }
                    }
                };

                renderingControl.KeyUp += (sender, args) =>
                {
                    MapRenderer.CurrentMapObject?.SetMapPosition(MapRenderer.CurrentMapObject.MapPosition, true);
                };
                return scrollFormsHost;
            }
        }

        public static bool IsDrawing { get; set; }

        private static Dictionary<Tuple<int, int, int>, int> WalkablePreview { get; set; }

        private static int Frame { get; set; }

        public static void Refresh()
        {
            MapRenderer.SetViewPort(new Point(-MapRenderer.MapXBuffer / 2, -MapRenderer.MapYBuffer / 2),
                new Size(MapRenderer.CurrentMap.Header.ImageSize.Width + MapRenderer.MapXBuffer, MapRenderer.CurrentMap.Header.ImageSize.Height + MapRenderer.MapYBuffer));
        }

        public static void SetViewPort(Point origin, Size size)
        {
            var steppedOrigin = new Point((int)(Math.Floor(origin.X / MapRenderer.MapExpandStep) * MapRenderer.MapExpandStep), (int)(Math.Floor(origin.Y / MapRenderer.MapExpandStep) * MapRenderer.MapExpandStep));
            var steppedSize = new Size((int)(Math.Ceiling(size.Width / MapRenderer.MapExpandStep) * MapRenderer.MapExpandStep), (int)(Math.Ceiling(size.Height / MapRenderer.MapExpandStep) * MapRenderer.MapExpandStep));
            MapRenderer.WindowOrigin = steppedOrigin;
            MapRenderer.WindowSize = steppedSize;

            var bufferedOffset = new Point(origin.X - MapRenderer.MapXBuffer, origin.Y - MapRenderer.MapYBuffer);
            var bufferedSize = new Size(size.Width + 2 * MapRenderer.MapXBuffer, size.Height + 2 * MapRenderer.MapYBuffer);
            MapRenderer.LevelRenderer.Origin = bufferedOffset;
            MapRenderer.LevelControl.SetSize(bufferedSize);
        }

        public static void Draw()
        {
            if (MapRenderer.IsDrawing)
            {
                MapRenderer.DrawMap();

                MapRenderer.LevelRenderer.Render();

                if (MapRenderer.MapDisplayOptions.IsAnimating)
                {
                    MapRenderer.Frame += 1;
                }
            }
        }

        private static void DrawMap()
        {
            MapRenderer.DrawBackground();
            
            MapRenderer.DrawMapImages();
            
            MapRenderer.DrawWalkable();
            
            MapRenderer.DrawWalkableOutline();

            switch (MapRenderer.CurrentTool)
            {
                case EditToolType.DrawTool:
                    MapRenderer.DrawWalkableEditing();
                    break;
                case EditToolType.SelectionTool:
                    MapRenderer.DrawObjectSelection();
                    break;
                default:
                    MapRenderer.MouseDragRelease = null;
                    MapRenderer.MouseClickPosition = null;
                    break;
            }

            MapRenderer.DrawObjectsAndHitBoxes();

			MapRenderer.DrawSelectedObjectOutlines();

            MapRenderer.DrawSelectedObjectMoves();

            MapRenderer.DrawSelectedEventMarkers();
        }
    }
}
