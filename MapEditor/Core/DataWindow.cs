using MapEditor.ViewModels;
using MapEditor.Views;

namespace MapEditor.Core
{
    public static class DataWindow
    {
        private static DataWindowViewModel Instance { get; set; }
        private static DataWindowView WindowInstance { get; set; }

        static DataWindow()
        {
            DataWindow.Instance = new DataWindowViewModel
            {
            };
            DataWindow.WindowInstance = new DataWindowView
            {
                Owner = LoadingWindowViewModel.MainWindow
            };
            DataWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                DataWindow.WindowInstance.Hide();
            };
            DataWindow.WindowInstance.DataContext = DataWindow.Instance;
        }

        public static void ShowWindow()
        {
            DataWindow.WindowInstance.Show();
        }
    }
}
