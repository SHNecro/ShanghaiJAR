using System.Drawing;

namespace NSBattle.Character
{
    internal class CharacterAnimation
    {
        public static Point Return(int[] interval, int[] xpoint, int y, int waittime)
        {
            int index1 = 0;
            for (int index2 = 1; index2 < interval.Length; ++index2)
            {
                if (waittime <= interval[index2] && waittime > interval[index2 - 1])
                    index1 = index2;
            }
            if (index1 != xpoint.Length)
                return new Point(xpoint[index1], y);
            return new Point(0, 0);
        }

        public static Point ReturnKai(int[] interval, int[] xpoint, int y, int waittime)
        {
            int index1 = 0;
            int num = 0;
            for (int index2 = 0; index2 < interval.Length; ++index2)
            {
                interval[index2] += num;
                num = interval[index2];
            }
            for (int index2 = 1; index2 < interval.Length; ++index2)
            {
                if (waittime <= interval[index2] && waittime > interval[index2 - 1])
                    index1 = index2;
            }
            if (index1 != xpoint.Length)
                return new Point(xpoint[index1], y);
            return new Point(0, 0);
        }

        public static Point MoveAnimation(int waittime)
        {
            return CharacterAnimation.Return(new int[7]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6
            }, new int[7] { 0, 1, 2, 3, 2, 1, 0 }, 0, waittime);
        }

        public static Point MoveAnimationS(int waittime)
        {
            return CharacterAnimation.Return(new int[7]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        6
            }, new int[7] { 1, 2, 3, 2, 1, 0, 0 }, 0, waittime);
        }

        public static Point BusterAnimation(int waittime)
        {
            return CharacterAnimation.Return(new int[5]
            {
        0,
        2,
        6,
        8,
        30
            }, new int[5] { 5, 5, 6, 5, 5 }, 0, waittime);
        }

        public static Point DamageAnimation(int waittime)
        {
            return CharacterAnimation.Return(new int[4]
            {
        0,
        2,
        6,
        8
            }, new int[4] { 0, 5, 6, 5 }, 0, waittime);
        }

        public static Point SworsAnimation(int waittime)
        {
            return CharacterAnimation.Return(new int[9]
            {
        8,
        10,
        12,
        14,
        16,
        21,
        25,
        28,
        30
            }, new int[9] { 0, 1, 2, 3, 4, 5, 6, 4, 0 }, waittime <= 25 ? 1 : 0, waittime);
        }

        public static Point BombAnimation(int waittime)
        {
            return CharacterAnimation.Return(new int[6]
            {
        3,
        6,
        9,
        12,
        15,
        25
            }, new int[6] { 0, 1, 2, 3, 4, 0 }, waittime <= 12 ? 2 : 0, waittime);
        }
    }
}
