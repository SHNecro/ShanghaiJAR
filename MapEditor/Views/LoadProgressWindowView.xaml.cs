using System.Windows;
using System.Windows.Input;

namespace MapEditor.Views
{
    /// <summary>
    /// Interaction logic for LoadProgressWindowView.xaml
    /// </summary>
    public partial class LoadProgressWindowView : Window
    {
        public LoadProgressWindowView()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }
    }
}
