using MapEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class MapEventConverter : IValueConverter
	{
        public bool IsMapEntities { get; set; }

		public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
		{
            if (!(values is IEnumerable<MapObject> mapObjects))
            {
                return Binding.DoNothing;
            }

            if (this.IsMapEntities)
            {
                return new ObservableCollection<MapEntity>(mapObjects.OfType<MapEntity>());
            }
            else
            {
                return new ObservableCollection<MapMystery>(mapObjects.OfType<MapMystery>());
            }
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
		{
            return Binding.DoNothing;
		}
	}
}
