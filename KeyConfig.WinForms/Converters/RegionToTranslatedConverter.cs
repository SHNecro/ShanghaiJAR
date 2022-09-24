using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace KeyConfigLinux.Converters
{
    public class RegionToTranslatedConverter
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

        public string Convert(string region, string key)
            => this.Convert(region, typeof(string), key, CultureInfo.CurrentCulture) as string;

        private static void LoadLabels(string region)
        {
            Language[region] = new Dictionary<string, string>();
            var languageDoc = new XmlDocument();
            var xmlFileContents = File.ReadAllText($"language/{region}/Config.xml");

            var bom = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (xmlFileContents.StartsWith(bom, StringComparison.Ordinal))
            {
                xmlFileContents = xmlFileContents.Remove(0, bom.Length);
            }

            languageDoc.Load(new StringReader(xmlFileContents));

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
