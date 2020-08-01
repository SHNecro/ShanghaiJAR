using MapEditor.ViewModels;
using MapEditor.Views;

namespace MapEditor.Core
{
    public static class ErrorsWindow
    {
        private static ErrorsWindowViewModel Instance { get; set; }
        private static ErrorsWindowView WindowInstance { get; set; }

        static ErrorsWindow()
        {
            ErrorsWindow.Instance = new ErrorsWindowViewModel
            {
            };
            ErrorsWindow.WindowInstance = new ErrorsWindowView
            {
                Owner = LoadingWindowViewModel.MainWindow
            };
            ErrorsWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                ErrorsWindow.WindowInstance.Hide();
            };
            ErrorsWindow.WindowInstance.DataContext = ErrorsWindow.Instance;
        }

        public static void ShowWindow()
        {
            ErrorsWindow.Instance.Refresh();
            ErrorsWindow.WindowInstance.Show();
        }
    }
}
