using MapEditor.ViewModels;
using MapEditor.Views;

namespace MapEditor.Core
{
    public static class DataDumpWindow
    {
        private static DataDumpWindowViewModel Instance { get; set; }
        private static DataDumpWindowView WindowInstance { get; set; }

        static DataDumpWindow()
        {
            DataDumpWindow.Instance = new DataDumpWindowViewModel
            {
            };
            DataDumpWindow.WindowInstance = new DataDumpWindowView
            {
                Owner = LoadingWindowViewModel.MainWindow
            };
            DataDumpWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                DataDumpWindow.WindowInstance.Hide();
            };
            DataDumpWindow.WindowInstance.DataContext = DataDumpWindow.Instance;
        }

        public static void ShowWindow()
        {
            DataDumpWindow.WindowInstance.Show();
        }
    }
}
