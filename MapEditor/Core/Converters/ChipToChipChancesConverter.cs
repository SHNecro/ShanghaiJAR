using MapEditor.Models.Elements;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class ChipToChipChancesConverter : IValueConverter
	{
        private static ChipToChipDefinitionConverter chipToChipDefinitionConverter = new ChipToChipDefinitionConverter();
        private static ChipCodesMultiConverter chipCodesMultiConverter = new ChipCodesMultiConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var chip = value as Chip;
            if (chip == null)
            {
                return Binding.DoNothing;
            }

            if (!chip.IsRandom)
            {
                return null;
            }

            var chips = new[] { chip }.Concat(chip.RandomAlternatives);
            var chances = chips.Select(c =>
            {
                var definition = chipToChipDefinitionConverter.Convert(c, targetType, parameter, culture) as ChipDefinition;
                if (definition == null)
                {
                    return null;
                }

                var code = chipCodesMultiConverter.Convert(new object[] { definition.Codes, c.CodeNumber }, targetType, parameter, culture);
                if (code == Binding.DoNothing)
                {
                    return null;
                }

                return Tuple.Create($"{definition.Name} {code}", c.RandomChance);
            }).Where(tup => tup != null).OrderByDescending(tup => tup.Item2);

            return string.Join("\n", chances.Select(tup => $"{tup.Item1} ({tup.Item2 * 100:N0}%)"));
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
		}
	}
}
