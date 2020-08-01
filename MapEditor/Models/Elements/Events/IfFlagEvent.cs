namespace MapEditor.Models.Elements.Events
{
    public class IfFlagEvent : EventBase
    {
        private int flagNumber;
        private bool isTrue;
        private int statementID;

        public int FlagNumber
        {
            get { return this.flagNumber; }
            set { this.SetValue(ref this.flagNumber, value); }
        }

        public bool IsTrue
        {
            get { return this.isTrue; }
            set { this.SetValue(ref this.isTrue, value); }
        }

        public int StatementID
        {
            get { return this.statementID; }
            set { this.SetValue(ref this.statementID, value); }
        }

        public override string Info => "Executes if the specified flag is of a given value.";

        public override string Name
        {
            get
            {
                var isTrueString = this.IsTrue ? string.Empty : "NOT ";
                return $"If {this.StatementID}: {isTrueString}Flag {this.FlagNumber}";
            }
        }

        protected override string GetStringValue()
        {
            var isTrueString = this.IsTrue ? "True" : "False";
            return $"ifFlag:{this.FlagNumber}:{isTrueString}:{this.StatementID}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed if flag event \"{value}\".", e => e.Length == 4 && e[0] == "ifFlag"))
            {
                return;
            }

            var newFlagNumber = this.ParseIntOrAddError(entries[1]);
            var newIsTrue = this.ParseBoolOrAddError(entries[2]);
            var newStatementID = this.ParseIntOrAddError(entries[3]);

            this.FlagNumber = newFlagNumber;
            this.IsTrue = newIsTrue;
            this.StatementID = newStatementID;
        }
    }
}
