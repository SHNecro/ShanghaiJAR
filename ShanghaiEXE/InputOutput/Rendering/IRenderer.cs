using System;
using System.Drawing;
using NSGame;
using Common.Vectors;

namespace NSShanghaiEXE.InputOutput.Rendering
{
    public interface IRenderer : IDisposable
    {
        event EventHandler<TextureLoadProgressUpdatedEventArgs> ProgressUpdated;

        void Begin(Color color);
        void DrawImage(IRenderer device, string texture, Rectangle spriteRect, bool leftpoint, Vector2 position, bool reversed, Color color);
        void DrawImage(IRenderer device, string texture, Rectangle spriteRect, bool leftpoint, Vector2 position, Color color);
        void DrawImage(IRenderer device, string texture, Rectangle spriteRect, bool leftpoint, Vector2 position, float scale, float rotation, bool reversed, Color color);
        void DrawImage(IRenderer device, string texture, Rectangle spriteRect, bool leftpoint, Vector2 position, float scale, float rotation, Color color);

        // Micro text: Map name
        void DrawMicroText(string text, Vector2 position, Color color);

        // Mini text: Chip, subchip descriptions
        void DrawMiniText(string text, Vector2 position, Color color);
        void DrawMiniText(string text, Vector2 position, Color color, SaveData save);

        // Text: Dialogue, savegame, others
        void DrawText(string text, Vector2 position);
        void DrawText(string text, Vector2 position, bool shadow);
        void DrawText(string text, Vector2 position, bool shadow, SaveData save);
        void DrawText(string text, Vector2 position, Color color);
        void DrawText(string text, Vector2 position, Color color, SaveData save);
        void DrawText(string text, Vector2 position, SaveData save);
        void End();
        ITextMeasurer GetTextMeasurer();
        void LoadTexture(string texture);
        void AbortRenderThread();
    }
}