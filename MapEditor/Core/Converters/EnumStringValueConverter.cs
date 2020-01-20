using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class EnumStringValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
            if (parameter is Type)
            {
                return Enum.Parse((Type)parameter, value.ToString());
            }
            else
            {
                return Enum.Parse(parameter.GetType(), value.ToString());
            }
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString();
		}
	}
}
