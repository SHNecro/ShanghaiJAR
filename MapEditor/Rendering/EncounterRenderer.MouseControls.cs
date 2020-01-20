using MapEditor.Models;
using System;
using System.Drawing;

namespace MapEditor.Rendering
{
    public static partial class EncounterRenderer
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
            EncounterRenderer.FieldMousePosition = e.Location;
            EncounterRenderer.DrawField();
        }

        private static void FieldControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!EncounterRenderer.FieldMouseDownPosition.HasValue)
            {
                EncounterRenderer.FieldMouseDownPosition = e.Location;
                EncounterRenderer.DrawField();
            }
        }

        private static void FieldControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (EncounterRenderer.FieldMouseDownPosition.HasValue)
            {
                EncounterRenderer.FieldMouseDragReleasePosition = e.Location;
                EncounterRenderer.DrawField();
            }
            EncounterRenderer.FieldMouseDownPosition = null;
        }

        private static void PatternControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EncounterRenderer.PatternMousePosition = e.Location;
            EncounterRenderer.DrawPatterns();
        }

        private static void PatternControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!EncounterRenderer.PatternMouseClickPosition.HasValue)
            {
                EncounterRenderer.PatternMouseClickPosition = e.Location;
                EncounterRenderer.DrawPatterns();
            }
        }

        private static void PatternControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!EncounterRenderer.PatternMouseClickConfirmPosition.HasValue)
            {
                EncounterRenderer.PatternMouseClickConfirmPosition = e.Location;
                EncounterRenderer.DrawPatterns();
            }
        }

        private static void EnemyControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EncounterRenderer.IsEnemyMouseDown = true;
            ((System.Windows.Forms.Control)sender).Capture = false;
            EncounterRenderer.DrawEnemy();
        }

        private static void EnemyControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (EncounterRenderer.IsEnemyMouseDown)
            {
                EncounterRenderer.ShowEnemySelect();
            }
            EncounterRenderer.IsEnemyMouseDown = false;
            EncounterRenderer.DrawEnemy();
            EncounterRenderer.DrawEnemies();
        }

        private static void EnemySelectionControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EncounterRenderer.EnemySelectionMousePosition = e.Location;
            EncounterRenderer.DrawEnemies();
        }

        private static void EnemySelectionControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!EncounterRenderer.EnemySelectionMouseClickPosition.HasValue)
            {
                EncounterRenderer.EnemySelectionMouseClickPosition = e.Location;
                EncounterRenderer.DrawEnemies();
            }
        }

        private static void EnemySelectionControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!EncounterRenderer.EnemySelectionMouseClickConfirmPosition.HasValue)
            {
                EncounterRenderer.EnemySelectionMouseClickConfirmPosition = e.Location;
                EncounterRenderer.DrawEnemies();
            }
        }

        private static void PrimaryPanelControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EncounterRenderer.IsPrimaryPanelMouseDown = true;
            ((System.Windows.Forms.Control)sender).Capture = false;
            EncounterRenderer.DrawPrimaryPanel();
        }

        private static void PrimaryPanelControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (EncounterRenderer.IsPrimaryPanelMouseDown)
            {
                EncounterRenderer.ShowPanelSelect(true);
            }
            EncounterRenderer.IsPrimaryPanelMouseDown = false;
            EncounterRenderer.DrawPrimaryPanel();
            EncounterRenderer.DrawPanels();
        }

        private static void PrimaryPanelControl_MouseEnter(object sender, EventArgs e)
        {
            EncounterRenderer.IsHoveringPrimaryPanel = true;
            EncounterRenderer.DrawPrimaryPanel();
            EncounterRenderer.DrawField();
        }

        private static void PrimaryPanelControl_MouseLeave(object sender, EventArgs e)
        {
            EncounterRenderer.IsPrimaryPanelMouseDown = false;
            EncounterRenderer.IsHoveringPrimaryPanel = false;
            EncounterRenderer.DrawPrimaryPanel();
            EncounterRenderer.DrawField();
        }

        private static void SecondaryPanelControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EncounterRenderer.IsSecondaryPanelMouseDown = true;
            ((System.Windows.Forms.Control)sender).Capture = false;
            EncounterRenderer.DrawSecondaryPanel();
        }

        private static void SecondaryPanelControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (EncounterRenderer.IsSecondaryPanelMouseDown)
            {
                EncounterRenderer.ShowPanelSelect(false);
            }
            EncounterRenderer.IsSecondaryPanelMouseDown = false;
            EncounterRenderer.DrawSecondaryPanel();
            EncounterRenderer.DrawPanels();
        }

        private static void SecondaryPanelControl_MouseEnter(object sender, EventArgs e)
        {
            EncounterRenderer.IsHoveringSecondaryPanel = true;
            EncounterRenderer.DrawSecondaryPanel();
            EncounterRenderer.DrawField();
        }

        private static void SecondaryPanelControl_MouseLeave(object sender, EventArgs e)
        {
            EncounterRenderer.IsSecondaryPanelMouseDown = false;
            EncounterRenderer.IsHoveringSecondaryPanel = false;
            EncounterRenderer.DrawSecondaryPanel();
            EncounterRenderer.DrawField();
        }

        private static void PanelSelectionControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EncounterRenderer.PanelSelectionMousePosition = e.Location;
            EncounterRenderer.DrawPanels();
        }

        private static void PanelSelectionControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!EncounterRenderer.PanelSelectionMouseClickPosition.HasValue)
            {
                EncounterRenderer.PanelSelectionMouseClickPosition = e.Location;
                EncounterRenderer.DrawPanels();
            }
        }

        private static void PanelSelectionControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!EncounterRenderer.PanelSelectionMouseClickConfirmPosition.HasValue)
            {
                EncounterRenderer.PanelSelectionMouseClickConfirmPosition = e.Location;
                EncounterRenderer.DrawPanels();
            }
        }
    }
}
