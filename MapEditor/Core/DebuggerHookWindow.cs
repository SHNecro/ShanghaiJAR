using MapEditor.ViewModels;
using MapEditor.Views;
using System;

namespace MapEditor.Core
{
    public static class DebuggerHookWindow
    {
        private static DebuggerHookWindowViewModel Instance { get; set; }
        private static DebuggerHookWindowView WindowInstance { get; set; }

        static DebuggerHookWindow()
        {
            DebuggerHookWindow.Instance = new DebuggerHookWindowViewModel
            {
            };
            DebuggerHookWindow.WindowInstance = new DebuggerHookWindowView
            {
                Owner = LoadingWindowViewModel.MainWindow
            };
            DebuggerHookWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                DebuggerHookWindow.WindowInstance.Hide();
            };
            DebuggerHookWindow.WindowInstance.DataContext = DebuggerHookWindow.Instance;
        }

        public static void ShowWindow()
        {
            DebuggerHookWindow.WindowInstance.Show();
        }
    }
}
