using MapEditor.Core;
using Common.OpenGL;
using System.Drawing;

namespace MapEditor.Rendering
{
    public static partial class AddEditTranslationRenderer
    {
        public static void DrawDialogue()
        {
            if (AddEditTranslationRenderer.CurrentEntry?.Dialogue?.Face != null)
            {
                AddEditTranslationRenderer.DialogueRenderer.Draw(new Sprite
                {
                    Texture = "window",
                    Width = 240,
                    Height = 56,
                    TexX = 0,
                    TexY = 0,
                    Position = new Point(0, 0)
                }.WithTopLeftPosition(), 1);

                var face = AddEditTranslationRenderer.CurrentEntry.Dialogue.Face;
                AddEditTranslationRenderer.DialogueRenderer.Draw(new Sprite
                {
                    Texture = $"Face{face.Sheet}",
                    Width = 40,
                    Height = 48,
                    TexX = face.Mono ? (40 * 5) : 40 * 0,
                    TexY = 48 * face.Index,
                    Position = new Point(5, 4)
                }.WithTopLeftPosition(), 2);

                for (var i = 0; i < 3; i++)
                {
                    AddEditTranslationRenderer.DialogueRenderer.DrawText(new Text
                    {
                        Content = AddEditTranslationRenderer.CurrentEntry.Dialogue[i],
                        Color = Constants.TextColor,
                        Font = Constants.Fonts[FontType.Normal],
                        Position = new Point(48 - 1, (16 * i))
                    }, 2);
                }

                AddEditTranslationRenderer.DialogueRenderer.Draw(new Sprite
                {
                    Texture = "window",
                    Width = 16,
                    Height = 16,
                    TexX = 240 + (16 * (AddEditTranslationRenderer.Frame % 3)),
                    TexY = 0,
                    Position = new Point(224, 36)
                }.WithTopLeftPosition(), 2);
            }

            AddEditTranslationRenderer.DialogueRenderer.Render();
        }
    }
}
