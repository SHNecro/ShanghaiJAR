namespace MapEditor.Models.Elements.Terms
{
    public class NoneTerm : TermBase
    {
        public override string Name => "None";

        protected override string GetStringValue()
        {
            return "none";
        }

        protected override void SetStringValue(string value)
        {
            this.Validate(value, "Malformed term.", v => v == "none");
        }
    }
}
