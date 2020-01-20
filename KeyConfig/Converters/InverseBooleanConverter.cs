using System;
using System.Globalization;
using System.Windows.Data;

namespace KeyConfig.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue == null ? Binding.DoNothing : !boolValue.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue == null ? Binding.DoNothing : !boolValue.Value;
        }
    }
}
