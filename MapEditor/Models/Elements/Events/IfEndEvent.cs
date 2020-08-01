namespace MapEditor.Models.Elements.Events
{
    public class IfEndEvent : EventBase
    {
        private int statementID;

        public int StatementID
        {
            get { return this.statementID; }
            set { this.SetValue(ref this.statementID, value); }
        }

        public override string Info => "End the if statement with the given ID.";

        public override string Name => $"End If {this.StatementID}";

        protected override string GetStringValue()
        {
            return $"ifEnd:{this.StatementID}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed if end event \"{value}\".", e => e.Length == 2 && e[0] == "ifEnd"))
            {
                return;
            }

            var newStatementID = this.ParseIntOrAddError(entries[1]);

            this.StatementID = newStatementID;
        }
    }
}
