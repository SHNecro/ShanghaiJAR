namespace MapEditor.Models.Elements.Events
{
    public class BranchHeadEvent : EventBase
    {
        private int branchNumber;

        public int BranchNumber
        {
            get
            {
                return this.branchNumber;
            }

            set
            {
                this.SetValue(ref this.branchNumber, value);
            }
        }

        public override string Info => "Marks start of a section that executes only when the branch number matches the last selected question option. Otherwise, skips to the next BranchEnd.";

        public override string Name => $"Branch: Answered Option {this.BranchNumber}";

        protected override string GetStringValue()
        {
            return $"BranchHead:{this.branchNumber}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed branch head event \"{value}\".", e => e.Length == 2 && e[0] == "BranchHead"))
            {
                return;
            }

            var newBranchNumber = this.ParseIntOrAddError(entries[1]);

            this.BranchNumber = newBranchNumber;
        }
    }
}
