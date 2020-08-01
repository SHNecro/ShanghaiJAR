using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public class MessageEvent : EventBase, ITranslatedModel
    {
        private string messageKey;

        public string MessageKey
        {
            get
            {
                return this.messageKey;
            }
            set
            {
                this.SetValue(ref this.messageKey, value);
            }
        }

        public override string Info => "Prints a message in the message box.";

        public override string Name
        {
            get
            {
                var dialogue = Constants.TranslationService.Translate(this.MessageKey);
                return $"Msg: {dialogue.Face.ToString()}: {dialogue.Text}";
            }
        }

        public void RefreshTranslation()
        {
            this.OnPropertyChanged(nameof(this.Name));
        }

        protected override string GetStringValue()
        {
            return $"msg:{this.MessageKey}";
        }

        protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, "Invalid number of parameters.", e => e.Length == 2 && e[0] == "msg"))
            {
                return;
            }
            
            var newMessageKey = entries[1];
            this.Validate(newMessageKey, () => this.MessageKey, s => $"Message key \"{s}\" does not exist.", Constants.TranslationService.CanTranslate);

            this.MessageKey = newMessageKey;
        }
    }
}
