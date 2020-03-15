using Common.ExtensionMethods;
using System;
using System.Globalization;
using System.Windows.Data;

namespace KeyConfig.Converters
{
    public class SHKeyCodeToKeyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var keyCode = System.Convert.ToInt32(value);
			return keyCode.FromSHKeyCode() ?? value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
