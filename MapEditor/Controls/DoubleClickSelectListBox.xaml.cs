using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for DoubleClickSelectListBox.xaml
    /// </summary>
    public partial class DoubleClickSelectListBox : ListBox
    {
        public static readonly DependencyProperty ConfirmedItemProperty = DependencyProperty.Register("ConfirmedItem", typeof(object), typeof(DoubleClickSelectListBox), new PropertyMetadata(null));

        public DoubleClickSelectListBox()
        {
            InitializeComponent();
        }

        public object ConfirmedItem
        {
            get { return this.GetValue(ConfirmedItemProperty); }
            set { this.SetValue(ConfirmedItemProperty, value); }
        }

        private void OnRowDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            this.ConfirmedItem = this.SelectedItem;
        }
    }
}
