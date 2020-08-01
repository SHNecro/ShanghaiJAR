namespace MapEditor.Models.Elements.Events
{
    public class EditItemEvent : EventBase
    {
        private int itemNumber;
        private bool isAdding;

        public EditItemEvent()
        {
            // TODO: MEMORY LEAK
            // However, would require propagating Dispose() all the way down Map, which would have been nice to do at the very start
            if (Constants.KeyItemDefinitions != null)
            {
                Constants.KeyItemDefinitions.CollectionChanged += (sender, args) => { this.OnPropertyChanged(nameof(this.ItemNumber)); };
            }
        }

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
                var keyItemString = Constants.KeyItemDefinitions.ContainsKey(this.ItemNumber) ? Constants.KeyItemDefinitions[this.ItemNumber].Name : "INVALID";
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

            this.ItemNumber = newItemNumber;
            this.IsAdding = newIsAdding;
        }
    }
}
