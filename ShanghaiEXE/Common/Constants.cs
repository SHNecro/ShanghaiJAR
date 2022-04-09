﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace NSShanghaiEXE.Common
{
    public class Constants
    {
        public const int ArbitraryLargeValue = 99999;

        public static readonly Size ScreenSize = new Size(240, 160);

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
    }
}
