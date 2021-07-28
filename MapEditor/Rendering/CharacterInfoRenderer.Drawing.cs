using Common.OpenGL;
using Common.Vectors;
using System.Drawing;

namespace MapEditor.Rendering
{
    public static partial class CharacterInfoRenderer
    {
        public static void DrawSpriteSheet()
        {
            var viewModel = CharacterInfoRenderer.ViewModel;
            var renderPass = CharacterInfoRenderer.RenderPassPadding;

            CharacterInfoRenderer.SpriteSheetRenderer.DrawLevel(0, 0, viewModel.CurrentCharacterSheet, renderPass);

            var size = new Size(64, 96);
            var color = viewModel.Angle == 1 || viewModel.Angle == 5 ? Color.Wheat : Color.Green;
            var topLeft = new Point(viewModel.TexX, viewModel.TexY);
            CharacterInfoRenderer.SpriteSheetRenderer.DrawQuad(new Quad
            {
                A = new Point(topLeft.X + size.Width, topLeft.Y),
                B = new Point(topLeft.X + size.Width, topLeft.Y + size.Height),
                C = new Point(topLeft.X, topLeft.Y + size.Height),
                D = new Point(topLeft.X, topLeft.Y),
                Color = Color.FromArgb(196, color),
                Type = DrawType.Outline
            }, renderPass);

            CharacterInfoRenderer.SpriteSheetRenderer.Render();
        }

        public static void DrawPreview()
        {
            var viewModel = CharacterInfoRenderer.ViewModel;
            var renderPass = CharacterInfoRenderer.RenderPassPadding;
            
            var size = new Size(64, 96);
            var reverseScale = viewModel.Angle == 1 || viewModel.Angle == 5 ? Vector2.One : new Vector2(-1, 1);
            var topLeft = new Point(viewModel.TexX, viewModel.TexY);
            var animFrame = 0;
            if (viewModel.IsFloating)
            {
                animFrame = CharacterInfoRenderer.Frame / 3 % 7;
            }
            else if (viewModel.IsWalking)
            {
                animFrame = 1 + (CharacterInfoRenderer.Frame / 3 % 6);
            }

            CharacterInfoRenderer.PreviewRenderer.Draw(new Sprite {
                Position = new Point(32, 48),
                TexX = viewModel.TexX + 64 * animFrame,
                TexY = viewModel.TexY,
                Width = 64,
                Height = 96,
                Texture = viewModel.CurrentCharacterSheet
            }.WithScale(reverseScale), renderPass);

            if (!viewModel.NoShadow)
            {
                CharacterInfoRenderer.PreviewRenderer.Draw(new Sprite
                {
                    Position = new Point(32, 72),
                    TexX = 0,
                    TexY = 384,
                    Width = 32,
                    Height = 48,
                    Texture = $"charachip1"
                }, renderPass - 1);
            }

            CharacterInfoRenderer.PreviewRenderer.Render();
        }
    }
}
