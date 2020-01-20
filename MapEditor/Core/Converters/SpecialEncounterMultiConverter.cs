using MapEditor.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class SpecialEncounterMultiConverter : IMultiValueConverter
	{
        public bool IsSpecialEncounters { get; set; }

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
            if (!(values[0] is IEnumerable<RandomEncounter> encounters) || values[1] == null || !(values[1] is int))
            {
                return Binding.DoNothing;
            }

            var specialEncountersCount = (int)values[1];
            var normalEncountersCount = encounters.Count() - specialEncountersCount;

            var selectedEncounters = this.IsSpecialEncounters ? encounters.Skip(normalEncountersCount) : encounters.Take(normalEncountersCount);
            return selectedEncounters;
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
	}
}
