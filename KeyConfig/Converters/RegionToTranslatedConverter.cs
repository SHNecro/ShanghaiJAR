using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Xml;

namespace KeyConfig.Converters
{
    public class RegionToTranslatedConverter : IValueConverter
    {
        private static readonly Dictionary<string, Dictionary<string, string>> Language;

        static RegionToTranslatedConverter()
        {
            RegionToTranslatedConverter.Language = new Dictionary<string, Dictionary<string, string>>();

            foreach (var locale in RegionToTranslatedConverter.Locales)
            {
                RegionToTranslatedConverter.LoadLabels(locale.Item2);
            }
        }

        public static IEnumerable<Tuple<string, string>> Locales => Directory.GetDirectories("language")
            .Select(p => Path.GetFileName(p)).Where(l => l != "data").Select(localeName =>
        {
            Tuple<string, string> locale;
            try
            {
                var culture = new CultureInfo(localeName);
                locale = Tuple.Create(culture.Parent.NativeName, culture.Name);
            }
            catch (CultureNotFoundException)
            {
                locale = Tuple.Create(localeName, localeName);
            }

            return locale;
        });

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var region = (string)value;
            var key = (string)parameter;

            if (region == null || key == null || !Language.ContainsKey(region) || !Language[region].ContainsKey(key))
            {
                return string.Empty;
            }

            return Language[region][key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        private static void LoadLabels(string region)
        {
            Language[region] = new Dictionary<string, string>();
            var languageDoc = new XmlDocument();
            languageDoc.Load($"language/{region}/Config.xml");

            var text = languageDoc.SelectNodes("data/Text");
            foreach (XmlNode node in text)
            {
                var key = node.Attributes["Key"].Value;
                var value = node.Attributes["Value"].Value;
                Language[region].Add(key, value);
            }
        }
    }
}
