using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace KeyConfigLinux.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue == null ? BindingOperations.DoNothing : !boolValue.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = value as bool?;
            return boolValue == null ? BindingOperations.DoNothing : !boolValue.Value;
        }
    }
}
