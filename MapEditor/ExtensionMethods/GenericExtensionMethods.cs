using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MapEditor.ExtensionMethods
{
    public static class GenericExtensionMethods
    {
        public static Wrapper<T> Wrap<T>(this T obj) => new Wrapper<T>(obj);

        public static bool Matches<T>(this T obj, Func<T, bool> predicate)
        {
            return predicate.Invoke(obj);
        }

        public static ObservableCollection<T> AsObservableCollectionOrEmpty<T>(this IEnumerable<T> collection)
        {
            var observableCollection = collection as ObservableCollection<T>;
            if (observableCollection != null)
            {
                return observableCollection;
            }

            return collection == null ? new ObservableCollection<T>() : new ObservableCollection<T>(collection);
        }
    }
}
