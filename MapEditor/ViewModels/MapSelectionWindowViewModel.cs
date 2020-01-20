using MapEditor.Core;
using MapEditor.Rendering;
using System;

namespace MapEditor.ViewModels
{
    public class MapSelectionWindowViewModel : ViewModelBase
    {
        private string selectedMapName;

        public string CurrentMapName
        {
            get
            {
                return MapSelectionRenderer.CurrentMapName;
            }
            set
            {
                MapSelectionRenderer.CurrentMapName = value;
                this.OnPropertyChanged(nameof(this.CurrentMapName));
            }
        }

        public string SelectedMapName
        {
            get
            {
                return this.selectedMapName;
            }
            set
            {
                this.SetValue(ref this.selectedMapName, value);
                this.MapNameSetterAction(value);
            }
        }

        public Action<string> MapNameSetterAction { get; set; }

        public void SelectMapName(string initialKey)
        {
            this.CurrentMapName = initialKey;
            this.SelectedMapName = initialKey;
        }
    }
}
