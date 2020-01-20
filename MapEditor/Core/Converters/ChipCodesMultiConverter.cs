using NSChip;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class ChipCodesMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
            if (!(values[0] is ChipFolder.CODE[]) || !(values[1] == null || values[1] is int))
            {
                return Binding.DoNothing;
            }

            if (values[1] == null)
            {
                var combined = string.Join(string.Empty, ((ChipFolder.CODE[])values[0]).Distinct().Select(this.CodeToString));
                return combined;
            }

            var code = this.CodeToString(((ChipFolder.CODE[])values[0])[((int?)values[1]).Value]);
            return code;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
            var doNothingArray = new object[targetTypes.Length];
            for (int i = 0; i < doNothingArray.Length; i++)
            {
                doNothingArray[i] = Binding.DoNothing;
            }
            return doNothingArray;
		}

        private string CodeToString(ChipFolder.CODE code)
        {
            if (code == ChipFolder.CODE.asterisk)
            {
                return "＊";
            }
            else if (code == ChipFolder.CODE.none)
            {
                return string.Empty;
            }
            else
            {
                return code.ToString();
            }
        }
	}
}
