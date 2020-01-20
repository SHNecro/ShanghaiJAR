using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class EncounterToIsSpecialBooleanMultiConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var paramsArray = values;
            if (values == null || paramsArray.Length != 3)
            {
                return Binding.DoNothing;
            }

            object sourceCollection = paramsArray[0];
            object specialEncounterCount = paramsArray[1];
            object checkedObject = paramsArray[2];

            if (sourceCollection == null || specialEncounterCount == null || !(specialEncounterCount is int) || checkedObject == null)
            {
                return Binding.DoNothing;
            }

            try
            {
                var bindFlags = BindingFlags.Public | BindingFlags.Instance;
                var sourceIndex = (int)sourceCollection.GetType().GetMethod("IndexOf", bindFlags).Invoke(sourceCollection, new[] { checkedObject });
                var count = (int)sourceCollection.GetType().GetProperty("Count", bindFlags).GetGetMethod(false).Invoke(sourceCollection, new object[] { });
                return sourceIndex >= count - (int)specialEncounterCount;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
                return Binding.DoNothing;
            }
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
