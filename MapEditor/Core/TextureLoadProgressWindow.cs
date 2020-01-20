using MapEditor.ViewModels;
using MapEditor.Views;
using System.Windows;

namespace MapEditor.Core
{
    public static class TextureLoadProgressWindow
    {
        private static TextureLoadProgressWindowViewModel Instance { get; set; }
        private static TextureLoadProgressWindowView WindowInstance { get; set; }

        static TextureLoadProgressWindow()
        {
            TextureLoadProgressWindow.Instance = new TextureLoadProgressWindowViewModel();
            TextureLoadProgressWindow.WindowInstance = new TextureLoadProgressWindowView();
            TextureLoadProgressWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                TextureLoadProgressWindow.WindowInstance.Hide();
            };
            TextureLoadProgressWindow.WindowInstance.DataContext = TextureLoadProgressWindow.Instance;
        }

        public static void ShowWindow()
        {
            if (LoadingWindowViewModel.MainWindow.IsLoaded)
            {
                TextureLoadProgressWindow.WindowInstance.Owner = LoadingWindowViewModel.MainWindow;
            }
            else
            {
                var showOnLoad = default(RoutedEventHandler);
                showOnLoad = new RoutedEventHandler((sender, args) =>
                {
                    TextureLoadProgressWindow.WindowInstance.Owner = LoadingWindowViewModel.MainWindow;
                    LoadingWindowViewModel.MainWindow.Loaded -= showOnLoad;
                });
                LoadingWindowViewModel.MainWindow.Loaded += showOnLoad;
            }
            TextureLoadProgressWindow.WindowInstance.ShowDialog();
        }

        public static void SetProgress(double progress, string label)
        {
            TextureLoadProgressWindow.Instance.Progress = progress;
            TextureLoadProgressWindow.Instance.ProgressLabel = label;
        }

        public static void HideWindow()
        {
            TextureLoadProgressWindow.WindowInstance.Hide();
        }
    }
}
