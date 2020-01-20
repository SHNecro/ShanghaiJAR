using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Common.OpenGL
{
    [Flags]
	public enum DrawType
    {
        Fill = 1,
        Outline = 2
	}

	public class Quad
	{
		public Point A { get; set; }
		public Point B { get; set; }
		public Point C { get; set; }
		public Point D { get; set; }

		public Color Color { get; set; }

		public DrawType Type { get; set; }

        public Quad Copy() => new Quad { A = this.A, B = this.B, C = this.C, D = this.D, Color = this.Color, Type = this.Type };

        public bool Contains(Point p)
        {
            GraphicsPath g = new GraphicsPath();
            g.AddPolygon(new Point[] { this.A, this.B, this.C, this.D });
            return g.IsVisible(p);
        }
    }
}
