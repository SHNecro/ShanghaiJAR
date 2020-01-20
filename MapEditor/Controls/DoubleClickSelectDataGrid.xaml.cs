using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for DoubleClickSelectDataGrid.xaml
    /// </summary>
    public partial class DoubleClickSelectDataGrid : DataGrid
	{
		public static readonly DependencyProperty ConfirmedItemProperty = DependencyProperty.Register("ConfirmedItem", typeof(object), typeof(DoubleClickSelectDataGrid), new PropertyMetadata(null));

		public DoubleClickSelectDataGrid()
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            e.Handled = false;
        }
    }
}
