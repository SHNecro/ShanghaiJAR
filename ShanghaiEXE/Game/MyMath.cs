using Common.Vectors;
using System;

namespace NSGame
{
    internal class MyMath
    {
        private const float pi = 3.141593f;

        public static float Abs(float a)
        {
            if (a < 0.0)
                a *= -1f;
            return a;
        }

        public static float Pow(float a, int b)
        {
            float num = 1f;
            for (int index = 0; index < b; ++index)
                num *= a;
            return num;
        }

        public static float Rad(float a)
        {
            return (float)(a * 3.14159274101257 / 180.0);
        }

        public static float Degree(float a)
        {
            a = (float)(a * 180.0 / 3.14159274101257);
            if (a < 0.0)
                a += 360f;
            else if (a > 360.0)
                a -= 360f;
            return a;
        }

        public static int Sign(float a)
        {
            if (a < 0.0)
                return -1;
            return a >= 0.0 ? 1 : 0;
        }

        public static Vector2 Polar(float theta, float speed)
        {
            if (theta < 0.0)
            {
                while (theta < 0.0)
                    theta += 360f;
            }
            else if (theta >= 360.0)
            {
                while (theta >= 360.0)
                    theta -= 360f;
            }
            Vector2 vector2 = new Vector2((float)Math.Cos(theta + 90.0) * speed, (float)Math.Sin(theta + 90.0) * speed);
            vector2.X *= -1f;
            vector2.Y *= -1f;
            return vector2;
        }

        public static Vector2 CreateVector(Vector2 p, Vector2 q)
        {
            return new Vector2(q.X - p.X, q.Y - p.Y);
        }

        public static float InnerProduct(Vector2 a, Vector2 b)
        {
            return (float)(a.X * (double)b.X + a.Y * (double)b.Y);
        }

        public static float OuterProduct(Vector2 a, Vector2 b)
        {
            return (float)(a.X * (double)b.Y - a.Y * (double)b.X);
        }

        public static float VectorLength2(Vector2 v)
        {
            return MyMath.InnerProduct(v, v);
        }

        public static bool SquareHit(Square square, Circle circle)
        {
            int[,] numArray = new int[2, 4]
            {
        {
          0,
          1,
          3,
          2
        },
        {
          1,
          3,
          2,
          0
        }
            };
            for (int index = 0; index < 4; ++index)
            {
                Vector2 vector1 = MyMath.CreateVector(square.p[numArray[0, index]], square.p[numArray[1, index]]);
                Vector2 vector2 = MyMath.CreateVector(square.p[numArray[0, index]], circle.point);
                float num1 = MyMath.InnerProduct(vector1, vector2);
                float num2 = MyMath.VectorLength2(vector1);
                float num3 = MyMath.VectorLength2(vector2);
                float num4 = num1 / num2;
                if (num4 >= 0.0 && 1.0 >= num4)
                {
                    float num5 = num1 * num1 / num2;
                    if (num3 - num5 < (double)(circle.range * circle.range))
                        return true;
                }
            }
            return false;
        }
    }
}
