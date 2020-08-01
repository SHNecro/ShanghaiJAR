namespace MapEditor.Models.Elements.Events
{
    public class EventDeleteEvent : EventBase
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

        public override string Info => "Deletes objects with a given ID.";

        public override string Name => $"Delete Objects: \"{this.ID}\"";

        protected override string GetStringValue()
        {
            return $"eventDelete:{this.ID}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed object delete event \"{value}\".", e => e.Length == 2 && e[0] == "eventDelete"))
            {
                return;
            }

            var newID = entries[1];

            this.ID = newID;
        }
    }
}
