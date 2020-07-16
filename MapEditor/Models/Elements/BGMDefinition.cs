namespace MapEditor.Models.Elements
{
    public class BGMDefinition
    {
        public string File { get; set; }

        public string Name { get; set; }

        public string Label => $"{this.File}.ogg ({this.Name})";
    }
}
