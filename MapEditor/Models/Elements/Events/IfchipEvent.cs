using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public class IfChipEvent : EventBase
    {
        private int chipID;
        private int chipCodeNumber;
        private bool isPresent;
        private int statementID;

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

        public bool IsPresent
        {
            get { return this.isPresent; }
            set { this.SetValue(ref this.isPresent, value); }
        }

        public int StatementID
        {
            get { return this.statementID; }
            set { this.SetValue(ref this.statementID, value); }
        }

        public override string Info => "Executes if the player has or does not have the specified chip.";

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
                var isPresentString = this.IsPresent ? "Have" : "Does not have";
                return $"If {this.StatementID}: {isPresentString} \"{chipDefinition.Name} {chipCode}\"";
            }
        }

        protected override string GetStringValue()
        {
            var isPresentString = this.IsPresent ? "True" : "False";
            return $"ifchip:{this.ChipID}:{this.ChipCodeNumber}:{isPresentString}:{this.StatementID}";
        }

        protected override void SetStringValue(string value)
        {
            var newChip = default(Chip);
            var chipParams = value.Split(':');
            if (this.Validate(chipParams, $"Malformed if chip event \"{value}\".", cp => cp.Length == 5))
            {
                newChip = new Chip { ID = this.ParseIntOrAddError(chipParams[1]), CodeNumber = this.ParseIntOrAddError(chipParams[2]) };
            }

            var newIsPresent = this.ParseBoolOrAddError(chipParams[3]);
            var newStatementID = this.ParseIntOrAddError(chipParams[4]);

            this.Chip = newChip;
            this.IsPresent = newIsPresent;
            this.StatementID = newStatementID;
        }
    }
}
