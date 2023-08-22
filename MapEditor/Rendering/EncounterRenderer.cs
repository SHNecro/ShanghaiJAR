using MapEditor.Controls;
using MapEditor.Core;
using Common.OpenGL;
using MapEditor.Models;
using MapEditor.ViewModels;
using System;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;

namespace MapEditor.Rendering
{
    public static partial class EncounterRenderer
    {
        private const int RenderPassPadding = 5;

        static EncounterRenderer()
        {
            var fieldSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            EncounterRenderer.FieldRenderer = fieldSpriteRendererPanel;
            EncounterRenderer.FieldControl = fieldSpriteRendererPanel;
            EncounterRenderer.FieldControl.ScaledMouseMove += EncounterRenderer.FieldControl_MouseMove;
            EncounterRenderer.FieldControl.ScaledMouseDown += EncounterRenderer.FieldControl_MouseDown;
            EncounterRenderer.FieldControl.ScaledMouseUp += EncounterRenderer.FieldControl_MouseUp;

            var patternSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            EncounterRenderer.PatternRenderer = patternSpriteRendererPanel;
            EncounterRenderer.PatternControl = patternSpriteRendererPanel;
            EncounterRenderer.PatternControl.ScaledMouseMove += EncounterRenderer.PatternControl_MouseMove;
            EncounterRenderer.PatternControl.ScaledMouseDown += EncounterRenderer.PatternControl_MouseDown;
            EncounterRenderer.PatternControl.ScaledMouseUp += EncounterRenderer.PatternControl_MouseUp;

            var enemySpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            EncounterRenderer.EnemyRenderer = enemySpriteRendererPanel;
            EncounterRenderer.EnemyControl = enemySpriteRendererPanel;
            EncounterRenderer.EnemyControl.ScaledMouseDown += EncounterRenderer.EnemyControl_MouseDown;
            EncounterRenderer.EnemyControl.ScaledMouseUp += EncounterRenderer.EnemyControl_MouseUp;

            var enemySelectionSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            EncounterRenderer.EnemySelectionRenderer = enemySelectionSpriteRendererPanel;
            EncounterRenderer.EnemySelectionControl = enemySelectionSpriteRendererPanel;
            EncounterRenderer.EnemySelectionControl.ScaledMouseMove += EncounterRenderer.EnemySelectionControl_MouseMove;
            EncounterRenderer.EnemySelectionControl.ScaledMouseDown += EncounterRenderer.EnemySelectionControl_MouseDown;
            EncounterRenderer.EnemySelectionControl.ScaledMouseUp += EncounterRenderer.EnemySelectionControl_MouseUp;

            var primaryPanelSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            EncounterRenderer.PrimaryPanelRenderer = primaryPanelSpriteRendererPanel;
            EncounterRenderer.PrimaryPanelControl = primaryPanelSpriteRendererPanel;
            EncounterRenderer.PrimaryPanelControl.ScaledMouseDown += EncounterRenderer.PrimaryPanelControl_MouseDown;
            EncounterRenderer.PrimaryPanelControl.ScaledMouseUp += EncounterRenderer.PrimaryPanelControl_MouseUp;
            EncounterRenderer.PrimaryPanelControl.ScaledMouseEnter += EncounterRenderer.PrimaryPanelControl_MouseEnter;
            EncounterRenderer.PrimaryPanelControl.ScaledMouseLeave += EncounterRenderer.PrimaryPanelControl_MouseLeave;

            var secondaryPanelSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            EncounterRenderer.SecondaryPanelRenderer = secondaryPanelSpriteRendererPanel;
            EncounterRenderer.SecondaryPanelControl = secondaryPanelSpriteRendererPanel;
            EncounterRenderer.SecondaryPanelControl.ScaledMouseDown += EncounterRenderer.SecondaryPanelControl_MouseDown;
            EncounterRenderer.SecondaryPanelControl.ScaledMouseUp += EncounterRenderer.SecondaryPanelControl_MouseUp;
            EncounterRenderer.SecondaryPanelControl.ScaledMouseEnter += EncounterRenderer.SecondaryPanelControl_MouseEnter;
            EncounterRenderer.SecondaryPanelControl.ScaledMouseLeave += EncounterRenderer.SecondaryPanelControl_MouseLeave;

            var panelSelectionSpriteRendererPanel = new SpriteRendererPanel(Constants.TextureLoadStrategy);
            EncounterRenderer.PanelSelectionRenderer = panelSelectionSpriteRendererPanel;
            EncounterRenderer.PanelSelectionControl = panelSelectionSpriteRendererPanel;
            EncounterRenderer.PanelSelectionControl.ScaledMouseMove += EncounterRenderer.PanelSelectionControl_MouseMove;
            EncounterRenderer.PanelSelectionControl.ScaledMouseDown += EncounterRenderer.PanelSelectionControl_MouseDown;
            EncounterRenderer.PanelSelectionControl.ScaledMouseUp += EncounterRenderer.PanelSelectionControl_MouseUp;

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
                return MainWindowViewModel.GetCurrentMap()?.RandomEncounters?.SelectedEncounter;
            }
            set
            {
                var previousEncounter = EncounterRenderer.CurrentEncounter;
                EncounterRenderer.CurrentEncounter = value;
                if (previousEncounter != null && EncounterRenderer.CurrentEncounter != null && !EncounterRenderer.CurrentEncounter.HasErrors)
                {
                    EncounterRenderer.DrawField();
                    EncounterRenderer.DrawPanels();
                    EncounterRenderer.DrawPatterns();
                    EncounterRenderer.UpdateSelectedEnemy();
                    EncounterRenderer.DrawPrimaryPanel();
                    EncounterRenderer.DrawSecondaryPanel();
                }
            }
        }

        public static Enemy CurrentEnemy
        {
            get
            {
                return EncounterRenderer.CurrentEncounter.SelectedEnemy;
            }

            set
            {
                EncounterRenderer.CurrentEncounter.SelectedEnemy = value;
            }
        }

        public static WindowsFormsHost FieldControlHost => EncounterRenderer.CreateStaticWindowsFormsHost(EncounterRenderer.FieldControl, EncounterRenderer.DrawField);
        public static WindowsFormsHost PatternControlHost => EncounterRenderer.CreateStaticWindowsFormsHost(EncounterRenderer.PatternControl, EncounterRenderer.DrawPatterns);
        public static ICommand ShowPatternSelectCommand => new RelayCommand(EncounterRenderer.ShowPatternSelect);
        public static WindowsFormsHost EnemyControlHost => EncounterRenderer.CreateStaticWindowsFormsHost(EncounterRenderer.EnemyControl, EncounterRenderer.DrawEnemy);
        public static WindowsFormsHost EnemySelectionControlHost => EncounterRenderer.CreateStaticWindowsFormsHost(EncounterRenderer.EnemySelectionControl, EncounterRenderer.DrawEnemies);
        public static ICommand ShowEnemySelectCommand => new RelayCommand(EncounterRenderer.ShowEnemySelect);
        public static WindowsFormsHost PrimaryPanelControlHost => EncounterRenderer.CreateStaticWindowsFormsHost(EncounterRenderer.PrimaryPanelControl, EncounterRenderer.DrawPrimaryPanel);
        public static WindowsFormsHost SecondaryPanelControlHost => EncounterRenderer.CreateStaticWindowsFormsHost(EncounterRenderer.SecondaryPanelControl, EncounterRenderer.DrawSecondaryPanel);
        public static WindowsFormsHost PanelSelectionControlHost => EncounterRenderer.CreateStaticWindowsFormsHost(EncounterRenderer.PanelSelectionControl, EncounterRenderer.DrawPanels);
        public static ICommand ShowPrimaryPanelSelectCommand => new RelayCommand(() => EncounterRenderer.ShowPanelSelect(true));
        public static ICommand ShowSecondaryPanelSelectCommand => new RelayCommand(() => EncounterRenderer.ShowPanelSelect(false));

        public static Window PatternWindow { get; set; }
        public static Window EnemySelectionWindow { get; set; }
        public static Window PanelSelectionWindow { get; set; }

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
            EncounterRenderer.PatternWindow?.Hide();
            EncounterRenderer.EnemySelectionWindow?.Hide();
            EncounterRenderer.PanelSelectionWindow?.Hide();
        }

        public static void UpdateSelectedEnemy()
        {
            if (EncounterRenderer.EnemySelectionWindow != null && EncounterRenderer.EnemySelectionWindow.IsVisible)
            {
                EncounterRenderer.DrawEnemies();
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
            if (EncounterRenderer.PatternWindow == null)
            {
                EncounterRenderer.PatternWindow = new Window
                {
                    Content = EncounterRenderer.PatternControlHost,
                    Owner = LoadingWindowViewModel.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.CanMinimize,
                    Title = "Field Pattern Selection",
                    Width = 260 * 4 + 20,
                    Height = 600 + 40
                };
                EncounterRenderer.PatternWindow.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    EncounterRenderer.PatternWindow.Hide();
                };
            }
            EncounterRenderer.PatternWindow.Show();
        }

        private static void ShowEnemySelect()
        {
            if (EncounterRenderer.EnemySelectionWindow == null)
            {
                EncounterRenderer.EnemySelectionWindow = new Window
                {
                    Content = EncounterRenderer.EnemySelectionControlHost,
                    Owner = LoadingWindowViewModel.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.CanMinimize,
                    Title = "Enemy Selection",
					Width = 80 * 10 + 125,
					Height = 80 * 11 + 65
				};
                EncounterRenderer.EnemySelectionWindow.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    EncounterRenderer.EnemySelectionWindow.Hide();
                };
            }
            EncounterRenderer.EnemySelectionWindow.Show();
        }

        private static void ShowPanelSelect(bool isPrimary)
        {
            EncounterRenderer.IsSettingPrimaryPanel = isPrimary;
            if (EncounterRenderer.PanelSelectionWindow == null)
            {
                EncounterRenderer.PanelSelectionWindow = new Window
                {
                    Content = EncounterRenderer.PanelSelectionControlHost,
                    Owner = LoadingWindowViewModel.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ResizeMode = ResizeMode.CanMinimize,
                    Title = "Panel Selection",
                    Width = 54 * 4 + 10,
                    Height = 42 * 4 + 10
                };
                EncounterRenderer.PanelSelectionWindow.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    EncounterRenderer.PanelSelectionWindow.Hide();
                };
            }
            EncounterRenderer.PanelSelectionWindow.Show();
        }
    }
}
