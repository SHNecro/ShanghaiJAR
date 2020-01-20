using MapEditor.Models.Elements;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class ChipToChipDefinitionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Chip chip) || !Constants.ChipDefinitions.ContainsKey(chip.ID))
            {
                return null;
            }

            return Constants.ChipDefinitions[chip.ID];
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ChipDefinition chipDefinition))
            {
                return null;
            }

            return new Chip { ID = chipDefinition.ID };
		}
	}
}
