using Common.Vectors;
using System.Drawing;

namespace Common.OpenGL
{
    public class Sprite
    {
        public PointF Position
        {
            get
            {
                return new PointF(this.X, this.Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }
        public float X { get; set; }
        public float Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int TexX { get; set; }
        public int TexY { get; set; }

        public string Texture { get; set; }

        public Vector2 Scale { get; set; } = Vector2.One;
        public float Rotate { get; set; }

        public Color ColorModulation { get; set; } = Color.White;

        public Sprite WithScale(Vector2 scale)
        {
            this.Scale = scale;
            return this;
        }

        public Sprite WithRotate(float rotate)
        {
            this.Rotate = rotate;
            return this;
        }

        public Sprite WithTopLeftPosition()
        {
            this.X += (float)(this.Scale.X * this.Width / 2.0);
            this.Y += (float)(this.Scale.Y * this.Height / 2.0);
            return this;
        }
    };
}
