using MapEditor.Models;
using System;
using System.Drawing;

namespace MapEditor.Rendering
{
    public static partial class BattleEventRenderer
    {
        private static Point? FieldMousePosition { get; set; }
        private static Point? FieldMouseDownPosition { get; set; }
        private static Point? FieldMouseDragReleasePosition { get; set; }
        private static Enemy FieldMouseDragEnemy { get; set; }

        private static Point? PatternMousePosition { get; set; }
        private static Point? PatternMouseClickPosition { get; set; }
        private static Point? PatternMouseClickConfirmPosition { get; set; }

        private static bool IsEnemyMouseDown { get; set; }

        private static Point? EnemySelectionMousePosition { get; set; }
        private static Point? EnemySelectionMouseClickPosition { get; set; }
        private static Point? EnemySelectionMouseClickConfirmPosition { get; set; }

        private static bool IsPrimaryPanelMouseDown { get; set; }
        private static bool IsHoveringPrimaryPanel { get; set; }
        private static bool IsSecondaryPanelMouseDown { get; set; }
        private static bool IsHoveringSecondaryPanel { get; set; }

        private static Point? PanelSelectionMousePosition { get; set; }
        private static Point? PanelSelectionMouseClickPosition { get; set; }
        private static Point? PanelSelectionMouseClickConfirmPosition { get; set; }

        private static void FieldControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            BattleEventRenderer.FieldMousePosition = e.Location;
            BattleEventRenderer.DrawField();
        }

        private static void FieldControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!BattleEventRenderer.FieldMouseDownPosition.HasValue)
            {
                BattleEventRenderer.FieldMouseDownPosition = e.Location;
                BattleEventRenderer.DrawField();
            }
        }

        private static void FieldControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (BattleEventRenderer.FieldMouseDownPosition.HasValue)
            {
                BattleEventRenderer.FieldMouseDragReleasePosition = e.Location;
                BattleEventRenderer.DrawField();
            }
            BattleEventRenderer.FieldMouseDownPosition = null;
        }

        private static void PatternControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            BattleEventRenderer.PatternMousePosition = e.Location;
            BattleEventRenderer.DrawPatterns();
        }

        private static void PatternControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!BattleEventRenderer.PatternMouseClickPosition.HasValue)
            {
                BattleEventRenderer.PatternMouseClickPosition = e.Location;
                BattleEventRenderer.DrawPatterns();
            }
        }

        private static void PatternControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!BattleEventRenderer.PatternMouseClickConfirmPosition.HasValue)
            {
                BattleEventRenderer.PatternMouseClickConfirmPosition = e.Location;
                BattleEventRenderer.DrawPatterns();
            }
        }

        private static void EnemyControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            BattleEventRenderer.IsEnemyMouseDown = true;
            ((System.Windows.Forms.Control)sender).Capture = false;
            BattleEventRenderer.DrawEnemy();
        }

        private static void EnemyControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (BattleEventRenderer.IsEnemyMouseDown)
            {
                BattleEventRenderer.ShowEnemySelect();
            }
            BattleEventRenderer.IsEnemyMouseDown = false;
            BattleEventRenderer.DrawEnemy();
            BattleEventRenderer.DrawEnemies();
        }

        private static void EnemySelectionControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            BattleEventRenderer.EnemySelectionMousePosition = e.Location;
            BattleEventRenderer.DrawEnemies();
        }

        private static void EnemySelectionControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!BattleEventRenderer.EnemySelectionMouseClickPosition.HasValue)
            {
                BattleEventRenderer.EnemySelectionMouseClickPosition = e.Location;
                BattleEventRenderer.DrawEnemies();
            }
        }

        private static void EnemySelectionControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!BattleEventRenderer.EnemySelectionMouseClickConfirmPosition.HasValue)
            {
                BattleEventRenderer.EnemySelectionMouseClickConfirmPosition = e.Location;
                BattleEventRenderer.DrawEnemies();
            }
        }

        private static void PrimaryPanelControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            BattleEventRenderer.IsPrimaryPanelMouseDown = true;
            ((System.Windows.Forms.Control)sender).Capture = false;
            BattleEventRenderer.DrawPrimaryPanel();
        }

        private static void PrimaryPanelControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (BattleEventRenderer.IsPrimaryPanelMouseDown)
            {
                BattleEventRenderer.ShowPanelSelect(true);
            }
            BattleEventRenderer.IsPrimaryPanelMouseDown = false;
            BattleEventRenderer.DrawPrimaryPanel();
            BattleEventRenderer.DrawPanels();
        }

        private static void PrimaryPanelControl_MouseEnter(object sender, EventArgs e)
        {
            BattleEventRenderer.IsHoveringPrimaryPanel = true;
            BattleEventRenderer.DrawPrimaryPanel();
            BattleEventRenderer.DrawField();
        }

        private static void PrimaryPanelControl_MouseLeave(object sender, EventArgs e)
        {
            BattleEventRenderer.IsPrimaryPanelMouseDown = false;
            BattleEventRenderer.IsHoveringPrimaryPanel = false;
            BattleEventRenderer.DrawPrimaryPanel();
            BattleEventRenderer.DrawField();
        }

        private static void SecondaryPanelControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            BattleEventRenderer.IsSecondaryPanelMouseDown = true;
            ((System.Windows.Forms.Control)sender).Capture = false;
            BattleEventRenderer.DrawSecondaryPanel();
        }

        private static void SecondaryPanelControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (BattleEventRenderer.IsSecondaryPanelMouseDown)
            {
                BattleEventRenderer.ShowPanelSelect(false);
            }
            BattleEventRenderer.IsSecondaryPanelMouseDown = false;
            BattleEventRenderer.DrawSecondaryPanel();
            BattleEventRenderer.DrawPanels();
        }

        private static void SecondaryPanelControl_MouseEnter(object sender, EventArgs e)
        {
            BattleEventRenderer.IsHoveringSecondaryPanel = true;
            BattleEventRenderer.DrawSecondaryPanel();
            BattleEventRenderer.DrawField();
        }

        private static void SecondaryPanelControl_MouseLeave(object sender, EventArgs e)
        {
            BattleEventRenderer.IsSecondaryPanelMouseDown = false;
            BattleEventRenderer.IsHoveringSecondaryPanel = false;
            BattleEventRenderer.DrawSecondaryPanel();
            BattleEventRenderer.DrawField();
        }

        private static void PanelSelectionControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            BattleEventRenderer.PanelSelectionMousePosition = e.Location;
            BattleEventRenderer.DrawPanels();
        }

        private static void PanelSelectionControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!BattleEventRenderer.PanelSelectionMouseClickPosition.HasValue)
            {
                BattleEventRenderer.PanelSelectionMouseClickPosition = e.Location;
                BattleEventRenderer.DrawPanels();
            }
        }

        private static void PanelSelectionControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!BattleEventRenderer.PanelSelectionMouseClickConfirmPosition.HasValue)
            {
                BattleEventRenderer.PanelSelectionMouseClickConfirmPosition = e.Location;
                BattleEventRenderer.DrawPanels();
            }
        }
    }
}
