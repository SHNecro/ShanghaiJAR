using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models
{
    public class RandomMysteryCollection : StringRepresentation
	{
		private ObservableCollection<RandomMystery> randomMysteryData;
        private int lastSelectedIndex;
		private RandomMystery selectedMystery;

		public RandomMysteryCollection()
		{
			this.RandomMysteryData = new ObservableCollection<RandomMystery>();
		}

		public ObservableCollection<RandomMystery> RandomMysteryData
		{
			get
			{
				return this.randomMysteryData;
			}
			set
			{
				if (this.RandomMysteryData != null)
				{
					this.RandomMysteryData.CollectionChanged -= this.OnRandomMysteryDataCollectionChanged;
				}

				this.SetValue(ref this.randomMysteryData, value);
				this.RandomMysteryData.CollectionChanged += this.OnRandomMysteryDataCollectionChanged;
				this.SelectedMystery = this.RandomMysteryData.FirstOrDefault();
			}
		}

		public RandomMystery SelectedMystery
		{
			get
			{
				return this.selectedMystery;
			}
			set
			{
				if (value != null || this.RandomMysteryData.Count == 0)
				{
                    if (this.selectedMystery != null)
                    {
                        this.selectedMystery.PropertyChanged -= this.OnSelectedMysteryPropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanged += this.OnSelectedMysteryPropertyChanged;
                    }

					this.SetValue(ref this.selectedMystery, value);
                    this.lastSelectedIndex = this.RandomMysteryData.IndexOf(this.SelectedMystery);
				}
			}
		}

		public RandomMystery this[int i] => this.RandomMysteryData[i];

		protected override string GetStringValue()
        {
			if (this.RandomMysteryData.Count == 0)
			{
				return string.Empty;
			}
			return "random:" + string.Join(":", this.RandomMysteryData.Select(rm => rm.StringValue));
        }

        protected override void SetStringValue(string value)
        {
			var newRandomMysteryData = value.Split(':').Skip(1).Select(rms => new RandomMystery { StringValue = rms }).ToList();

            this.RandomMysteryData = new ObservableCollection<RandomMystery>(newRandomMysteryData);
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            if (this.RandomMysteryData == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }
            else
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>(this.RandomMysteryData.SelectMany(sr => this.UpdateChildErrorStack(sr)));
            }
        }

        private void OnRandomMysteryDataCollectionChanged(object sender, EventArgs args)
		{
            if (!this.RandomMysteryData.Contains(this.SelectedMystery))
            {
                this.SelectedMystery = this.lastSelectedIndex < this.RandomMysteryData.Count && this.lastSelectedIndex >= 0 ? this.RandomMysteryData[this.lastSelectedIndex] : this.RandomMysteryData.LastOrDefault();
            }
            this.OnPropertyChanged(nameof(this.RandomMysteryData));
        }

        private void OnSelectedMysteryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"SelectedMystery.{e.PropertyName}");
        }
    }
}
