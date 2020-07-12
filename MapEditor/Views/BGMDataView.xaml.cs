using MapEditor.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapEditor.Views
{
    /// <summary>
    /// Interaction logic for BGMDataView.xaml
    /// </summary>
    public partial class BGMDataView : UserControl
    {
        public BGMDataView()
        {
            InitializeComponent();
        }

        private void EntryKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) ==
                ModifierKeys.Control)
            {
                if (this.DataContext is BGMDataViewModel vm)
                {
                    vm.UndoCommand.Execute(null);
                }
            }
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) ==
                ModifierKeys.Control)
            {
                if (this.DataContext is BGMDataViewModel vm)
                {
                    vm.SaveCommand.Execute(null);
                }
            }
        }

        private void LoopMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Manual implementation of wheel events because of IntegerUpDown property update bug
            if (sender is TextBox textBox)
            {
                var sample = Convert.ToInt64(textBox.Text);
                var diff = e.Delta > 0 ? 4410 : -4410;
                var newValue = sample + diff;
                textBox.SetCurrentValue(TextBox.TextProperty, newValue.ToString());
            }
        }
    }
}
