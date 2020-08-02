using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MapEditor.ExtensionMethods
{
    public static class GenericExtensionMethods
    {
        public static Wrapper<T> Wrap<T>(this T obj) => new Wrapper<T>(obj);

        public static bool Matches<T>(this T obj, Func<T, bool> predicate)
        {
            return predicate.Invoke(obj);
        }
    }
}
