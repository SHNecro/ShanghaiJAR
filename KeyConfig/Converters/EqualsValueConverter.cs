using System;
using System.Globalization;
using System.Windows.Data;

namespace KeyConfig.Converters
{
    public class EqualsValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == (string)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;
            return isChecked ? parameter : Binding.DoNothing;
        }
    }
}
