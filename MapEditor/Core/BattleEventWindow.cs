using MapEditor.Models.Elements.Events;
using MapEditor.ViewModels;
using MapEditor.Views;
using System;

namespace MapEditor.Core
{
    public static class BattleEventWindow
    {
        private static BattleEventWindowViewModel Instance { get; set; }
        private static BattleEventWindowView WindowInstance { get; set; }

        static BattleEventWindow()
        {
            BattleEventWindow.Instance = new BattleEventWindowViewModel
            {
            };

            BattleEventWindow.HasBeenShown = false;
        }

        public static bool HasBeenShown { get; set; }

        public static void ShowWindow(BattleEvent initialBattleEvent)
        {
            if (BattleEventWindow.WindowInstance == null)
            {
                BattleEventWindow.WindowInstance = new BattleEventWindowView
                {
                    Owner = LoadingWindowViewModel.MainWindow
                };
                BattleEventWindow.WindowInstance.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    BattleEventWindow.WindowInstance.Hide();
                };
                BattleEventWindow.WindowInstance.DataContext = BattleEventWindow.Instance;
            }
            BattleEventWindow.RefreshWindow(initialBattleEvent);
            BattleEventWindow.WindowInstance.Show();
            BattleEventWindow.HasBeenShown = true;
        }

        public static void RefreshWindow(BattleEvent initialBattleEvent)
        {
            BattleEventWindow.Instance.CurrentBattleEvent = initialBattleEvent;
        }

        public static void SetBattleEventSetterAction(Action<BattleEvent> pageSetterAction)
        {
            BattleEventWindow.Instance.BattleEventSetterAction = pageSetterAction;
        }

        public static void SetWindowEnable(bool enable)
        {
            BattleEventWindow.WindowInstance.IsEnabled = enable;
        }
    }
}
