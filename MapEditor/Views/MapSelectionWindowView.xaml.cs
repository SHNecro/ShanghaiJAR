using MapEditor.ViewModels;
using System.Windows;

namespace MapEditor.Views
{
    /// <summary>
    /// Interaction logic for MapSelectionWindowView.xaml
    /// </summary>
    public partial class MapSelectionWindowView : Window
    {
        public MapSelectionWindowView()
        {
            InitializeComponent();
        }

        public void ScrollToSelectedEntry()
        {
            this.MapListBox.ScrollIntoView(((MapSelectionWindowViewModel)this.DataContext).CurrentMapName);
        }
    }
}
