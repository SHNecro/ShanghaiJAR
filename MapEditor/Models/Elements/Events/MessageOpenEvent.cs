namespace MapEditor.Models.Elements.Events
{
    public class MessageOpenEvent : EventBase
    {
        public override string Info => "Shows the message box.";

        public override string Name => "Open Message Box";

        protected override string GetStringValue()
        {
            return "msgopen:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, "Malformed message open.", v => v == "msgopen:");
        }
    }
}
