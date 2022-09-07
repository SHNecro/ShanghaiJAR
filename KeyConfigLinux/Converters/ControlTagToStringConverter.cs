using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace KeyConfigLinux.Converters
{
    public class ControlTagToStringConverter : IValueConverter
    {
        // private List<Control> convertedItems = new List<Control>();
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var control = value as Control;

            // if (control != null)
            // {
            //     convertedItems.Add(control);
            // }
            return control?.Tag as string ?? BindingOperations.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Console.WriteLine(value);
            // return convertedItems.FirstOrDefault(c => c.Tag == value as string) ?? BindingOperations.DoNothing;
            return BindingOperations.DoNothing;
        }
    }
}
