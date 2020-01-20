namespace MapEditor.Models.Elements.Events
{
    public class MessageCloseEvent : EventBase
    {
        public override string Info => "Closes the message box.";

        public override string Name => "Close Message Box";

        protected override string GetStringValue()
        {
            return "msgclose:";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, "Malformed message close.", v => v == "msgclose:");
        }
    }
}
