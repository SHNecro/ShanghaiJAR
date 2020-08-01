using System;

namespace MapEditor.Models.Elements.Events
{
    public class HPChangeEvent : EventBase
    {
        private int hpChange;

        public int HPChange
        {
            get { return this.hpChange; }
            set { this.SetValue(ref this.hpChange, value); }
        }

        public override string Info => "Adds or removes HP.";

        public override string Name
        {
            get
            {
                var absHPChange = Math.Abs(this.HPChange);
                var isAddingString = this.HPChange >= 0 ? "+=" : "-=";
                return $"HP {isAddingString} {absHPChange}";
            }
        }

        protected override string GetStringValue()
        {
            var absHPChange = Math.Abs(this.HPChange);
            var isAddingString = this.HPChange >= 0 ? "True" : "False";
            return $"HP:{absHPChange}:{isAddingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed HP change event \"{value}\".", e => e.Length == 3 && e[0] == "HP"))
            {
                return;
            }

            var newHPChange = this.ParseIntOrAddError(entries[1]);
            var newIsAdding = this.ParseBoolOrAddError(entries[2]);

            this.HPChange = newIsAdding ? newHPChange : -newHPChange;
        }
    }
}
