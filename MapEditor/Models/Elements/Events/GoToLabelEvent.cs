namespace MapEditor.Models.Elements.Events
{
    public class GoToLabelEvent : EventBase
    {
        private string label;

        public string Label
        {
            get
            {
                return this.label;
            }

            set
            {
                this.SetValue(ref this.label, value);
            }
        }

        public override string Info => "Causes execution to jump to the given label.";

        public override string Name => $"GOTO: \"{this.Label}\"";

        protected override string GetStringValue()
        {
            return $"goto:{this.Label}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed goto event \"{value}\".", e => e.Length == 2 && e[0] == "goto"))
            {
                return;
            }

            var newLabel = entries[1];

            this.Label = newLabel;
        }
    }
}
