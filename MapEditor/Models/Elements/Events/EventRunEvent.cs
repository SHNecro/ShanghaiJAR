namespace MapEditor.Models.Elements.Events
{
    public class EventRunEvent : EventBase
    {
        private string id;
        private int page;
        private bool isParallel;

        public string ID
        {
            get { return this.id; }
            set { this.SetValue(ref this.id, value); }
        }

        public int Page
        {
            get { return this.page; }
            set { this.SetValue(ref this.page, value); }
        }

        public bool IsParallel
        {
            get { return this.isParallel; }
            set { this.SetValue(ref this.isParallel, value); }
        }

        public override string Info => "Runs the events of an object's active page or one specified.";

        public override string Name
        {
            get
            {
                var subtype = this.IsParallel ? " (parallel)" : string.Empty;
                var pageString = this.Page == -1 ? "Active page" : $"pg. {this.Page}";
                return $"Run events{subtype}: \"{this.ID}\" {pageString}";
            }
        }

        protected override string GetStringValue()
        {
            var subtype = this.IsParallel ? "EventLunPara" : "EventLun";
            return $"{subtype}:{this.ID}:{this.Page}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed event run event \"{value}\".", e => e.Length == 3 && (e[0] == "EventLun" || e[0] == "EventLunPara" )))
            {
                return;
            }

            var newID = entries[1];
            var newPage = this.ParseIntOrAddError(entries[2]);
            var newIsParallel = entries[0] == "EventLunPara";

            this.ID = newID;
            this.Page = newPage;
            this.IsParallel = newIsParallel;
        }
    }
}
