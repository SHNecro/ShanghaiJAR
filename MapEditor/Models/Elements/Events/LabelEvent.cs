namespace MapEditor.Models.Elements.Events
{
    public class LabelEvent : EventBase
    {
        private string label;

        public string Label
        {
            get { return this.label; }
            set { this.SetValue(ref this.label, value); }
        }

        public override string Info => "Places a label to be used as a target of a GoTo event.";

        public override string Name => $"Label: \"{this.Label}\"";

        protected override string GetStringValue()
        {
            return $"lavel:{this.Label}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed label event \"{value}\".", e => e.Length == 2 && e[0] == "lavel"))
            {
                return;
            }

            var newLabel = entries[1];

            this.Label = newLabel;
        }
    }
}
