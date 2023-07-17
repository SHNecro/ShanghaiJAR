using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class LocaleToFilePathsMultiConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var locale = value as string;
            if (locale == null || !Constants.TranslationService.Locales.Contains(locale))
            {
                return $"Invalid locale: {locale}";
            }
            else
            {
                var initialList = new List<FileNameFilter> { FileNameFilter.AllFilesFilter };
                initialList.AddRange(Constants.TranslationService.GetFilePaths(locale).Select(fp => new FileNameFilter { IsAllFiles = false, Filter = fp }));
				var assemblyLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;

                foreach (var filter in initialList)
                {
                    filter.Filter = filter.Filter.Replace(assemblyLoc, "");
                }

				return initialList;
            }
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
