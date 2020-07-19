using MapEditor.ViewModels;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapEditor.Views
{
    /// <summary>
    /// Interaction logic for BGMDataView.xaml
    /// </summary>
    public partial class BGMDataView : UserControl
    {
        private Action lastSentSeek;
        private Timer heldMouseResendTimer;
        private bool? originallyIsLooping;

        public BGMDataView()
        {
            InitializeComponent();

            this.heldMouseResendTimer = new Timer { Interval = 100, AutoReset = true, Enabled = false };
            this.heldMouseResendTimer.Elapsed += (sender, args) => { this.lastSentSeek?.Invoke(); };
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
                if (!long.TryParse(textBox.Text, out long sample))
                {
                    return;
                }

                var diff = e.Delta > 0 ? 4410 : -4410;
                var newValue = sample + diff;
                textBox.SetCurrentValue(TextBox.TextProperty, newValue.ToString());
            }
        }

        private void Border_MouseDown(object sender, MouseEventArgs e)
        {
            var element = sender as Border;
            if (element == null)
            {
                return;
            }

            this.heldMouseResendTimer.Start();
            this.heldMouseResendTimer.AutoReset = true;

            element.CaptureMouse();
            this.Border_MouseEvent(sender, e);
        }

        private void Border_MouseUp(object sender, MouseEventArgs e)
        {
            var element = sender as Border;
            if (element == null)
            {
                return;
            }

            this.heldMouseResendTimer.Stop();
            this.heldMouseResendTimer.AutoReset = false;

            element.ReleaseMouseCapture();
            this.Border_MouseEvent(sender, e);

            this.originallyIsLooping = null;
        }

        private void Border_MouseEvent(object sender, MouseEventArgs e)
        {
            var element = sender as Border;
            if (element == null)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.heldMouseResendTimer.Stop();
                this.heldMouseResendTimer.Start();

                var mousePos = e.GetPosition(element);
                var percent = Math.Min(1.0, Math.Max(0.0, mousePos.X / element.ActualWidth));

                if (this.DataContext is BGMDataViewModel vm)
                {
                    this.lastSentSeek = () => vm.SeekToPercent(percent);
                    this.lastSentSeek.Invoke();

                    var progressSamples = (long)(percent * vm.OggProgress.TotalSamples);
                    if (vm.IsLooping && progressSamples >= vm.PlayingBGM.LoopEnd)
                    {
                        this.originallyIsLooping = vm.IsLooping;
                        vm.IsLooping = false;
                    }
                    else if (!vm.IsLooping && progressSamples < vm.PlayingBGM.LoopEnd && (this.originallyIsLooping ?? false))
                    {
                        vm.IsLooping = true;
                    }
                }
            }
        }
    }
}
