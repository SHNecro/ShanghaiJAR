using NSChip;

namespace MapEditor.Models.Elements
{
    public class ChipDefinition
	{
        private ChipDefinition() { }

		public static ChipDefinition GetChipDefinition(int id)
        {
            var definition = new ChipDefinition();

            var chipFolder = new ChipFolder(null);
            var chipMade = chipFolder.ReturnChip(id);

            var type = typeof(ChipFolder).Assembly.GetType("NSChip.DammyChip");
            if (chipMade.GetType() == type)
            {
                return null;
            }

            definition.ID = chipMade.number;
            definition.Name = chipMade.name;
            definition.NameKey = Constants.TranslationCallKeys[definition.Name];
            definition.Codes = chipMade.code;
            Constants.TranslationCallKeys.Clear();
            
            return definition;
		}

        public int ID { get; set; }

        public string Name { get; set; }

        public string NameKey { get; set; }

        public ChipFolder.CODE[] Codes { get; set; }
    }
}
