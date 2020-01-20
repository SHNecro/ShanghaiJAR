using System.Windows;

namespace MapEditor.Core
{
    public class MinMaxBinding : DependencyObject
    {
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(int?), typeof(MinMaxBinding), new PropertyMetadata(null));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(int?), typeof(MinMaxBinding), new PropertyMetadata(null));

        public int? Minimum
        {
            get { return (int?)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        public int? Maximum
        {
            get { return (int?)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        public bool MinInclusive { get; set; } = true;

        public bool MaxInclusive { get; set; } = true;
    }
}
