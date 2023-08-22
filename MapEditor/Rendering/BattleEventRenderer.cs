using MapEditor.Controls;
using MapEditor.Core;
using Common.OpenGL;
using MapEditor.Models;
using MapEditor.Models.Elements.Events;
using System;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using MapEditor.ViewModels;

namespace MapEditor.Rendering
{
    public static partial class BattleEventRenderer
    {
        private const int RenderPassPadding = 5;

        static BattleEventRenderer()
        {
            var fieldSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            BattleEventRenderer.FieldRenderer = fieldSpriteRendererPanel;
            BattleEventRenderer.FieldControl = fieldSpriteRendererPanel;
            BattleEventRenderer.FieldControl.ScaledMouseMove += BattleEventRenderer.FieldControl_MouseMove;
            BattleEventRenderer.FieldControl.ScaledMouseDown += BattleEventRenderer.FieldControl_MouseDown;
            BattleEventRenderer.FieldControl.ScaledMouseUp += BattleEventRenderer.FieldControl_MouseUp;

            var patternSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            BattleEventRenderer.PatternRenderer = patternSpriteRendererPanel;
            BattleEventRenderer.PatternControl = patternSpriteRendererPanel;
            BattleEventRenderer.PatternControl.ScaledMouseMove += BattleEventRenderer.PatternControl_MouseMove;
            BattleEventRenderer.PatternControl.ScaledMouseDown += BattleEventRenderer.PatternControl_MouseDown;
            BattleEventRenderer.PatternControl.ScaledMouseUp += BattleEventRenderer.PatternControl_MouseUp;

            var enemySpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            BattleEventRenderer.EnemyRenderer = enemySpriteRendererPanel;
            BattleEventRenderer.EnemyControl = enemySpriteRendererPanel;
            BattleEventRenderer.EnemyControl.ScaledMouseDown += BattleEventRenderer.EnemyControl_MouseDown;
            BattleEventRenderer.EnemyControl.ScaledMouseUp += BattleEventRenderer.EnemyControl_MouseUp;

            var enemySelectionSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            BattleEventRenderer.EnemySelectionRenderer = enemySelectionSpriteRendererPanel;
            BattleEventRenderer.EnemySelectionControl = enemySelectionSpriteRendererPanel;
            BattleEventRenderer.EnemySelectionControl.ScaledMouseMove += BattleEventRenderer.EnemySelectionControl_MouseMove;
            BattleEventRenderer.EnemySelectionControl.ScaledMouseDown += BattleEventRenderer.EnemySelectionControl_MouseDown;
            BattleEventRenderer.EnemySelectionControl.ScaledMouseUp += BattleEventRenderer.EnemySelectionControl_MouseUp;

            var primaryPanelSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            BattleEventRenderer.PrimaryPanelRenderer = primaryPanelSpriteRendererPanel;
            BattleEventRenderer.PrimaryPanelControl = primaryPanelSpriteRendererPanel;
            BattleEventRenderer.PrimaryPanelControl.ScaledMouseDown += BattleEventRenderer.PrimaryPanelControl_MouseDown;
            BattleEventRenderer.PrimaryPanelControl.ScaledMouseUp += BattleEventRenderer.PrimaryPanelControl_MouseUp;
            BattleEventRenderer.PrimaryPanelControl.ScaledMouseEnter += BattleEventRenderer.PrimaryPanelControl_MouseEnter;
            BattleEventRenderer.PrimaryPanelControl.ScaledMouseLeave += BattleEventRenderer.PrimaryPanelControl_MouseLeave;

            var secondaryPanelSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            BattleEventRenderer.SecondaryPanelRenderer = secondaryPanelSpriteRendererPanel;
            BattleEventRenderer.SecondaryPanelControl = secondaryPanelSpriteRendererPanel;
            BattleEventRenderer.SecondaryPanelControl.ScaledMouseDown += BattleEventRenderer.SecondaryPanelControl_MouseDown;
            BattleEventRenderer.SecondaryPanelControl.ScaledMouseUp += BattleEventRenderer.SecondaryPanelControl_MouseUp;
            BattleEventRenderer.SecondaryPanelControl.ScaledMouseEnter += BattleEventRenderer.SecondaryPanelControl_MouseEnter;
            BattleEventRenderer.SecondaryPanelControl.ScaledMouseLeave += BattleEventRenderer.SecondaryPanelControl_MouseLeave;

            var panelSelectionSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            BattleEventRenderer.PanelSelectionRenderer = panelSelectionSpriteRendererPanel;
            BattleEventRenderer.PanelSelectionControl = panelSelectionSpriteRendererPanel;
            BattleEventRenderer.PanelSelectionControl.ScaledMouseMove += BattleEventRenderer.PanelSelectionControl_MouseMove;
            BattleEventRenderer.PanelSelectionControl.ScaledMouseDown += BattleEventRenderer.PanelSelectionControl_MouseDown;
            BattleEventRenderer.PanelSelectionControl.ScaledMouseUp += BattleEventRenderer.PanelSelectionControl_MouseUp;

            SpriteRendererPanel.TexturesReloaded += () =>
            {
                fieldSpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
                patternSpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
                enemySpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
                enemySelectionSpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
                primaryPanelSpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
                secondaryPanelSpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
                panelSelectionSpriteRendererPanel.TextureLoadStrategy = Constants.TextureLoadStrategy;
            };
        }

        public static RandomEncounter CurrentEncounter
        {
            get
            {
                return BattleEventRenderer.CurrentBattleEvent.Encounter;
            }
            set
            {
                var previousEncounter = BattleEventRenderer.CurrentBattleEvent.Encounter;
                BattleEventRenderer.CurrentBattleEvent.Encounter = value;
                if (previousEncounter != null && BattleEventRenderer.CurrentBattleEvent.Encounter != null && !BattleEventRenderer.CurrentBattleEvent.Encounter.HasErrors)
                {
                    BattleEventRenderer.DrawField();
                    BattleEventRenderer.DrawPanels();
                    BattleEventRenderer.DrawPatterns();
                    BattleEventRenderer.UpdateDrawnEnemy();
                    BattleEventRenderer.DrawPrimaryPanel();
                    BattleEventRenderer.DrawSecondaryPanel();
                }
            }
        }

        public static Enemy CurrentEnemy
        {
            get
            {
                return BattleEventRenderer.CurrentEncounter.SelectedEnemy;
            }

            set
            {
                BattleEventRenderer.CurrentEncounter.SelectedEnemy = value;
            }
        }

        public static WindowsFormsHost FieldControlHost => BattleEventRenderer.CreateStaticWindowsFormsHost(BattleEventRenderer.FieldControl, BattleEventRenderer.DrawField);
        public static WindowsFormsHost PatternControlHost => BattleEventRenderer.CreateStaticWindowsFormsHost(BattleEventRenderer.PatternControl, BattleEventRenderer.DrawPatterns);
        public static ICommand ShowPatternSelectCommand => new RelayCommand(BattleEventRenderer.ShowPatternSelect);
        public static WindowsFormsHost EnemyControlHost => BattleEventRenderer.CreateStaticWindowsFormsHost(BattleEventRenderer.EnemyControl, BattleEventRenderer.DrawEnemy);
        public static WindowsFormsHost EnemySelectionControlHost => BattleEventRenderer.CreateStaticWindowsFormsHost(BattleEventRenderer.EnemySelectionControl, BattleEventRenderer.DrawEnemies);
        public static ICommand ShowEnemySelectCommand => new RelayCommand(BattleEventRenderer.ShowEnemySelect);
        public static WindowsFormsHost PrimaryPanelControlHost => BattleEventRenderer.CreateStaticWindowsFormsHost(BattleEventRenderer.PrimaryPanelControl, BattleEventRenderer.DrawPrimaryPanel);
        public static WindowsFormsHost SecondaryPanelControlHost => BattleEventRenderer.CreateStaticWindowsFormsHost(BattleEventRenderer.SecondaryPanelControl, BattleEventRenderer.DrawSecondaryPanel);
        public static WindowsFormsHost PanelSelectionControlHost => BattleEventRenderer.CreateStaticWindowsFormsHost(BattleEventRenderer.PanelSelectionControl, BattleEventRenderer.DrawPanels);
        public static ICommand ShowPrimaryPanelSelectCommand => new RelayCommand(() => BattleEventRenderer.ShowPanelSelect(true));
        public static ICommand ShowSecondaryPanelSelectCommand => new RelayCommand(() => BattleEventRenderer.ShowPanelSelect(false));

        public static Window PatternWindow { get; set; }
        public static Window EnemySelectionWindow { get; set; }
        public static Window PanelSelectionWindow { get; set; }

        public static BattleEvent CurrentBattleEvent { get; set; }

        private static IMouseInteractionControl FieldControl { get; }
        private static ISpriteRenderer FieldRenderer { get; }
        private static IMouseInteractionControl PatternControl { get; }
        private static ISpriteRenderer PatternRenderer { get; }
        private static IMouseInteractionControl EnemyControl { get; }
        private static ISpriteRenderer EnemyRenderer { get; }
        private static IMouseInteractionControl EnemySelectionControl { get; }
        private static ISpriteRenderer EnemySelectionRenderer { get; }
        private static IMouseInteractionControl PrimaryPanelControl { get; }
        private static ISpriteRenderer PrimaryPanelRenderer { get; }
        private static IMouseInteractionControl SecondaryPanelControl { get; }
        private static ISpriteRenderer SecondaryPanelRenderer { get; }
        private static IMouseInteractionControl PanelSelectionControl { get; }
        private static ISpriteRenderer PanelSelectionRenderer { get; }
        private static bool IsSettingPrimaryPanel { get; set; }

        public static void HideWindows()
        {
            BattleEventRenderer.PatternWindow?.Hide();
            BattleEventRenderer.EnemySelectionWindow?.Hide();
            BattleEventRenderer.PanelSelectionWindow?.Hide();
        }

        public static void UpdateDrawnEnemy()
        {
            BattleEventRenderer.DrawEnemy();
            if (BattleEventRenderer.EnemySelectionWindow != null && BattleEventRenderer.EnemySelectionWindow.IsVisible)
            {
                BattleEventRenderer.DrawEnemies();
            }
        }

        private static WindowsFormsHost CreateStaticWindowsFormsHost(IMouseInteractionControl rendererControl, Action renderAction, bool scrollable = true)
        {
            var control = rendererControl.GetControl();
            control.Paint += (s, e) => renderAction();
            if (scrollable)
            {
                return new ScrollViewerWindowsFormsHost { Child = control };
            }
            else
            {
                return new WindowsFormsHost { Child = control };
            }
        }

        private static void ShowPatternSelect()
        {
            if (BattleEventRenderer.PatternWindow == null)
            {
                BattleEventRenderer.PatternWindow = new Window
                {
                    Content = BattleEventRenderer.PatternControlHost,
                    Owner = LoadingWindowViewModel.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.CanMinimize,
                    Title = "Field Pattern Selection",
                    Width = 260 * 4 + 20,
                    Height = 600 + 40
                };
                BattleEventRenderer.PatternWindow.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    BattleEventRenderer.PatternWindow.Hide();
                };
            }
            BattleEventRenderer.PatternWindow.Show();
        }

        private static void ShowEnemySelect()
        {
            if (BattleEventRenderer.EnemySelectionWindow == null)
            {
                BattleEventRenderer.EnemySelectionWindow = new Window
                {
                    Content = BattleEventRenderer.EnemySelectionControlHost,
                    Owner = LoadingWindowViewModel.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.CanMinimize,
                    Title = "Enemy Selection",
                    Width = 80 * 10 + 125,
                    Height = 80 * 11 + 65
                };
                BattleEventRenderer.EnemySelectionWindow.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    BattleEventRenderer.EnemySelectionWindow.Hide();
                };
            }
            BattleEventRenderer.EnemySelectionWindow.Show();
        }

        private static void ShowPanelSelect(bool isPrimary)
        {
            BattleEventRenderer.IsSettingPrimaryPanel = isPrimary;
            if (BattleEventRenderer.PanelSelectionWindow == null)
            {
                BattleEventRenderer.PanelSelectionWindow = new Window
                {
                    Content = BattleEventRenderer.PanelSelectionControlHost,
                    Owner = LoadingWindowViewModel.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.CanMinimize,
                    Title = "Panel Selection",
                    Width = 54 * 4 + 10,
                    Height = 42 * 4 + 10
                };
                BattleEventRenderer.PanelSelectionWindow.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    BattleEventRenderer.PanelSelectionWindow.Hide();
                };
            }
            BattleEventRenderer.PanelSelectionWindow.Show();
        }
    }
}
