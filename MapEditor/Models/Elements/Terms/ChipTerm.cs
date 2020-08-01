using MapEditor.Core;

namespace MapEditor.Models.Elements.Terms
{
    public class ChipTerm : TermBase
    {
        private int chipID;
        private int chipCodeNumber;

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
                return $"Have chip: \"{chipDefinition.Name} {chipCode}\"";
            }
        }

        protected override string GetStringValue()
        {
            return $"havechip/{this.Chip.ID}/{this.Chip.CodeNumber + 1}";
        }

        protected override void SetStringValue(string value)
        {
            var newChip = default(Chip);
            var chipParams = value.Split('/');
            if (this.Validate(chipParams, $"Malformed chip term \"{value}\".", cp => cp.Length == 3))
            {
                newChip = new Chip { ID = this.ParseIntOrAddError(chipParams[1]), CodeNumber = this.ParseIntOrAddError(chipParams[2]) - 1 };
            }

            this.Chip = newChip;
        }
    }
}
