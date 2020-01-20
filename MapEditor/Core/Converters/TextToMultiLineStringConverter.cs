using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class TextToMultiLineStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string textString))
            {
                return null;
            }

            return textString.Replace(",", Environment.NewLine).Replace("，", ", ");
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string textString))
            {
                return null;
            }

            return textString.Replace(", ", "，").Replace(Environment.NewLine, ",");
        }
	}
}
