using MapEditor.Core;

namespace MapEditor.Models.Elements.Events
{
    public class QuestionEvent : EventBase, ITranslatedModel
    {
        private string questionKey;
        private string answerKey;

        public string QuestionKey
        {
            get
            {
                return this.questionKey;
            }
            set
            {
                this.SetValue(ref this.questionKey, value);
            }
        }

        public string AnswerKey
        {
            get
            {
                return this.answerKey;
            }
            set
            {
                this.SetValue(ref this.answerKey, value);
            }
        }

        public override string Info => "Asks a question in the message box. The selected option index is used by BranchHead to determine what events to execute, or EditValue. The number of question lines and answers can be (Q-A): (1-2, 2-2, 0-3, 1-4)";

        public override string Name
        {
            get
            {
                var dialogue = Constants.TranslationService.Translate(this.QuestionKey);
                return $"Q: {dialogue.Face.ToString()}: {dialogue.Text}: {Constants.TranslationService.Translate(this.AnswerKey).Text}";
            }
        }

        public void RefreshTranslation()
        {
            this.OnPropertyChanged(nameof(this.Name));
        }

        protected override string GetStringValue()
        {
            return $"question:{this.QuestionKey}:{this.AnswerKey}";
        }

		// TODO:
		protected override void SetStringValue(string value)
        {
            var entries = value.Split(':');
            if (!this.Validate(entries, "Malformed question event.", e => e.Length == 3 && e[0] == "question"))
            {
                return;
            }

            var newQuestionKey = entries[1];
            this.Validate(newQuestionKey, () => this.QuestionKey, s => $"Question key \"{s}\" does not exist.", Constants.TranslationService.CanTranslate);

            var newAnswerKey = entries[2];
            this.Validate(newAnswerKey, () => this.AnswerKey, s => $"Answer key \"{s}\" does not exist.", Constants.TranslationService.CanTranslate);

            this.QuestionKey = newQuestionKey;
            this.AnswerKey = newAnswerKey;
        }
    }
}
