﻿using MapEditor.Core;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models
{
    public class RandomMystery : StringRepresentation
    {
        private int category;
        private int id;
        private int data;
        private string itemKey;

        public int Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.SetValue(ref this.category, value);
                this.ID = 0;
                this.Data = 0;
            }
        }

        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.SetValue(ref this.id, value);
                if (this.Category == 0)
                {
                    this.OnPropertyChanged(nameof(this.Chip));
                }
                this.OnPropertyChanged(nameof(this.Name));
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
                this.OnPropertyChanged(nameof(this.Data));
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        public string ItemKey
        {
            get
            {
                return this.itemKey;
            }
            set
            {
                this.SetValue(ref this.itemKey, value);
            }
        }

        public string Name
        {
            get
            {
                var newCategory = this.ParseEnumOrAddError<ItemTypeNumber>(this.Category.ToString());
                switch (newCategory)
                {
                    case ItemTypeNumber.Chip:
                        var chipDefinition = Constants.ChipDefinitions[this.ID + 1];

                        if (this.Data == -1 || this.Data > 4)
                        {
                            return "N/A";
                        }

                        var chipCode = chipDefinition.Codes[this.Data].ToString().Replace("asterisk", "＊");
                        return $"{chipDefinition.Name} {chipCode}";
                    case ItemTypeNumber.SubChip:
                        return this.ParseEnumOrAddError<SubChipTypeNumber>(this.ID.ToString()).ToString();
                    case ItemTypeNumber.AddOn:
                        var addOnDefinition = Constants.AddOnDefinitions[this.ID];
                        var addOnColor = this.ParseEnumOrAddError<ProgramColorTypeNumber>(this.Data.ToString()).ToString();
                        return $"{addOnDefinition.Name} {addOnColor}";
                    case ItemTypeNumber.Other:
                        var otherType = this.ParseEnumOrAddError<OtherItemTypeNumber>(this.ID.ToString()).ToString();
                        return (this.ID == 0 || this.ID == 2 || this.ID == 3) ? $"{otherType}" : $"{this.Data} {otherType}";
                    case ItemTypeNumber.Virus:
                        return "Encounter";
                    default:
                        return "N/A";
                }
            }
        }

        public Chip Chip
        {
            get
            {
                if (this.Category != 0)
                {
                    return null;
                }

                return new Chip { ID = this.ID + 1, CodeNumber = this.Data };
            }

            set
            {
                this.ID = value.ID - 1;
                this.Data = 0;
            }
        }

        protected override string GetStringValue()
        {
            return string.Join(",", new object[]
            {
                this.Category,
                this.ID,
                this.Data,
                this.Category != 4 ? this.ItemKey : Constants.EncounterKey
            });
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(',');
            if (!this.Validate(entries, "Invalid number of parameters.", e => e.Length == 4))
            {
                return;
            }

            var newCategory = this.ParseIntOrAddError(entries[0]);
            this.Validate(newCategory, "Invalid category (0: Chip, 1: SubChip, 2: AddOn, 3: Other, 4: Virus)", c => c >= 0 && c <= 4);

            var newId = this.ParseIntOrAddError(entries[1]);
            if (newId == 1)
            {
                this.Validate(newId, "Invalid SubChip ID (0-6)", c => c >= 0 && c <= 3);
            }

            var newData = this.ParseIntOrAddError(entries[2]);

            var newItemKey = entries[3];
            //this.Validate(itemKey, "Item key does not exist.", k => Constants.TranslationService.CanTranslate(k));

            if (!this.HasErrors)
            {
                this.Category = newCategory;
                this.ID = newId;
                this.Data = newData;
                this.ItemKey = newItemKey;
            }
        }
    }
}
