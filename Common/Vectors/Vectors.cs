using System;

namespace Common.Vectors
{
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(Vector2 v1, float z)
        {
            this.X = v1.X;
            this.Y = v1.Y;
            this.Z = z;
        }
    }

    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2 Multiply(Vector2 v1, float scalar)
        {
            return v1 * scalar;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, float scalar)
        {
            return new Vector2(v1.X * scalar, v1.Y * scalar);
        }

        public static Vector2 operator /(Vector2 v1, float scalar)
        {
            return new Vector2(v1.X / scalar, v1.Y / scalar);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return Math.Abs(v1.X - v2.X) < float.Epsilon && Math.Abs(v1.Y - v2.Y) < float.Epsilon;
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

        public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);
    }
}
