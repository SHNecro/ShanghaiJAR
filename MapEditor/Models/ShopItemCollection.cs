using MapEditor.Core;
using MapEditor.ExtensionMethods;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models
{
	public class ShopItemCollection : StringRepresentation
	{
		private ObservableCollection<ShopItem> shopItem;
        private int lastSelectedIndex;
		private ShopItem selectedShopItem;

        private int shopType;
        private int priceType;

		public ShopItemCollection()
		{
			this.ShopItems = new ObservableCollection<ShopItem>();
		}

		public ObservableCollection<ShopItem> ShopItems
		{
			get
			{
				return this.shopItem;
			}
			set
			{
				if (this.ShopItems != null)
				{
					this.ShopItems.CollectionChanged -= this.OnShopItemsCollectionChanged;
				}

				this.SetValue(ref this.shopItem, value);

				this.ShopItems.CollectionChanged += this.OnShopItemsCollectionChanged;

				this.SelectedShopItem = this.ShopItems.FirstOrDefault();
			}
		}

		public ShopItem SelectedShopItem
		{
			get
			{
				return this.selectedShopItem;
			}
			set
			{
				if (value != null || this.ShopItems.Count == 0)
                {
                    if (this.selectedShopItem != null)
                    {
                        this.selectedShopItem.PropertyChanged -= this.OnSelectedShopItemPropertyChanged;
                    }
                    if (value != null)
                    {
                        value.PropertyChanged += this.OnSelectedShopItemPropertyChanged;
                    }
                    this.SetValue(ref this.selectedShopItem, value);
                    this.lastSelectedIndex = this.ShopItems.IndexOf(this.SelectedShopItem);
				}
			}
        }

        public int ShopType
        {
            get
            {
                return this.shopType;
            }

            set
            {
                this.SetValue(ref this.shopType, value);
                foreach (var si in this.ShopItems)
                {
                    si.ShopType = value;
                }
            }
        }

        public int PriceType
        {
            get
            {
                return this.priceType;
            }

            set
            {
                this.SetValue(ref this.priceType, value);
                foreach (var si in this.ShopItems)
                {
                    si.PriceType = value;
                }
            }
        }

        public ShopItem this[int i] => this.ShopItems[i];

		protected override string GetStringValue()
		{
			return string.Join(":", this.ShopItems.Select(rm => rm.StringValue));
		}

		protected override void SetStringValue(string value)
		{
			var newShopItems = value.Split(':').Where(ms => !string.IsNullOrEmpty(ms)).Select(ms => new ShopItem { StringValue = ms, ShopType = this.ShopType, PriceType = this.PriceType }).ToList();

            this.ShopItems = new ObservableCollection<ShopItem>(newShopItems);
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            if (this.ShopItems == null)
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>();
            }
            else
            {
                return new ObservableCollection<Tuple<StringRepresentation[], string>>(this.ShopItems.SelectMany(sr => this.UpdateChildErrorStack(sr)));
            }
        }

        private void OnShopItemsCollectionChanged(object sender, EventArgs args)
		{
            if (!this.ShopItems.Contains(this.SelectedShopItem))
            {
                this.SelectedShopItem = this.lastSelectedIndex < this.ShopItems.Count && this.lastSelectedIndex >= 0 ? this.ShopItems[this.lastSelectedIndex] : this.ShopItems.LastOrDefault();
            }
            this.OnPropertyChanged(nameof(this.ShopItems));
            foreach (var si in this.ShopItems)
            {
                si.ShopType = this.ShopType;
                si.PriceType = this.PriceType;
            }
        }

        private void OnSelectedShopItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"SelectedShopItem.{e.PropertyName}");
        }
    }
}
