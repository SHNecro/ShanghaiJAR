using MapEditor.ViewModels;
using System;
using System.Windows;

namespace MapEditor.Views
{
    /// <summary>
    /// Interaction logic for TranslationKeySelectionWindowView.xaml
    /// </summary>
    public partial class TranslationKeySelectionWindowView : Window
    {
        public TranslationKeySelectionWindowView()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                Constants.TranslationService.KeysReloaded += this.RefreshEntries;
            };
            this.Unloaded += (sender, args) =>
            {
                Constants.TranslationService.KeysReloaded -= this.RefreshEntries;
            };
        }

        public void ScrollToSelectedEntry()
        {
            this.EntriesDataGrid.ScrollIntoView(((TranslationKeySelectionWindowViewModel)this.DataContext).CurrentEntry);
        }

        private void RefreshEntries(object sender, EventArgs args)
        {
            this.EntriesDataGrid.Items.Refresh();
        }
    }
}
