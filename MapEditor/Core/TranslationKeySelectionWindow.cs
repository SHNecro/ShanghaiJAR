using MapEditor.ViewModels;
using MapEditor.Views;
using System;

namespace MapEditor.Core
{
    public static class TranslationKeySelectionWindow
    {
        private static TranslationKeySelectionWindowViewModel Instance { get; set; }
        private static TranslationKeySelectionWindowView WindowInstance { get; set; }

        static TranslationKeySelectionWindow()
        {
            TranslationKeySelectionWindow.Instance = new TranslationKeySelectionWindowViewModel
            {
                FilterLocale = "en-US"
            };
            TranslationKeySelectionWindow.WindowInstance = new TranslationKeySelectionWindowView
            {
                Owner = LoadingWindowViewModel.MainWindow
            };
            TranslationKeySelectionWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                TranslationKeySelectionWindow.WindowInstance.Hide();
            };
            TranslationKeySelectionWindow.WindowInstance.DataContext = TranslationKeySelectionWindow.Instance;

            Constants.TranslationService.KeysReloaded += (sender, args) =>
            {
                TranslationKeySelectionWindow.Instance.FilterEntries();
            };
        }

        public static void ShowWindow(string initialKey)
        {
            TranslationKeySelectionWindow.Instance.SelectKeyEntry(initialKey, false);
            TranslationKeySelectionWindow.WindowInstance.Show();
            TranslationKeySelectionWindow.WindowInstance.ScrollToSelectedEntry();
        }

        public static void SetKeySetterAction(Action<string> keySetterAction)
        {
            TranslationKeySelectionWindow.Instance.KeySetterAction = keySetterAction;
        }

        public static void ScrollToSelectedEntry()
        {
            TranslationKeySelectionWindow.WindowInstance?.ScrollToSelectedEntry();
        }
    }
}
