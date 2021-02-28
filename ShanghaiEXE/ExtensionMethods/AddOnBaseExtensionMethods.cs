using NSAddOn;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class AddOnBaseExtensionMethods
    {
        private static Dictionary<string, Type> AddOns;

        private static Dictionary<Type, string> Names;

        static AddOnBaseExtensionMethods()
        {
            AddOns = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(t => t.IsClass && t.Namespace == typeof(AddOnBase).Namespace)
                       .ToDictionary(c => c.Name, c => c);

            Names = new Dictionary<Type, string>();
            foreach (var kvp in AddOns)
            {
                Names[kvp.Value] = kvp.Key;
            }
        }

        public static Type ToAddOnType(this string name) => AddOns[name];

        public static string ToAddOnName(this Type type) => $"Addon.{Names[type]}";
    }
}
