using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class UntranslatedStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string text))
            {
                return null;
            }

            return $"[{text}]";
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
		}
	}
}
