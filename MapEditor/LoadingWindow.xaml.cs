using MapEditor.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public static LoadingWindow Window;

        public LoadingWindow()
        {
            this.DataContext = new LoadingWindowViewModel();

            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingWindow.Window = this;

            var viewModel = (LoadingWindowViewModel)this.DataContext;
            viewModel.CloseAction += this.Close;
            viewModel.Initialize();
        }
    }
}
