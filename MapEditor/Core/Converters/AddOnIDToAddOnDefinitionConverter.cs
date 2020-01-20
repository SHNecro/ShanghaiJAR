using MapEditor.Models.Elements;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
	public class AddOnIDToAddOnDefinitionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int id) || !Constants.AddOnDefinitions.ContainsKey(id))
            {
                return null;
            }

            return Constants.AddOnDefinitions[id];
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is AddOnDefinition addOnDefinition))
            {
                return null;
            }

            return addOnDefinition.ID;
		}
	}
}
