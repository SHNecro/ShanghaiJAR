using System;

namespace MapEditor.Models.Elements.Events
{
    public class MoneyEvent : EventBase
    {
        private int zennyChange;

        public int ZennyChange
        {
            get { return this.zennyChange; }
            set { this.SetValue(ref this.zennyChange, value); }
        }

        public override string Info => "Adds or removes zenny.";

        public override string Name
        {
            get
            {
                var absHPChange = Math.Abs(this.ZennyChange);
                var isAddingString = this.ZennyChange >= 0 ? "+=" : "-=";
                return $"Zenny {isAddingString} {absHPChange}";
            }
        }

        protected override string GetStringValue()
        {
            var absHPChange = Math.Abs(this.ZennyChange);
            var isAddingString = this.ZennyChange >= 0 ? "True" : "False";
            return $"money:{absHPChange}:{isAddingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed money change event \"{value}\".", e => e.Length == 3 && e[0] == "money"))
            {
                return;
            }

            var newZennyChange = this.ParseIntOrAddError(entries[1]);
            var newIsAdding = this.ParseBoolOrAddError(entries[2]);

            this.ZennyChange = newIsAdding ? newZennyChange : -newZennyChange;
        }
    }
}
