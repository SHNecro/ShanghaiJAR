using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public class ChipGetEvent : EventBase
    {
        private int chipID;
        private int chipCodeNumber;
        private bool isAdding;

        public Chip Chip
        {
            get
            {
                return new Chip { ID = this.chipID, CodeNumber = this.chipCodeNumber };
            }

            set
            {
                this.OnPropertyChanged(nameof(this.Chip));
                this.ChipID = value.ID;
                this.ChipCodeNumber = value.CodeNumber ?? 0;
                this.OnPropertyChanged(nameof(this.ChipCodeNumber));
            }
        }

        public int ChipID
        {
            get
            {
                return this.chipID;
            }

            set
            {
                this.SetValue(ref this.chipID, value);
                this.OnPropertyChanged(nameof(this.Chip));
                this.OnPropertyChanged(nameof(this.ChipCodeNumber));
            }
        }

        public int ChipCodeNumber
        {
            get
            {
                return this.chipCodeNumber;
            }

            set
            {
                if (value != -1 && value < 4)
                {
                    this.SetValue(ref this.chipCodeNumber, value);
                }
                this.OnPropertyChanged(nameof(this.Chip));
                this.OnPropertyChanged(nameof(this.ChipCodeNumber));
            }
        }

        public bool IsAdding
        {
            get { return this.isAdding; }
            set { this.SetValue(ref this.isAdding, value); }
        }

        public override string Info => "Adds or removes a chip from the folder.";

        public override string Name
        {
            get
            {
                var chipDefinition = Constants.ChipDefinitions[this.ChipID];

                if (this.ChipCodeNumber == -1 || this.ChipCodeNumber > 4)
                {
                    return "N/A";
                }

                var chipCode = chipDefinition.Codes[this.ChipCodeNumber].ToString().Replace("asterisk", "＊");
                var addRemoveString = this.IsAdding ? "Add" : "Remove";
                return $"{addRemoveString} \"{chipDefinition.Name} {chipCode}\"";
            }
        }

        protected override string GetStringValue()
        {
            var addRemoveString = this.IsAdding ? "True" : "False";
            return $"chipGet:{this.ChipID}:{this.ChipCodeNumber}:{addRemoveString}";
        }

        protected override void SetStringValue(string value)
        {
            var newChip = default(Chip);
            var chipParams = value.Split(':');
            if (this.Validate(chipParams, $"Malformed chip add/remove event \"{value}\".", cp => cp.Length == 4))
            {
                newChip = new Chip { ID = this.ParseIntOrAddError(chipParams[1]), CodeNumber = this.ParseIntOrAddError(chipParams[2]) };
            }

            var newIsAdding = this.ParseBoolOrAddError(chipParams[3]);

            this.Chip = newChip;
            this.IsAdding = newIsAdding;
        }
    }
}
