using System.Windows;
using System.Windows.Input;

namespace MapEditor.Views
{
    /// <summary>
    /// Interaction logic for TextureLoadProgressWindowView.xaml
    /// </summary>
    public partial class TextureLoadProgressWindowView : Window
    {
        public TextureLoadProgressWindowView()
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
