using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models.Elements.Events
{
    public class ItemGetEvent : EventBase
    {
        private RandomMystery mystery;

        public RandomMystery Mystery
        {
            get
            {
                return this.mystery;
            }

            set
            {
                if (this.Mystery != null)
                {
                    this.Mystery.PropertyChanged -= this.MysteryPropertyChanged;
                }
                this.SetValue(ref this.mystery, value);
                this.Mystery.PropertyChanged += this.MysteryPropertyChanged;
            }
        }

        public override string Info => "Gives an item as if a MysteryData were opened.";

        public override string Name => $"Get Item: {this.Mystery.Name}";

        public ItemGetEvent()
        {
            this.Mystery = Constants.BlankMysteryCreator();
        }

        protected override string GetStringValue()
        {
            var mysteryString = string.Join(":", this.Mystery.StringValue.Split(','));
            return $"ItemGet:{mysteryString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed item get event \"{value}\".", e => e.Length == 5 && e[0] == "ItemGet"))
            {
                return;
            }

            var newMystery = new RandomMystery { StringValue = string.Join(",", entries.Skip(1)) };

            this.Mystery = newMystery;
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            return this.UpdateChildErrorStack(Mystery);
        }

        private void MysteryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Mystery));
        }
    }
}
