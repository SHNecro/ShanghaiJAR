using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace KeyConfigLinux.Converters
{
    public class EqualsValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == (string)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool) || !((bool)value))
            {
                return BindingOperations.DoNothing;
            }
            
            var parameterConverted = false;
            object convertedParameter = parameter;
            if (!parameterConverted && bool.TryParse(parameter.ToString(), out var convertedParameterBool))
            {
                convertedParameter = convertedParameterBool;
            }
            
            if (!parameterConverted && int.TryParse(parameter.ToString(), out var convertedParameterInt))
            {
                convertedParameter = convertedParameterInt;
            }
            
            if (!parameterConverted && double.TryParse(parameter.ToString(), out var convertedParameterDouble))
            {
                convertedParameter = convertedParameterDouble;
            }
            
            return convertedParameter;
        }
    }
}
