using Common.Vectors;

namespace NSGame
{
    internal class Bound
    {
        private readonly int time;
        private Vector2 endposition;
        private Vector2 position;
        private readonly float movex;
        private readonly float movey;
        public float plusy;
        private float speedy;
        private readonly float plusing;
        private const int startspeed = 6;

        public Bound(Vector2 position, Vector2 endposition, int time)
        {
            this.position = position;
            this.endposition = endposition;
            this.time = time;
            this.movex = (position.X - endposition.X) / time;
            this.movey = (position.Y - endposition.Y) / time;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (time / 2);
        }

        public Vector2 Update(Vector2 positionDirect)
        {
            positionDirect.X -= this.movex;
            positionDirect.Y -= this.movey;
            this.plusy += this.speedy;
            this.speedy -= this.plusing;
            return positionDirect;
        }
    }
}
