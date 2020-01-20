using System.Drawing;

namespace MapEditor.Core
{
    public class DrawCall
    {
        public string TextureName { get; set; }
        public Point TexturePosition { get; set; }
        public bool IsFromTopLeft { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }
        public bool IsReversed { get; set; }
        public double Scale { get; set; }
        public double Rotate { get; set; }
        public Color Color { get; set; }
    }
}
