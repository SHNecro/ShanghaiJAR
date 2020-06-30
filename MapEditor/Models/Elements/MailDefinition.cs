using System.Collections.Generic;

namespace MapEditor.Models.Elements
{
    public class MailDefinition
    {
        public string SenderKey { get; set; }
        public string SubjectKey { get; set; }

        public List<string> DialogueKeys { get; set; }

        public string Name => $"{Constants.TranslationService.Translate(this.SenderKey).Text}: {Constants.TranslationService.Translate(this.SubjectKey).Text}";
    }
}
