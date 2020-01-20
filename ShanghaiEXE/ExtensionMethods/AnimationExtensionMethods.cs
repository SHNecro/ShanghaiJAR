using NSShanghaiEXE.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSShanghaiEXE.ExtensionMethods
{
    public static class AnimationExtensionMethods
    {
        public static IEnumerable<Tuple<Animation, int>> ToFrameTimings(this IEnumerable<Animation> animations)
        {
            return animations.Select((a, i) => Tuple.Create(a, animations.Take(i + 1).Sum(prev => prev.Delay)));
        }
    }
}
