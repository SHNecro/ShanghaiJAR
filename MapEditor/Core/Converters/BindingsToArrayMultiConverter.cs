using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class BindingsToArrayMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
            return values.Clone();
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
            return value as object[];
		}
	}
}
