using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSShanghaiEXE.Common
{
    public class Constants
    {
        public static readonly Size ScreenSize = new Size(240, 160);

        private static readonly HashSet<Tuple<int, int>> FloatingCharacters = new HashSet<Tuple<int, int>>
        {
            new Tuple<int, int>(2, 6),
            new Tuple<int, int>(7, 0),
            new Tuple<int, int>(7, 1),
            new Tuple<int, int>(7, 2),
            new Tuple<int, int>(7, 3),
            new Tuple<int, int>(7, 4),
            new Tuple<int, int>(7, 7),
            new Tuple<int, int>(8, 0),
            new Tuple<int, int>(8, 6),
            new Tuple<int, int>(10, 0),
            new Tuple<int, int>(11, 5),
            new Tuple<int, int>(13, 0),
            new Tuple<int, int>(13, 1),
            new Tuple<int, int>(13, 2),
            new Tuple<int, int>(13, 5),
            new Tuple<int, int>(13, 7),
            new Tuple<int, int>(17, 4),
            new Tuple<int, int>(19, 4)
        };

        private static readonly HashSet<Tuple<int, int>> NoShadowCharacters = new HashSet<Tuple<int, int>>
        {
            new Tuple<int, int>(8, 6),
            new Tuple<int, int>(10, 1),
            new Tuple<int, int>(10, 2),
            new Tuple<int, int>(10, 3),
            new Tuple<int, int>(10, 4),
            new Tuple<int, int>(10, 5),
            new Tuple<int, int>(10, 6),
            new Tuple<int, int>(10, 7),
            new Tuple<int, int>(13, 1),
            new Tuple<int, int>(13, 2),
            new Tuple<int, int>(13, 4),
            new Tuple<int, int>(19, 1),
            new Tuple<int, int>(19, 2),
            new Tuple<int, int>(19, 3)
        };

        public static readonly List<byte[,]> PanelLayouts = new List<byte[,]>
        {
            new byte[3, 6] {
                {0,0,0,0,0,0},
                {0,0,0,0,0,0},
                {0,0,0,0,0,0}
            },
            new byte[3, 6] {
                {1,0,0,0,0,1},
                {1,0,0,0,0,1},
                {1,0,0,0,0,1}
            },
            new byte[3, 6] {
                {1,0,0,0,0,0},
                {0,0,0,0,0,0},
                {0,0,0,0,0,1}
            },
            new byte[3, 6] {
                {1,0,1,1,0,1},
                {0,0,0,0,0,0},
                {1,0,1,1,0,1}
            },
            new byte[3, 6] {
                {1,0,1,1,0,1},
                {1,0,1,1,0,1},
                {1,0,1,1,0,1}
            },
            new byte[3, 6] {
                {0,0,0,0,0,0},
                {0,1,0,0,1,0},
                {0,0,0,0,0,0}
            },
            new byte[3, 6] {
                {0,1,0,1,0,0},
                {0,0,1,0,1,0},
                {1,0,0,1,0,1}
            },
            new byte[3, 6] {
                {0,0,0,0,0,0},
                {1,1,1,1,1,1},
                {0,0,0,0,0,0}
            },
            new byte[3, 6] {
                {0,0,1,1,0,0},
                {0,0,0,0,0,0},
                {0,0,1,1,0,0}
            },
            new byte[3, 6] {
                {1,0,1,1,0,1},
                {1,1,0,0,1,1},
                {1,0,1,1,0,1}
            },
            new byte[3, 6] {
                {2,2,1,2,2,1},
                {1,0,1,1,0,1},
                {1,2,2,1,2,2}
            },
            new byte[3, 6] {
                {1,0,0,0,0,1},
                {1,1,0,0,1,1},
                {0,1,1,1,1,0}
            },
            new byte[3, 6] {
                {0,1,0,0,1,0},
                {1,0,1,1,0,1},
                {0,1,0,0,1,0}
            },
            new byte[3, 6] {
                {0,1,0,1,0,1},
                {0,0,0,0,0,0},
                {1,0,1,0,1,0}
            },
            new byte[3, 6] {
                {1,0,0,0,1,0},
                {0,0,1,0,0,1},
                {0,1,1,0,1,0}
            },
            new byte[3, 6] {
                {1,0,0,0,0,1},
                {1,3,0,0,3,1},
                {1,0,0,0,0,1}
            },
            new byte[3, 6] {
                {0,1,3,0,1,0},
                {0,1,1,1,1,0},
                {0,1,0,3,1,0}
            },
            new byte[3, 6] {
                {0,0,1,1,0,0},
                {3,0,1,1,0,3},
                {0,0,1,1,0,0}
            },
            new byte[3, 6] {
                {0,1,3,3,1,0},
                {1,1,1,1,1,1},
                {3,1,0,0,1,3}
            },
            new byte[3, 6] {
                {1,0,0,0,3,1},
                {0,1,0,0,1,0},
                {0,3,1,1,0,0}
            },
            new byte[3, 6] {
                {1,0,0,0,0,1},
                {1,0,3,3,0,1},
                {1,0,0,0,0,1}
            },
            new byte[3, 6] {
                {0,0,0,0,0,0},
                {0,0,0,0,0,0},
                {0,0,0,0,0,0}
            }
        };

        public static bool IsFloatingCharacter(int sheet, int index) => FloatingCharacters.Contains(new Tuple<int, int>(sheet, index));

        public static bool IsNoShadowCharacter(int sheet, int index) => NoShadowCharacters.Contains(new Tuple<int, int>(sheet, index));
    }
}
