using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models
{
    public class EventCollection : StringRepresentation
	{
		private ObservableCollection<EventObject> events;
        private int lastSelectedIndex;
        private EventObject selectedEvent;

		public EventCollection()
		{
			this.Events = new ObservableCollection<EventObject>();
		}

		public ObservableCollection<EventObject> Events
		{
			get
			{
				return this.events;
			}
			set
			{
				if (this.Events != null)
				{
					this.Events.CollectionChanged -= this.OnEventsCollectionChanged;
				}

				this.SetValue(ref this.events, value);

				this.Events.CollectionChanged += this.OnEventsCollectionChanged;

				this.SelectedEvent = this.Events.FirstOrDefault();
			}
		}

		public EventObject SelectedEvent
		{
			get
			{
				return this.selectedEvent;
			}
			set
			{
				if (value != null || this.Events.Count == 0)
                {
                    if (this.selectedEvent != null)
                    {
                        this.selectedEvent.PropertyChanged -= this.OnSelectedEventPropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanged += this.OnSelectedEventPropertyChanged;
                    }
                    this.SetValue(ref this.selectedEvent, value);
                    this.lastSelectedIndex = this.Events.IndexOf(this.SelectedEvent);
				}
			}
		}

        public EventObject this[int i] => this.Events[i];

		protected override string GetStringValue()
		{
			return string.Join("\r\n", this.Events.Select(rm => rm.StringValue));
		}

		protected override void SetStringValue(string value)
		{
			var newEvents = value.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Where(ms => !string.IsNullOrEmpty(ms)).Select(ms => EventObject.FromString(ms)).ToList();

            this.Events = new ObservableCollection<EventObject>(newEvents);
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            if (this.Events == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }
            else
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>(this.Events.SelectMany(sr => this.UpdateChildErrorStack(sr)));
            }
        }

        private void OnEventsCollectionChanged(object sender, EventArgs args)
		{
			if (!this.Events.Contains(this.SelectedEvent))
            {
                this.SelectedEvent = this.lastSelectedIndex < this.Events.Count && this.lastSelectedIndex >= 0 ? this.Events[this.lastSelectedIndex] : this.Events.LastOrDefault();
            }
			this.OnPropertyChanged(nameof(this.Events));
        }

        private void OnSelectedEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"SelectedEvent.{e.PropertyName}");
        }
    }
}
