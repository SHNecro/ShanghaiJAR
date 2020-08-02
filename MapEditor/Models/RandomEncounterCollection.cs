using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models
{
    public class RandomEncounterCollection : StringRepresentation
	{
        private ObservableCollection<RandomEncounter> randomEncounters;
        private int lastSelectedIndex;
        private RandomEncounter selectedEncounter;

		public RandomEncounterCollection()
		{
			this.RandomEncounters = new ObservableCollection<RandomEncounter>();
		}

		public ObservableCollection<RandomEncounter> RandomEncounters
        {
            get
            {
                return this.randomEncounters;
            }
            set
			{
				if (this.RandomEncounters != null)
				{
					this.RandomEncounters.CollectionChanged -= this.OnRandomEncountersCollectionChanged;
				}

				this.SetValue(ref this.randomEncounters, value);

				this.RandomEncounters.CollectionChanged += this.OnRandomEncountersCollectionChanged;

                this.SelectedEncounter = this.RandomEncounters.FirstOrDefault();
            }
        }

        public RandomEncounter SelectedEncounter
        {
            get
            {
                return this.selectedEncounter;
            }
            set
            {
                if (value != null || this.RandomEncounters.Count == 0)
                {
                    if (this.selectedEncounter != null)
                    {
                        this.selectedEncounter.PropertyChanged -= this.OnSelectedEncounterPropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanged += this.OnSelectedEncounterPropertyChanged;
                    }

                    this.SetValue(ref this.selectedEncounter, value);
                    this.lastSelectedIndex = this.RandomEncounters.IndexOf(this.SelectedEncounter);
                }
            }
        }

        public RandomEncounter this[int i] => this.RandomEncounters[i];

		protected override string GetStringValue()
        {
			return string.Join("\r\n", this.RandomEncounters.Select(re => re.StringValue));
        }

        protected override void SetStringValue(string value)
        {
			var newRandomEncounters = value.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(res => res != string.Empty).Select(res => new RandomEncounter { StringValue = res }).ToList();

            this.RandomEncounters = new ObservableCollection<RandomEncounter>(newRandomEncounters);
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            if (this.RandomEncounters == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }
            else
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>(this.RandomEncounters.SelectMany(sr => this.UpdateChildErrorStack(sr)));
            }
        }

        private void OnRandomEncountersCollectionChanged(object sender, EventArgs args)
		{
            if (!this.RandomEncounters.Contains(this.SelectedEncounter))
            {
                this.SelectedEncounter = this.lastSelectedIndex < this.RandomEncounters.Count && this.lastSelectedIndex >= 0 ? this.RandomEncounters[this.lastSelectedIndex] : this.RandomEncounters.LastOrDefault();
            }
            this.OnPropertyChanged(nameof(this.RandomEncounters));
        }

        private void OnSelectedEncounterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"SelectedEncounter.{e.PropertyName}");
        }
    }
}
