using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models
{
    public class MapObjectCollection : StringRepresentation
	{
        private ObservableCollection<MapObject> mapObjects;
        private int lastSelectedIndex;
        private MapObject selectedObject;

        public MapObjectCollection()
        {
            this.MapObjects = new ObservableCollection<MapObject>();
		}

        public ObservableCollection<MapObject> MapObjects
        {
            get
            {
                return this.mapObjects;
            }
            set
            {
                if (this.MapObjects != null)
                {
                    this.MapObjects.CollectionChanged -= this.OnMapObjectCollectionChanged;
                }

                this.SetValue(ref this.mapObjects, value);
                this.MapObjects.CollectionChanged += this.OnMapObjectCollectionChanged;
                this.SelectedObject = this.MapObjects.FirstOrDefault();
            }
        }

        public MapObject SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                if (this.selectedObject != null)
                {
                    this.selectedObject.PropertyChanged -= this.OnSelectedObjectPropertyChanged;
                }
                if (value != null)
                {
                    value.PropertyChanged += this.OnSelectedObjectPropertyChanged;
                }

                this.SetValue(ref this.selectedObject, value);
                if (value != null || this.MapObjects.Count == 0)
                {
                    this.lastSelectedIndex = this.MapObjects.IndexOf(this.SelectedObject);
                }
            }
        }

        protected override string GetStringValue()
        {
			return string.Join("\r\n\r\n", this.MapObjects.OfType<MapEntity>().Select(mo => mo.StringValue))
                + "\r\n\r\n"
                + string.Join("\r\n\r\n", this.MapObjects.OfType<MapMystery>().Select(mo => mo.StringValue));
        }

        protected override void SetStringValue(string value)
        {
			var newMapObjects = value.Split(new[] { "\r\n\r\n", "\n\n" } , StringSplitOptions.RemoveEmptyEntries).Select(MapObject.FromString).OrderBy(e => e, Comparer<MapObject>.Create((a, b) =>
            {
                if (a == b)
                {
                    return 0;
                }
                else if (a is MapEntity && b is MapMystery)
                {
                    return -1;
                }
                else if (a is MapMystery && b is MapEntity)
                {
                    return 1;
                }

                return 0;
            })).ToList();

            this.MapObjects = new ObservableCollection<MapObject>(newMapObjects);
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            if (this.MapObjects == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }
            else
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>(this.MapObjects.SelectMany(sr => this.UpdateChildErrorStack(sr)));
            }
        }

        private void OnMapObjectCollectionChanged(object sender, EventArgs args)
        {
            if (this.SelectedObject != null && !this.MapObjects.Contains(this.SelectedObject))
            {
                this.SelectedObject = this.lastSelectedIndex < this.MapObjects.Count && this.lastSelectedIndex >= 0 ? this.MapObjects[this.lastSelectedIndex] : this.MapObjects.LastOrDefault();
            }
            this.OnPropertyChanged(nameof(this.MapObjects));
        }

        private void OnSelectedObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"SelectedObject.{e.PropertyName}");
        }
    }
}
