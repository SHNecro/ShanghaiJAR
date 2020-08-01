namespace MapEditor.Models.Elements.Events
{
    public class StatusHideEvent : EventBase
    {
        private bool isHiding;

        public bool IsHiding
        {
            get { return this.isHiding; }
            set { this.SetValue(ref this.isHiding, value); }
        }

        public override string Info => "Hides or shows the hud.";

        public override string Name => this.IsHiding ? "Hide HUD" : "Show HUD";

        protected override string GetStringValue()
        {
            var isHidingString = this.IsHiding ? "True" : "False";
            return $"StatusHide:{isHidingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed status hide event \"{value}\".", e => e.Length == 2 && e[0] == "StatusHide"))
            {
                return;
            }

            var newIsHiding = this.ParseBoolOrAddError(entries[1]);

            this.IsHiding = newIsHiding;
        }
    }
}
