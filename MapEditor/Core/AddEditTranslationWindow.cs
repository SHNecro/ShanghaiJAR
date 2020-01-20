using MapEditor.ViewModels;
using MapEditor.Views;

namespace MapEditor.Core
{
    public static class AddEditTranslationWindow
    {
        private static AddEditTranslationWindowViewModel Instance { get; set; }
        private static AddEditTranslationWindowView WindowInstance { get; set; }

        static AddEditTranslationWindow()
        {
            AddEditTranslationWindow.Instance = new AddEditTranslationWindowViewModel
            {
            };
            AddEditTranslationWindow.WindowInstance = new AddEditTranslationWindowView
            {
                Owner = LoadingWindowViewModel.MainWindow
            };
            AddEditTranslationWindow.WindowInstance.Closing += (s, e) =>
            {
                e.Cancel = true;
                AddEditTranslationWindow.WindowInstance.Hide();
            };
            AddEditTranslationWindow.WindowInstance.DataContext = AddEditTranslationWindow.Instance;
        }

        public static void ShowWindow(string initialKey, string initialLocale, string initialFile)
        {
            AddEditTranslationWindow.Instance.Initialize(initialKey, initialLocale, initialFile);
            AddEditTranslationWindow.WindowInstance.Show();
        }
    }
}
