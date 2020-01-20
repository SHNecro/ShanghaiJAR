using MapEditor.Core;
using MapEditor.Models.Elements;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models
{
    public class TermCollection : StringRepresentation
	{
		private ObservableCollection<TermObject> terms;
        private int lastSelectedIndex;
		private TermObject selectedTerm;

		public TermCollection()
		{
			this.Terms = new ObservableCollection<TermObject>();
		}

		public ObservableCollection<TermObject> Terms
		{
			get
			{
				return this.terms;
			}
			set
			{
				if (this.Terms != null)
				{
					this.Terms.CollectionChanged -= this.OnTermsCollectionChanged;
				}

				this.SetValue(ref this.terms, value);

				this.Terms.CollectionChanged += this.OnTermsCollectionChanged;

				this.SelectedTerm = this.Terms.FirstOrDefault();
			}
		}

		public TermObject SelectedTerm
		{
			get
			{
				return this.selectedTerm;
			}
			set
			{
				if (value != null || this.Terms.Count == 0)
                {
                    if (this.selectedTerm != null)
                    {
                        this.selectedTerm.PropertyChanged -= this.OnSelectedTermPropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanged += this.OnSelectedTermPropertyChanged;
                    }
                    this.SetValue(ref this.selectedTerm, value);
                    this.lastSelectedIndex = this.Terms.IndexOf(this.SelectedTerm);
                }
			}
		}

		public TermObject this[int i] => this.Terms[i];

		protected override string GetStringValue()
		{
			return string.Join(",", this.Terms.Select(rm => rm.StringValue));
		}

		protected override void SetStringValue(string value)
		{
			var newTerms = value.Split(',').Where(ms => !string.IsNullOrEmpty(ms)).Select(ms => TermObject.FromString(ms)).ToList();
			this.AddChildErrors(null, newTerms);

			if (!this.HasErrors)
			{
				this.Terms = new ObservableCollection<TermObject>(newTerms);
			}
		}

		private void OnTermsCollectionChanged(object sender, EventArgs args)
		{
			if (!this.Terms.Contains(this.SelectedTerm))
			{
				this.SelectedTerm = this.lastSelectedIndex < this.Terms.Count && this.lastSelectedIndex >= 0 ? this.Terms[this.lastSelectedIndex] : this.Terms.LastOrDefault();
			}
			this.OnPropertyChanged(nameof(this.Terms));
        }

        private void OnSelectedTermPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"SelectedTerm.{e.PropertyName}");
        }
    }
}
