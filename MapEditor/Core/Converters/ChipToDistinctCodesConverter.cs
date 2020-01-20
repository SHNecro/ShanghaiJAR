using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class ChipToDistinctCodesConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Chip chip) || !Constants.ChipDefinitions.ContainsKey(chip.ID))
            {
                return null;
            }

            var chipDefinition = Constants.ChipDefinitions[chip.ID];
            return chipDefinition.Codes.Select((c, codeNumber) => Tuple.Create(codeNumber, c == NSChip.ChipFolder.CODE.asterisk ? "＊" : c.ToString())).GroupBy(t => t.Item2).Select(g => g.First());
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
		}
	}
}
