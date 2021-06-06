using Common.Vectors;

namespace NSGame
{
    public class Circle
    {
        public Vector2 point;
        public int range;

        public Circle(Vector2 p, int r)
        {
            this.point = p;
            this.range = r;
        }
    }
}
