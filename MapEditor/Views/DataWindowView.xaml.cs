using MapEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MapEditor.Views
{
    /// <summary>
    /// Interaction logic for DataWindowView.xaml
    /// </summary>
    public partial class DataWindowView : Window
    {
        public DataWindowView()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            (this.DataContext as DataWindowViewModel)?.Remove();
        }
    }
}
