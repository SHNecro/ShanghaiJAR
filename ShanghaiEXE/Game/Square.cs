using Common.Vectors;
using System.Drawing;

namespace NSGame
{
    public class Square
    {
        public Vector2[] p = new Vector2[4];

        public Square(Vector2 point, Point lange)
        {
            this.p[0] = new Vector2(point.X - lange.X / 2, point.Y - lange.Y / 2);
            this.p[1] = new Vector2(point.X + lange.X / 2, point.Y - lange.Y / 2);
            this.p[2] = new Vector2(point.X - lange.X / 2, point.Y + lange.Y / 2);
            this.p[3] = new Vector2(point.X + lange.X / 2, point.Y + lange.Y / 2);
        }
    }
}
