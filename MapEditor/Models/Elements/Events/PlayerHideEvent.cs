namespace MapEditor.Models.Elements.Events
{
    public class PlayerHideEvent : EventBase
    {
        private bool isHiding;

        public bool IsHiding
        {
            get { return this.isHiding; }
            set { this.SetValue(ref this.isHiding, value); }
        }

        public override string Info => "Hides or shows the player.";

        public override string Name => this.IsHiding ? "Hide Player" : "Show Player";

        protected override string GetStringValue()
        {
            var isHidingString = this.IsHiding ? "True" : "False";
            return $"playerHide:{isHidingString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed player hide event \"{value}\".", e => e.Length == 2 && e[0] == "playerHide"))
            {
                return;
            }
            
            var newIsHiding = this.ParseBoolOrAddError(entries[1]);

            this.IsHiding = newIsHiding;
        }
    }
}
