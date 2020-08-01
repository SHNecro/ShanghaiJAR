using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MapEditor.Core;
using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models
{
    public class ShopItem : StringRepresentation
    {
        private static readonly Dictionary<int, int> SubChipPrices = new Dictionary<int, int>{
            { 0, 500 },
            { 1, 1000 },
            { 2, 500 },
            { 3, 50 },
            { 4, 100 },
            { 5, 10000 },
            { 6, 5000 },
        };

        private int id;
        private int data;
        private int price;
        private int stock;

        private int shopType;
        private int priceType;

        public int ID
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;

                if (this.ShopType == (int)ShopTypeNumber.Chips)
                {
                    if (value != 0)
                    {
                        this.Data = 0;
                    }
                    this.OnPropertyChanged(nameof(this.Chip));
                }

                this.OnPropertyChanged(nameof(this.ID));
            }
        }

        public int Data
        {
            get
            {
                return this.data;
            }

            set
            {
                this.SetValue(ref this.data, value);
            }
        }

        public int Price
        {
            get { return this.price; }
            set { this.SetValue(ref this.price, value); }
        }

        public int Stock
        {
            get { return this.stock; }
            set { this.SetValue(ref this.stock, value); }
        }

        public int ShopType
        {
            get { return this.shopType; }
            set { this.SetValue(ref this.shopType, value); }
        }

        public int PriceType
        {
            get { return this.priceType; }
            set { this.SetValue(ref this.priceType, value); }
        }

        public Chip Chip
        {
            get
            {
                if (this.ShopType != (int)ShopTypeNumber.Chips)
                {
                    return null;
                }

                return new Chip { ID = this.ID, CodeNumber = this.Data };
            }

            set
            {
                this.ID = value.ID;
                this.Data = 0;
            }
        }

        public string Name
        {
            get
            {
                var priceTypeString = (new EnumDescriptionTypeConverter(typeof(ShopPriceTypeNumber))).ConvertToString((ShopPriceTypeNumber)this.PriceType);
                var stockString = this.Stock == 0 ? string.Empty : $" (x{this.Stock})";
                if (this.ShopType == (int)ShopTypeNumber.Chips)
                {
                    if (this.ID == 0)
                    {
                        return $"HPMemory{stockString}: {this.Price} (+ {this.Data}) {priceTypeString}";
                    }

                    if (!Constants.ChipDefinitions.ContainsKey(this.ID) || this.Data >= Constants.ChipDefinitions[this.ID].Codes.Length)
                    {
                        return "ERROR";
                    }

                    var chipDefinition = Constants.ChipDefinitions[this.ID];
                    var chipCode = chipDefinition.Codes[this.Data].ToString().Replace("asterisk", "＊");

                    return $"{chipDefinition.Name} {chipCode}{stockString}: {this.Price} {priceTypeString}";
                }
                if (this.ShopType == (int)ShopTypeNumber.SubChips)
                {
                    var subChipString = (new EnumDescriptionTypeConverter(typeof(SubChipTypeNumber))).ConvertToString((SubChipTypeNumber)this.ID);
                    var discountString = this.Data != 0 ? $" - {this.Data}" : string.Empty;

                    if (!ShopItem.SubChipPrices.ContainsKey(this.ID))
                    {
                        return "ERROR";
                    }

                    var priceString = ShopItem.SubChipPrices[this.ID];
                    return $"{subChipString}{stockString}: {priceString}{discountString} {priceTypeString}";
                }
                if (this.ShopType == (int)ShopTypeNumber.AddOns)
                {

                    if (!Constants.AddOnDefinitions.ContainsKey(this.ID))
                    {
                        return "ERROR";
                    }

                    var addOnNameString = Constants.AddOnDefinitions[this.ID].Name;
                    var addOnColor = this.ParseEnumOrAddError<ProgramColorTypeNumber>(this.Data.ToString()).ToString();

                    return $"{addOnNameString} {addOnColor}{stockString}: {this.Price} {priceTypeString}";
                }
                if (this.ShopType == (int)ShopTypeNumber.Interiors)
                {

                    if (!Constants.InteriorDefinitions.ContainsKey(this.ID))
                    {
                        return "ERROR";
                    }

                    var interiorNameString = Constants.InteriorDefinitions[this.ID];

                    return $"{interiorNameString}{stockString}: {this.Price} {priceTypeString}";
                }

                return this.StringValue;
            }
        }

        protected override string GetStringValue()
        {
            return $"{this.ID},{this.Data},{this.Price},{this.Stock}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(',');
            if (!this.Validate(entries, $"Malformed shopitem \"{value}\".", e => e.Length >= 4))
            {
                return;
            }

            var newID = this.ParseIntOrAddError(entries[0]);
            var newData = this.ParseIntOrAddError(entries[1]);
            var newPrice = this.ParseIntOrAddError(entries[2]);
            var newStock = this.ParseIntOrAddError(entries[3]);

            this.ID = newID;
            this.Data = newData;
            this.Price = newPrice;
            this.Stock = newStock;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            base.OnPropertyChanged(nameof(this.Name));
        }
    }
}
