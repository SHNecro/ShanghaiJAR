using System;
using System.Drawing;

namespace Common.OpenGL
{
    public interface ISpriteRenderer
    {
        Point Origin { get; set; }

        double RenderScale { get; set; }
        double RenderScaleX { get; set; }
        double RenderScaleY { get; set; }

        event Action<OriginChangedEventArgs> OriginChanged;

        void Draw(Sprite sprite, int renderPass);

        void DrawLevel(int x, int y, string texture, int renderPass);

        void DrawTiledBackground(Tile backgroundTile);

        void SetClearColor(Color color);

        void DrawQuad(Quad q, int renderPass);

        void DrawText(Text t, int renderPass);

        Size? GetTextureSize(string textureName);

		void Render();
    }
}
