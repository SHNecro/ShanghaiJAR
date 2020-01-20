using MapEditor.ViewModels;
using MapEditor.Views;
using System.Windows;

namespace MapEditor.Core
{
    public static class SettingsWindow
    {
        private static SettingsWindowViewModel Instance { get; set; }
        private static SettingsWindowView WindowInstance { get; set; }

        static SettingsWindow()
        {
            SettingsWindow.Instance = new SettingsWindowViewModel();
            SettingsWindow.WindowInstance = new SettingsWindowView();
            SettingsWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                SettingsWindow.WindowInstance.Hide();
            };
            SettingsWindow.WindowInstance.DataContext = SettingsWindow.Instance;
        }

        public static void ShowWindow()
        {
            if (LoadingWindowViewModel.MainWindow?.IsLoaded ?? false)
            {
                SettingsWindow.WindowInstance.Owner = LoadingWindowViewModel.MainWindow;
            }
            else
            {
                if (LoadingWindow.Window.IsLoaded)
                {
                    SettingsWindow.WindowInstance.Owner = LoadingWindow.Window;
                }
                else
                {
                    var showOnLoad = default(RoutedEventHandler);
                    showOnLoad = new RoutedEventHandler((sender, args) =>
                    {
                        SettingsWindow.WindowInstance.Owner = LoadingWindowViewModel.MainWindow;
                        LoadingWindowViewModel.MainWindow.Loaded -= showOnLoad;
                    });
                    LoadingWindowViewModel.MainWindow.Loaded += showOnLoad;
                }

            }
            SettingsWindow.Instance.RefreshSettings();
            SettingsWindow.WindowInstance.ShowDialog();
        }

        public static void HideWindow()
        {
            SettingsWindow.WindowInstance.Hide();
        }
    }
}
