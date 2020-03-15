using Common.ExtensionMethods;
using System;
using System.Globalization;
using System.Windows.Data;

namespace KeyConfig.Converters
{
    public class SHButtonCodeToXInputButtonConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var buttonCode = System.Convert.ToInt32(value);
			return buttonCode.FromSHButtonCode() ?? value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
