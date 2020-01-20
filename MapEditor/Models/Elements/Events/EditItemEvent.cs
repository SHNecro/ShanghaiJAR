using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class EditItemEvent : EventBase
    {
        private int itemNumber;
        private bool isAdding;

        public int ItemNumber
        {
            get { return this.itemNumber; }
            set { this.SetValue(ref this.itemNumber, value); }
        }

        public bool IsAdding
        {
            get { return this.isAdding; }
            set { this.SetValue(ref this.isAdding, value); }
        }

        public override string Info => "Adds or removes a key item.";

        public override string Name
        {
            get
            {
                var isAddingString = this.IsAdding ? "Add" : "Remove";
                var keyItemString = (new EnumDescriptionTypeConverter(typeof(KeyItemTypeNumber))).ConvertToString((KeyItemTypeNumber)this.ItemNumber);
                return $"{isAddingString} {keyItemString} Key Item";
            }
        }

        protected override string GetStringValue()
        {
            var isAddingString = this.IsAdding.ToString();
            return $"EditItem:{this.ItemNumber}:{isAddingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed edit item event \"{value}\".", e => e.Length == 3 && e[0] == "EditItem"))
            {
                return;
            }

            var newItemNumber = this.ParseIntOrAddError(entries[1]);
            var newIsAdding = this.ParseBoolOrAddError(entries[2]);

            if (!this.HasErrors)
            {
                this.ItemNumber = newItemNumber;
                this.IsAdding = newIsAdding;
            }
        }
    }
}
