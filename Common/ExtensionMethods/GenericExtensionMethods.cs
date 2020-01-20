using System;

namespace Common.ExtensionMethods
{
    public static class GenericExtensionMethods
    {
        public static bool Matches<T>(this T obj, Func<T, bool> predicate)
        {
            return predicate.Invoke(obj);
        }
    }
}
