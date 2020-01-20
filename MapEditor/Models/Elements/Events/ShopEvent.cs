using Common;
using ExtensionMethods;
using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;
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
                var faceString = this.Face.ToString();
                var priceTypeString = (new EnumDescriptionTypeConverter(typeof(ShopPriceTypeNumber))).ConvertToString((ShopPriceTypeNumber)this.PriceTypeNumber);
                return $"Shop: {faceString}: {shopTypeString} ({priceTypeString})";
            }
        }

        protected override string GetStringValue()
        {
            var shopItemsString = this.ShopItems.StringValue;
            return $"shop:{this.shopStockIndex}:{this.ShopTypeNumber}:{this.ShopClerkTypeNumber}:{this.FaceSheet}:{this.FaceIndex}:{this.PriceTypeNumber}:{shopItemsString}";
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

            var newFaceSheet = this.ParseIntOrAddError(entries[4]);
            var newFaceIndex = this.ParseIntOrAddError(entries[5]);
            var newFace = new FaceId(newFaceSheet, (byte)newFaceIndex, false).ToFace();
            this.ParseEnumOrAddError<FACE>(((int)newFace).ToString());

            var newPriceTypeNumber = this.ParseIntOrAddError(entries[6]);
            this.ParseEnumOrAddError<ShopPriceTypeNumber>(entries[6]);

            var newShopItems = new ShopItemCollection { StringValue = string.Join(":", entries.Skip(7)), ShopType = newShopTypeNumber };
            this.AddChildErrors(null, new[] { newShopItems });

            if (!this.HasErrors)
            {
                this.ShopStockIndex = newShopStockIndex;
                this.ShopTypeNumber = newShopTypeNumber;
                this.ShopClerkTypeNumber = newClerkType;
                this.Face = newFace;
                this.PriceTypeNumber = newPriceTypeNumber;
                this.ShopItems = newShopItems;
            }
        }

        private void OnShopItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged($"ShopItems.{e.PropertyName}");
        }
    }
}
