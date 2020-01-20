using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class NumberToOffsetNumberConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d + (double.TryParse(parameter as string ?? "0", out double offset) ? offset : 0);
            }
            else if (value is int i)
            {
                return i + (int.TryParse(parameter as string ?? "0", out int offset) ? offset : 0);
            }
            else
            {
                return Binding.DoNothing;
            }
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d - (double.TryParse(parameter as string ?? "0", out double offset) ? offset : 0);
            }
            else if (value is int i)
            {
                return i - (int.TryParse(parameter as string ?? "0", out int offset) ? offset : 0);
            }
            else
            {
                return Binding.DoNothing;
            }
        }
	}
}
