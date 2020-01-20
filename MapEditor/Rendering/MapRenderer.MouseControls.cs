using System;
using System.Drawing;
using System.Windows.Forms;

namespace MapEditor.Rendering
{
    public static partial class MapRenderer
    {
        private static Point? MousePosition { get; set; }
        private static Point? MouseDownPosition { get; set; }
        private static Rectangle? MouseDragRectangle { get; set; }
        private static Rectangle? MouseDragRelease { get; set; }
        private static Point? MouseClickPosition { get; set; }

        private static void LevelControlMouseDown(object sender, MouseEventArgs e)
        {
            MapRenderer.MouseDownPosition = new Point(e.X + MapRenderer.LevelRenderer.Origin.X, e.Y + MapRenderer.LevelRenderer.Origin.Y);
        }

        private static void LevelControlMouseMove(object sender, MouseEventArgs e)
        {
            MapRenderer.MousePosition = new Point(e.X + MapRenderer.LevelRenderer.Origin.X, e.Y + MapRenderer.LevelRenderer.Origin.Y);
            if (MapRenderer.MouseDownPosition.HasValue && e.Button.HasFlag(MouseButtons.Left))
            {
                var dragRectangle = new Rectangle(MapRenderer.MouseDownPosition.Value, new Size(
                    e.X + MapRenderer.LevelRenderer.Origin.X - MapRenderer.MouseDownPosition.Value.X,
                    e.Y + MapRenderer.LevelRenderer.Origin.Y - MapRenderer.MouseDownPosition.Value.Y
                ));

                MapRenderer.MouseDragRectangle = dragRectangle.Size.IsEmpty ? (Rectangle?)null : dragRectangle;
            }
        }

        private static void LevelControlMouseUp(object sender, MouseEventArgs e)
        {
            if (MapRenderer.MouseDragRectangle.HasValue)
            {
                MapRenderer.MouseDragRelease = MapRenderer.MouseDragRectangle;
            }
            else
            {
                MapRenderer.MouseClickPosition = new Point(e.X + MapRenderer.LevelRenderer.Origin.X, e.Y + MapRenderer.LevelRenderer.Origin.Y);
            }
            MapRenderer.MouseDownPosition = null;
            MapRenderer.MouseDragRectangle = null;
        }

        private static void LevelControlMouseLeave(object sender, EventArgs e)
        {
            MapRenderer.MousePosition = null;
        }
    }
}
