using System.Collections.Generic;

namespace MapEditor.Core
{
    public class Chip
    {
        public int ID { get; set; }
        public int? CodeNumber { get; set; }

        public bool IsRandom { get; set; }
        public double RandomChance { get; set; }
        public List<Chip> RandomAlternatives { get; set; }
    }
}
