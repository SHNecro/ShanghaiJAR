using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class KeyToTranslatedTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as string;
            if (key == null || !Constants.TranslationService.CanTranslate(key))
            {
                return $"Invalid Key: {key}";
            }
            else
            {
                return Constants.TranslationService.Translate(key).Text;
            }
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
