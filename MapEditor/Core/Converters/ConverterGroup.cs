using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class ConverterGroup : List<IValueConverter>, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Reverse<IValueConverter>().Aggregate(value, (current, converter) => current == Binding.DoNothing ? Binding.DoNothing : converter.ConvertBack(current, targetType, parameter, culture));
        }

        #endregion
    }
}
