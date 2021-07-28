using Common.OpenGL;
using System.Drawing;

namespace MapEditor.Rendering
{
    public static partial class SpriteSelectionRenderer
    {
        public static void DrawSpriteSheet()
        {
            var renderPass = SpriteSelectionRenderer.RenderPassPadding;

            SpriteSelectionRenderer.SpriteSheetRenderer.DrawLevel(0, 0, SpriteSelectionRenderer.CurrentPage.Texture, renderPass);

            if (SpriteSelectionRenderer.CurrentPage.IsCharacter)
            {
                var size = new Size(64, 96);
                var color = SpriteSelectionRenderer.CurrentPage.Angle == 1 || SpriteSelectionRenderer.CurrentPage.Angle == 5 ? Color.Wheat : Color.Green;
                var topLeft = new Point(SpriteSelectionRenderer.CurrentPage.TexX, SpriteSelectionRenderer.CurrentPage.TexY);
                SpriteSelectionRenderer.SpriteSheetRenderer.DrawQuad(new Quad
                {
                    A = new Point(topLeft.X + size.Width, topLeft.Y),
                    B = new Point(topLeft.X + size.Width, topLeft.Y +size.Height),
                    C = new Point(topLeft.X, topLeft.Y +size.Height),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(196, color),
                    Type = DrawType.Outline
                }, 2 * renderPass);
            }
            else
            {
                var frame = 0;
                do
                {
                    frame++;
                    var topLeft = new Point(SpriteSelectionRenderer.CurrentPage.TexX + ((frame - 1) * SpriteSelectionRenderer.CurrentPage.TexW), SpriteSelectionRenderer.CurrentPage.TexY);
                    SpriteSelectionRenderer.SpriteSheetRenderer.DrawQuad(new Quad
                    {
                        A = new Point(topLeft.X + SpriteSelectionRenderer.CurrentPage.TexW, topLeft.Y),
                        B = new Point(topLeft.X + SpriteSelectionRenderer.CurrentPage.TexW, topLeft.Y + SpriteSelectionRenderer.CurrentPage.TexH),
                        C = new Point(topLeft.X, topLeft.Y + SpriteSelectionRenderer.CurrentPage.TexH),
                        D = new Point(topLeft.X, topLeft.Y),
                        Color = Color.FromArgb(196, frame == 1 ? Color.Wheat : Color.Green),
                        Type = DrawType.Outline
                    }, 2 * renderPass);
                } while (frame < SpriteSelectionRenderer.CurrentPage.Frames);
            }

            SpriteSelectionRenderer.SpriteSheetRenderer.Render();
        }
    }
}
