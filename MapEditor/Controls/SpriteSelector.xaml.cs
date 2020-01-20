using MapEditor.Core;
using MapEditor.Models;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for SpriteSelector.xaml
    /// </summary>
    public partial class SpriteSelector : UserControl
    {
        public static readonly DependencyProperty PageProperty = DependencyProperty.Register("Page", typeof(MapEventPage), typeof(SpriteSelector), new PropertyMetadata(null));

        public SpriteSelector()
        {
            InitializeComponent();
        }

        public MapEventPage Page
        {
            get
            {
                return (MapEventPage)this.GetValue(PageProperty);
            }

            set
            {
                this.SetValue(PageProperty, value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SpriteSelectionWindow.SetPageSetterAction((page) =>
            {
                this.Page = page;
            });
            SpriteSelectionWindow.ShowWindow(this.Page);
        }
    }
}
