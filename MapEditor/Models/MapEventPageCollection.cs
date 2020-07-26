using MapEditor.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models
{
    public class MapEventPageCollection : StringRepresentation
	{
		private ObservableCollection<MapEventPage> mapEventPages;
        private int lastSelectedIndex;
		private MapEventPage selectedEventPage;

		public MapEventPageCollection()
		{
			this.MapEventPages = new ObservableCollection<MapEventPage>();
		}

        public ObservableCollection<MapEventPage> MapEventPages
		{
			get
			{
				return this.mapEventPages;
			}
			set
			{
				if (this.MapEventPages != null)
				{
					this.MapEventPages.CollectionChanged -= this.OnEventsCollectionChanged;
				}

				this.SetValue(ref this.mapEventPages, value);

				this.MapEventPages.CollectionChanged += this.OnEventsCollectionChanged;

				this.SelectedEventPage = this.MapEventPages.FirstOrDefault();
			}
		}

		public MapEventPage SelectedEventPage
		{
			get
			{
				return this.selectedEventPage;
			}
			set
			{
				if (value != null || this.MapEventPages.Count == 0)
                {
                    if (this.selectedEventPage != null)
                    {
                        this.selectedEventPage.PropertyChanged -= this.OnSelectedObjectPropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanged += this.OnSelectedObjectPropertyChanged;
                    }
                    this.SetValue(ref this.selectedEventPage, value);
                    this.lastSelectedIndex = this.MapEventPages.IndexOf(this.SelectedEventPage);
				}
			}
		}

        public MapEventPage this[int i] => this.MapEventPages[i];

        public static MapEventPageCollection FromStringList(IList<string> stringList)
        {
            var newPageCollection = new MapEventPageCollection { MapEventPages = new ObservableCollection<MapEventPage>(stringList.Select((pageString, pageNumber) => { return new MapEventPage { PageNumber = pageNumber + 1, StringValue = pageString }; })) };
            newPageCollection.AddChildErrors(null, newPageCollection.MapEventPages);

            return newPageCollection;
        }

		protected override string GetStringValue()
		{
			return string.Join("\r\n", this.MapEventPages.Select(rm => rm.StringValue));
		}

		protected override void SetStringValue(string value)
		{
            throw new InvalidOperationException("List of event pages must be parsed as part of MapObject.");
		}

		private void OnEventsCollectionChanged(object sender, EventArgs args)
        {
            if (!this.MapEventPages.Contains(this.SelectedEventPage))
            {
                this.SelectedEventPage = this.lastSelectedIndex < this.MapEventPages.Count && this.lastSelectedIndex >= 0 ? this.MapEventPages[this.lastSelectedIndex] : this.MapEventPages.LastOrDefault();
            }
            this.OnPropertyChanged(nameof(this.MapEventPages));

            for (int i = 0; i < this.MapEventPages.Count; i++)
            {
                var page = this.MapEventPages[i];
                page.UpdatePageNumberAction = null;
                page.PageNumber = i + 1;
            }
            for (int i = 0; i < this.MapEventPages.Count; i++)
            {
                var page = this.MapEventPages[i];
                page.UpdatePageNumberAction = (oldIndex, newIndex) =>
                {
                    if (newIndex < 0 || newIndex >= this.MapEventPages.Count)
                    {
                        return false;
                    }

                    this.MapEventPages.Move(oldIndex, newIndex);
                    return true;
                };
            }
        }

        private void OnSelectedObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"SelectedEventPage.{e.PropertyName}");
        }
    }
}
