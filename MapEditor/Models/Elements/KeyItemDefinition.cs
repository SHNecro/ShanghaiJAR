using System.Collections.Generic;

namespace MapEditor.Models.Elements
{
    public class KeyItemDefinition
    {
        public string NameKey { get; set; }

        public List<string> DialogueKeys { get; set; }

        public string Name => Constants.TranslationService.Translate(this.NameKey);
    }
}
