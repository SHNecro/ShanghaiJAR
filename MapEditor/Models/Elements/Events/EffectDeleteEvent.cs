namespace MapEditor.Models.Elements.Events
{
    public class EffectDeleteEvent : EventBase
    {
        private string id;

        public string ID
        {
            get
            {
                return this.id;
            }

            set
            {
                this.SetValue(ref this.id, value);
            }
        }

        public override string Info => "Deletes effects with a given ID.";

        public override string Name => $"Delete Effect: \"{this.ID}\"";

        protected override string GetStringValue()
        {
            return $"effectDelete:{this.ID}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed effect delete event \"{value}\".", e => e.Length == 2 && e[0] == "effectDelete"))
            {
                return;
            }

            var newID = entries[1];

            this.ID = newID;
        }
    }
}
