using MapEditor.ViewModels;
using MapEditor.Views;
using System.Windows.Input;

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
                DataWindow.Instance.Remove();
                DataWindow.WindowInstance.Hide();
            };
            DataWindow.WindowInstance.DataContext = DataWindow.Instance;
        }

        public static void ShowWindow()
        {
            DataWindow.WindowInstance.Show();
        }

        private static void ShowWindow(int tabIndex)
        {
            ShowWindow();
            DataWindow.Instance.SelectedTabIndex = tabIndex;
        }

        public static ICommand OpenMessagesTabCommand => new RelayCommand(() => DataWindow.ShowWindow(0));
        public static ICommand OpenKeyItemTabCommand => new RelayCommand(() => DataWindow.ShowWindow(1));
        public static ICommand OpenMailTabCommand => new RelayCommand(() => DataWindow.ShowWindow(2));
        public static ICommand OpenCharacterInfoTabCommand => new RelayCommand(() => DataWindow.ShowWindow(3));
        public static ICommand OpenBGMTabCommand => new RelayCommand(() => DataWindow.ShowWindow(4));
    }
}
