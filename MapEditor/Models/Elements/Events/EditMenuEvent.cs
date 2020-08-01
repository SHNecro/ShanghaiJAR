using MapEditor.Core.Converters;
using MapEditor.Models.Elements.Enums;

namespace MapEditor.Models.Elements.Events
{
    public class EditMenuEvent : EventBase
    {
        private int menuNumber;
        private bool isAdding;

        public int MenuNumber
        {
            get
            {
                return this.menuNumber;
            }

            set
            {
                this.SetValue(ref this.menuNumber, value);
            }
        }

        public bool IsAdding
        {
            get
            {
                return this.isAdding;
            }

            set
            {
                this.SetValue(ref this.isAdding, value);
            }
        }

        public override string Info => "Adds or removes a menu option.";

        public override string Name
        {
            get
            {
                var isAddingString = this.IsAdding ? "Add" : "Remove";
                var keyItemString = (new EnumDescriptionTypeConverter(typeof(MenuItemTypeNumber))).ConvertToString((MenuItemTypeNumber)this.MenuNumber);
                return $"{isAddingString} {keyItemString} Menu Option";
            }
        }

        protected override string GetStringValue()
        {
            var isAddingString = this.IsAdding ? "True" : "False";
            return $"editMenu:{this.MenuNumber}:{isAddingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed edit menu event \"{value}\".", e => e.Length == 3 && e[0] == "editMenu"))
            {
                return;
            }

            var newMenuNumber = this.ParseIntOrAddError(entries[1]);
            var newIsAdding = this.ParseBoolOrAddError(entries[2]);

            this.MenuNumber = newMenuNumber;
            this.IsAdding = newIsAdding;
        }
    }
}
