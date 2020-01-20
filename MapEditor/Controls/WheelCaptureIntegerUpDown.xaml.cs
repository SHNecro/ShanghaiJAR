using System.Windows.Input;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for WheelCaptureIntegerUpDown.xaml
    /// </summary>
    public partial class WheelCaptureIntegerUpDown
    {
        public WheelCaptureIntegerUpDown()
        {
            InitializeComponent();
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            if ((this.Value == this.Minimum && e.Delta < 0) || (this.Value == this.Maximum && e.Delta > 0))
            {
                switch (this.MouseWheelActiveTrigger)
                {
                    case Xceed.Wpf.Toolkit.Primitives.MouseWheelActiveTrigger.FocusedMouseOver:
                    case Xceed.Wpf.Toolkit.Primitives.MouseWheelActiveTrigger.Focused:
                        e.Handled = this.IsKeyboardFocusWithin;
                        break;
                    case Xceed.Wpf.Toolkit.Primitives.MouseWheelActiveTrigger.MouseOver:
                        e.Handled = true;
                        break;
                    case Xceed.Wpf.Toolkit.Primitives.MouseWheelActiveTrigger.Disabled:
                        e.Handled = false;
                        break;
                }
            }
        }
    }
}
