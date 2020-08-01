namespace MapEditor.Models.Elements.Events
{
    public class GetMailEvent : EventBase
    {
        private int mailNumber;
        private bool isPlayingEffect;

        public GetMailEvent()
        {
            // TODO: MEMORY LEAK
            // However, would require propagating Dispose() all the way down Map, which would have been nice to do at the very start
            if (Constants.MailDefinitions != null)
            {
                Constants.MailDefinitions.CollectionChanged += (sender, args) => { this.OnPropertyChanged(nameof(this.MailNumber)); };
            }
        }

        public int MailNumber
        {
            get { return this.mailNumber; }
            set { this.SetValue(ref this.mailNumber, value); }
        }

        public bool IsPlayingEffect
        {
            get { return this.isPlayingEffect; }
            set { this.SetValue(ref this.isPlayingEffect, value); }
        }

        public override string Info => "Adds a mail, optionally playing the mail effect.";

        public override string Name
        {
            get
            {
                var mailName = Constants.MailDefinitions.ContainsKey(this.MailNumber) ? Constants.MailDefinitions[this.MailNumber].Name : "INVALID";
                var isPlayingEffectString = this.IsPlayingEffect ? string.Empty : " (Silent)";
                return $"Get Mail: {mailName}{isPlayingEffectString}";
            }
        }

        protected override string GetStringValue()
        {
            var isPlayingEffectString = this.IsPlayingEffect ? "True" : "False";
            return $"GetMail:{this.MailNumber}:{isPlayingEffectString}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, $"Malformed Mail event \"{value}\".", e => e.Length == 3 && e[0] == "GetMail"))
            {
                return;
            }

            var newMailNumber = this.ParseIntOrAddError(entries[1], () => this.MailNumber, (ev) => ev >= 0, (ev) => $"Invalid mail number {ev} (>= 0)");
            var newIsPlayingEffect = this.ParseBoolOrAddError(entries[2]);

            this.MailNumber = newMailNumber;
            this.IsPlayingEffect = newIsPlayingEffect;
        }
    }
}
