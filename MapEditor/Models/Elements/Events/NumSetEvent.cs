using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public class NumSetEvent : EventBase, ITranslatedModel
    {
        private string numSetKey;
        private int targetVariable;
        private int numberOfDigits;

        public string NumSetKey
        {
            get { return this.numSetKey; }
            set { this.SetValue(ref this.numSetKey, value); }
        }

        public int TargetVariable
        {
            get { return this.targetVariable; }
            set { this.SetValue(ref this.targetVariable, value); }
        }

        public int NumberOfDigits
        {
            get { return this.numberOfDigits; }
            set { this.SetValue(ref this.numberOfDigits, value); }
        }

        public override string Info => "Number input, stored in the specified variable (-1 if cancelled).";

        public override string Name
        {
            get
            {
                var dialogue = Constants.TranslationService.Translate(this.NumSetKey);
                var digitsString = new string('#', this.NumberOfDigits);
                return $"Num: {dialogue.Face.ToString()}: {dialogue.Text}: var[{this.TargetVariable}] = {digitsString}";
            }
        }

        public void RefreshTranslation()
        {
            this.OnPropertyChanged(nameof(this.Name));
        }

        protected override string GetStringValue()
        {
            return $"numset:{this.NumSetKey}:{this.TargetVariable}:{this.NumberOfDigits}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed numset event \"{value}\".", e => e.Length == 4 && e[0] == "numset"))
            {
                return;
            }

            var newMessageKey = entries[1];
            this.Validate(newMessageKey, () => this.NumSetKey, s=> $"NumSet message key \"{s}\" does not exist.", Constants.TranslationService.CanTranslate);

            var newTargetVariable = this.ParseIntOrAddError(entries[2], () => this.TargetVariable, tv => tv >= 0, tv => $"Invalid target variable {tv} (>= 0)");
            var newNumberOfDigits = this.ParseIntOrAddError(entries[3], () => this.NumberOfDigits, nod => nod >= 0, nod => $"Invalid number of digits {nod} (>= 0)");

            this.NumSetKey = newMessageKey;
            this.TargetVariable = newTargetVariable;
            this.NumberOfDigits = newNumberOfDigits;
        }
    }
}
