using System.Collections.Generic;

namespace Common.OpenGL
{
    public class RenderedObjectCollection
    {
        public RenderedObjectCollection()
        {
            this.Sprites = new List<Sprite>();
            this.Levels = new List<Sprite>();
            this.Quads = new List<Quad>();
            this.Texts = new List<Text>();
        }

        public IList<Sprite> Sprites { get; set; }
        public IList<Sprite> Levels { get; set; }
        public IList<Quad> Quads { get; set; }
        public IList<Text> Texts { get; set; }
    }
}
