using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class LevelBottomUpMultiConverter : IMultiValueConverter
	{
        private int lastConvertedLevels;

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
            if (!(values[0] is int) || !(values[1] is int))
            {
                return Binding.DoNothing;
            }

            lastConvertedLevels = (int)values[0];
            return (int)values[0] - (int)values[1];
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
            object enteredValue;
            if (int.TryParse(value.ToString(), out int intValue))
            {
                enteredValue = lastConvertedLevels - intValue;
            }
            else
            {
                enteredValue = Binding.DoNothing;
            }
            return new[] { Binding.DoNothing, enteredValue };
		}
	}
}
