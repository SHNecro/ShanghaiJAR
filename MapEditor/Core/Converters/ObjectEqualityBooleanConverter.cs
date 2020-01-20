using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class ObjectEqualityBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = parameter?.Equals(value);
            return a ?? Binding.DoNothing;
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool && (bool)value) ? parameter : Binding.DoNothing;
		}
	}
}
