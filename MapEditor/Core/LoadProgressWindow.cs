using MapEditor.ViewModels;
using MapEditor.Views;
using System.Windows;

namespace MapEditor.Core
{
    public static class LoadProgressWindow
    {
        private static LoadProgressWindowViewModel Instance { get; set; }
        private static LoadProgressWindowView WindowInstance { get; set; }

        static LoadProgressWindow()
        {
            LoadProgressWindow.Instance = new LoadProgressWindowViewModel();
            LoadProgressWindow.WindowInstance = new LoadProgressWindowView();
            LoadProgressWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                LoadProgressWindow.WindowInstance.Hide();
            };
            LoadProgressWindow.WindowInstance.DataContext = LoadProgressWindow.Instance;
        }

        public static void ShowWindow()
        {
            if (LoadingWindowViewModel.MainWindow.IsLoaded)
            {
                LoadProgressWindow.WindowInstance.Owner = LoadingWindowViewModel.MainWindow;
            }
            else
            {
                var showOnLoad = default(RoutedEventHandler);
                showOnLoad = new RoutedEventHandler((sender, args) =>
                {
                    LoadProgressWindow.WindowInstance.Owner = LoadingWindowViewModel.MainWindow;
                    LoadingWindowViewModel.MainWindow.Loaded -= showOnLoad;
                });
                LoadingWindowViewModel.MainWindow.Loaded += showOnLoad;
            }
            LoadProgressWindow.WindowInstance.ShowDialog();
        }

        public static void SetProgress(double progress, string label)
        {
            LoadProgressWindow.Instance.Progress = progress;
            LoadProgressWindow.Instance.ProgressLabel = label;
        }

        public static void HideWindow()
        {
            LoadProgressWindow.WindowInstance.Hide();
        }
    }
}
