using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                return initialList;
            }
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
