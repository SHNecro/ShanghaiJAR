using MapEditor.Core;
using MapEditor.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MapEditor.Controls
{
    /// <summary>
    /// Interaction logic for MapSelector.xaml
    /// </summary>
    public partial class MapSelector : UserControl
    {
        public static readonly DependencyProperty MapNameProperty = DependencyProperty.Register("MapName", typeof(string), typeof(MapSelector), new PropertyMetadata(null));

        public MapSelector()
        {
            InitializeComponent();
        }

        public string MapName
        {
            get
            {
                return (string)this.GetValue(MapNameProperty);
            }

            set
            {
                this.SetValue(MapNameProperty, value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MapSelectionWindow.SetMapSetterAction((mapName) =>
            {
                this.MapName = mapName;
            });
            MapSelectionWindow.ShowWindow(this.MapName);
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var encodedPath = $"{LoadingWindowViewModel.Settings.MapDataFolder}/{this.MapName}.she";
            if (File.Exists(encodedPath))
            {
                MainWindowViewModel.GetInstance().LoadMap(encodedPath, true);
            }
            else
            {
                MessageBox.Show($"The map file \"{encodedPath}\" could not be found.{Environment.NewLine}Please check that the correct folder is selected in the settings.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
