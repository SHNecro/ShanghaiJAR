using Common.ExtensionMethods;
using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace KeyConfigLinux.Converters
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
			return BindingOperations.DoNothing;
		}
	}
}
