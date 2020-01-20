using MapEditor.ViewModels;
using MapEditor.Views;
using System;

namespace MapEditor.Core
{
    public static class MapSelectionWindow
    {
        private static MapSelectionWindowViewModel Instance { get; set; }
        private static MapSelectionWindowView WindowInstance { get; set; }

        static MapSelectionWindow()
        {
            MapSelectionWindow.Instance = new MapSelectionWindowViewModel
            {
            };
        }

        public static void ShowWindow(string initialMapName)
        {

            if (MapSelectionWindow.WindowInstance == null)
            {
                MapSelectionWindow.WindowInstance = new MapSelectionWindowView
                {
                    Owner = LoadingWindowViewModel.MainWindow
                };
                MapSelectionWindow.WindowInstance.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    MapSelectionWindow.WindowInstance.Hide();
                };
                MapSelectionWindow.WindowInstance.DataContext = MapSelectionWindow.Instance;
            }

            MapSelectionWindow.Instance.SelectMapName(initialMapName);
            MapSelectionWindow.WindowInstance.Show();
            MapSelectionWindow.WindowInstance.ScrollToSelectedEntry();
        }

        public static void SetMapSetterAction(Action<string> mapSetterAction)
        {
            MapSelectionWindow.Instance.MapNameSetterAction = mapSetterAction;
        }
    }
}
