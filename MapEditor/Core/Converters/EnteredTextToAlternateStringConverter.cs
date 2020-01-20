using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class EnteredTextToAlternateStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string textString))
            {
                return null;
            }

            return textString.Replace(",", Environment.NewLine);
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string textString))
            {
                return null;
            }
            
            var isOpeningQuote = true;
            var replacedString = new StringBuilder();
            foreach (var c in textString)
            {
                switch (c)
                {
                    case '"':
                        replacedString.Append(isOpeningQuote ? '“' : '”');
                        isOpeningQuote = !isOpeningQuote;
                        break;
                    case '“':
                        replacedString.Append(c);
                        isOpeningQuote = false;
                        break;
                    case '”':
                        replacedString.Append(c);
                        isOpeningQuote = true;
                        break;
                    case ',':
                        replacedString.Append('，');
                        break;
                    case '*':
                        replacedString.Append('＊');
                        break;
                    case '\'':
                        replacedString.Append('’');
                        break;
                    default:
                        replacedString.Append(c);
                        break;
                }
            }

            return replacedString.Replace(Environment.NewLine, ",").ToString();
        }
	}
}
