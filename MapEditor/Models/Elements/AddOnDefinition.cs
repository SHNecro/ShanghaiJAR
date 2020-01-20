using NSAddOn;

namespace MapEditor.Models.Elements
{
    public class AddOnDefinition
    {
        private AddOnDefinition() { }

		public static AddOnDefinition GetAddOnDefinition(int id)
        {
            var definition = new AddOnDefinition();
            
            var addOnMade = AddOnBase.AddOnSet(id, 0);
            if (addOnMade == null)
            {
                return null;
            }

            definition.ID = addOnMade.ID;
            definition.Name = addOnMade.name;
            definition.NameKey = Constants.TranslationCallKeys[definition.Name];
            Constants.TranslationCallKeys.Clear();
            
            return definition;
		}

        public int ID { get; set; }

        public string Name { get; set; }

        public string NameKey { get; set; }
    }
}
