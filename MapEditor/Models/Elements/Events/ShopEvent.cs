using Common;
using ExtensionMethods;
using MapEditor.Core;
using MapEditor.Core.Converters;
using MapEditor.ExtensionMethods;
using MapEditor.Models.Elements.Enums;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MapEditor.Models.Elements.Events
{
    public class ShopEvent : EventBase
    {
        private int shopStockIndex;
        private int shopTypeNumber;
        private int shopClerkTypeNumber;
        private int faceSheet;
        private int faceIndex;
		private bool isMono;
        private bool isAuto;
		private int priceTypeNumber;
        private ShopItemCollection shopItems;

        public int ShopStockIndex
        {
            get { return this.shopStockIndex; }
            set { this.SetValue(ref this.shopStockIndex, value); }
        }

        public int ShopTypeNumber
        {
            get
            {
                return this.shopTypeNumber;
            }

            set
            {
                this.SetValue(ref this.shopTypeNumber, value);
                if (this.ShopItems != null)
                {
                    this.ShopItems.ShopType = value;
                }
            }
        }

        public int ShopClerkTypeNumber
        {
            get { return this.shopClerkTypeNumber; }
            set { this.SetValue(ref this.shopClerkTypeNumber, value); }
        }

        public int FaceSheet
        {
            get { return this.faceSheet; }
            set { this.SetValue(ref this.faceSheet, value, nameof(this.Face)); }
        }

        public int FaceIndex
        {
            get { return this.faceIndex; }
            set { this.SetValue(ref this.faceIndex, value, nameof(this.Face)); }
        }

		public FACE Face
		{
			get
			{
				return new FaceId(this.FaceSheet, (byte)this.FaceIndex, false).ToFace();
			}

			set
			{
				var faceId = value.ToFaceId();
				this.FaceSheet = faceId.Sheet;
                this.FaceIndex = faceId.Index;
            }
        }

        public bool IsManualFace => !Enum.IsDefined(typeof(FACE), ((this.FaceSheet - 1) * 16) + this.FaceIndex);

		public bool IsMono
		{
			get
			{
				return this.isMono;
			}

			set
			{
				this.SetValue(ref this.isMono, value);
			}
		}

		public bool IsAuto
		{
			get
			{
				return this.isAuto;
			}

			set
			{
				this.SetValue(ref this.isAuto, value);
			}
		}

		public int PriceTypeNumber
        {
            get
            {
                return this.priceTypeNumber;
            }

            set
            {
                this.SetValue(ref this.priceTypeNumber, value);
                if (this.ShopItems != null)
                {
                    this.ShopItems.PriceType = value;
                }
            }
        }

        public ShopItemCollection ShopItems
        {
            get
            {
                return this.shopItems;
            }
            set
            {
                if (this.shopItems != null)
                {
                    this.shopItems.PropertyChanged -= this.OnShopItemsPropertyChanged;
                }
                value.PropertyChanged += this.OnShopItemsPropertyChanged;
                this.SetValue(ref this.shopItems, value);
            }
        }

        public override string Info => "Opens a store with a given type, clerk, and items. The index of the shop's stock must be manually specified.";

        public override string Name
        {
            get
            {
                var shopTypeString = (new EnumDescriptionTypeConverter(typeof(ShopTypeNumber))).ConvertToString((ShopTypeNumber)this.ShopTypeNumber);
                var faceString = new FaceId(this.FaceSheet, (byte)this.FaceIndex, false).ToString();
                var priceTypeString = (new EnumDescriptionTypeConverter(typeof(ShopPriceTypeNumber))).ConvertToString((ShopPriceTypeNumber)this.PriceTypeNumber);
                return $"Shop: {faceString}: {shopTypeString} ({priceTypeString})";
            }
        }

        protected override string GetStringValue()
        {
            var entry4 = this.FaceSheet.ToString();
            var entry5 = this.FaceIndex.ToString();
            if (this.IsMono || this.IsAuto)
            {
                entry4 = $"{this.FaceSheet},{this.FaceIndex}";
                entry5 = $"{this.IsMono},{this.IsAuto}";
            }

            var shopItemsString = this.ShopItems.StringValue;
            return $"shop:{this.shopStockIndex}:{this.ShopTypeNumber}:{this.ShopClerkTypeNumber}:{entry4}:{entry5}:{this.PriceTypeNumber}:{shopItemsString}";
        }

		protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed shop event \"{value}\".", e => e.Length >= 7 && e[0] == "shop"))
            {
                return;
            }

            var newShopStockIndex = this.ParseIntOrAddError(entries[1]);

            var newShopTypeNumber = this.ParseIntOrAddError(entries[2]);
            this.ParseEnumOrAddError<ShopTypeNumber>(entries[2]);

            var newClerkType = this.ParseIntOrAddError(entries[3]);
            this.ParseEnumOrAddError<ShopClerkTypeNumber>(entries[3]);

			var faceSheet = 0;
			var faceIndex = (byte)0;
			var mono = false;
			var auto = false;

			string[] faceSections = null, modifierSections = null;
			if (entries[4].Contains(",") && entries[5].Contains(",")
				&& int.TryParse((faceSections = faceSections ?? entries[4].Split(','))[0], out faceSheet)
				&& byte.TryParse((faceSections = faceSections ?? entries[4].Split(','))[1], out faceIndex)
				&& bool.TryParse((modifierSections = modifierSections ?? entries[5].Split(','))[0], out mono)
				&& bool.TryParse((modifierSections = modifierSections ?? entries[5].Split(','))[1], out auto))
			{
				;
			}
			else if (int.TryParse(entries[4], out faceSheet)
				&& byte.TryParse(entries[5], out faceIndex))
			{
				;
			}
			var newFace = new FaceId(faceSheet, (byte)faceIndex).ToFace();
			//this.ParseEnumOrAddError<FACE>(((int)newFace).ToString());

			var newPriceTypeNumber = this.ParseIntOrAddError(entries[6]);
            this.ParseEnumOrAddError<ShopPriceTypeNumber>(entries[6]);

            var newShopItems = new ShopItemCollection { StringValue = string.Join(":", entries.Skip(7)), ShopType = newShopTypeNumber, PriceType = newPriceTypeNumber };

            this.ShopStockIndex = newShopStockIndex;
            this.ShopTypeNumber = newShopTypeNumber;
            this.ShopClerkTypeNumber = newClerkType;
            this.Face = newFace;
            this.IsMono = mono;
            this.IsAuto = auto;
            this.PriceTypeNumber = newPriceTypeNumber;
            this.ShopItems = newShopItems;
        }

        protected override ObservableCollection<Tuple<StringRepresentation[], string>> GetErrors()
        {
            return this.UpdateChildErrorStack(ShopItems);
        }

        private void OnShopItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"ShopItems.{e.PropertyName}");
        }
    }
}
