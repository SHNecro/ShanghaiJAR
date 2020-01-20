using MapEditor.Models;
using MapEditor.Rendering;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for Styles.xaml
    /// </summary>
    public partial class Styles : ResourceDictionary
    {
        public Styles()
        {
            InitializeComponent();
        }

        private void ListObject_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var enteredMapObject = (MapObject)(((ListBoxItem)sender).DataContext);
            MapRenderer.ListHoveredMapObject = enteredMapObject;
        }

        private void ListObject_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((ListBoxItem)sender).ReleaseMouseCapture();
            MapRenderer.ListHoveredMapObject = null;
        }

        private void ListMove_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var enteredMapObject = (Move)(((ListBoxItem)sender).DataContext);
            MapRenderer.ListHoveredMove = enteredMapObject;
        }

        private void ListMove_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((ListBoxItem)sender).ReleaseMouseCapture();
            MapRenderer.ListHoveredMove = null;
        }

        private void SetTagProperty_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var binding = button.GetBindingExpression(Button.TagProperty);
            var boundProperty = binding.DataItem.GetType().GetProperty(binding.ResolvedSourcePropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var toSet = Convert.ChangeType(button.CommandParameter, boundProperty.PropertyType);
            boundProperty.SetValue(binding.DataItem, toSet);
            binding.UpdateSource();
        }

        private void SetTagProperty_CheckUncheck(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var binding = checkBox.GetBindingExpression(CheckBox.TagProperty);
            var boundProperty = binding.DataItem.GetType().GetProperty(binding.ResolvedSourcePropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var toSetArray = checkBox.CommandParameter as object[];
            var toSet = Convert.ChangeType(toSetArray[(checkBox.IsChecked ?? false) ? 0 : 1], boundProperty.PropertyType);
            boundProperty.SetValue(binding.DataItem, toSet);
            binding.UpdateSource();
        }

        private void SelectObject_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var binding = button.GetBindingExpression(Button.TagProperty);
            var boundProperty = binding.DataItem.GetType().GetProperty(binding.ResolvedSourcePropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var toSelectID = boundProperty.GetValue(binding.DataItem) as string;

            if (toSelectID != null)
            {
                var toSelect = MapRenderer.CurrentMap.MapObjects.MapObjects.FirstOrDefault(mo => mo.ID == toSelectID);

                if (toSelect != null)
                {
                    MapRenderer.CurrentMap.MapObjects.SelectedObject = toSelect;
                }
            }
        }

        private void SelectObjectIndex_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var binding = button.GetBindingExpression(Button.TagProperty);
            var boundProperty = binding.DataItem.GetType().GetProperty(binding.ResolvedSourcePropertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var toSelectIndexInt = boundProperty.GetValue(binding.DataItem) as int?;

            if (toSelectIndexInt != null && toSelectIndexInt != -1)
            {
                var toSelect = toSelectIndexInt >= MapRenderer.CurrentMap.MapObjects.MapObjects.Count ? null : MapRenderer.CurrentMap.MapObjects.MapObjects[toSelectIndexInt.Value];

                if (toSelect != null)
                {
                    MapRenderer.CurrentMap.MapObjects.SelectedObject = toSelect;
                }
            }
        }

        private void ComboBox_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            ((ComboBox)sender).GetBindingExpression(ComboBox.SelectedValueProperty).UpdateTarget();
        }
    }
}
