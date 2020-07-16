using MapEditor.Rendering;
using MapEditor.ViewModels;
using System;
using System.Windows;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		protected override void OnStateChanged(EventArgs e)
		{
			base.OnStateChanged(e);
			switch (WindowState)
			{
				case WindowState.Minimized:
					MapRenderer.IsDrawing = false;
					break;
				case WindowState.Normal:
                    MapRenderer.IsDrawing = true;
                    break;
			}
		}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !MainWindowViewModel.GetInstance().ConfirmMapChange();
            if (!e.Cancel)
            {
                Environment.Exit(Environment.ExitCode);
            }
        }
    }
}
