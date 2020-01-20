namespace MapEditor.Core
{
    public class LevelDisplayOptions
    {
        public LevelDisplayOptions()
        {
            this.ShowMapImages = true;
            this.ShowWalkable = false;
            this.ShowWalkableOutline = false;
            this.ShowObjects = true;
            this.ShowHitboxes = true;
        }

        public bool ShowWalkable { get; set; }

        public bool ShowWalkableOutline { get; set; }

        public bool ShowMapImages { get; set; }

        public bool ShowObjects { get; set; }

        public bool ShowHitboxes { get; set; }
    }
}
