using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace KeyConfigLinux.Converters
{
    public class BindingToConverterParameterMultiConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count != 3 || !(values[0] is IValueConverter))
            {
                throw new InvalidOperationException();
            }

            var baseConverter = values[0] as IValueConverter;
            var value = values[1];
            var converterParameter = values[2];

            Console.WriteLine(baseConverter + " " + value + " " + converterParameter);
            return baseConverter.Convert(value, targetType, converterParameter, culture);
        }
    }
}