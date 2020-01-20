using MapEditor.Models;
using MapEditor.ViewModels;
using MapEditor.Views;
using System;

namespace MapEditor.Core
{
    public static class SpriteSelectionWindow
    {
        private static SpriteSelectionWindowViewModel Instance { get; set; }
        private static SpriteSelectionWindowView WindowInstance { get; set; }

        static SpriteSelectionWindow()
        {
            SpriteSelectionWindow.Instance = new SpriteSelectionWindowViewModel
            {
            };

            SpriteSelectionWindow.HasBeenShown = false;
        }

        public static bool HasBeenShown { get; set; }

        public static void ShowWindow(MapEventPage initialPage)
        {
            if (SpriteSelectionWindow.WindowInstance == null)
            {
                SpriteSelectionWindow.WindowInstance = new SpriteSelectionWindowView
                {
                    Owner = LoadingWindowViewModel.MainWindow
                };
                SpriteSelectionWindow.WindowInstance.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    SpriteSelectionWindow.WindowInstance.Hide();
                };
                SpriteSelectionWindow.WindowInstance.DataContext = SpriteSelectionWindow.Instance;
            }
            SpriteSelectionWindow.RefreshWindow(initialPage);
            SpriteSelectionWindow.WindowInstance.Show();
            SpriteSelectionWindow.HasBeenShown = true;
        }

        public static void RefreshWindow(MapEventPage initialPage)
        {
            SpriteSelectionWindow.Instance.CurrentPage = initialPage;
        }

        public static void SetPageSetterAction(Action<MapEventPage> pageSetterAction)
        {
            SpriteSelectionWindow.Instance.PageSetterAction = pageSetterAction;
        }

        public static void SetWindowEnable(bool enable)
        {
            SpriteSelectionWindow.WindowInstance.IsEnabled = enable;
        }
    }
}
