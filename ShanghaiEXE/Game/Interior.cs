using System;

namespace NSGame
{
    [Serializable]
    public class Interior
    {
        public int number;
        public int posiX;
        public int posiY;
        public bool set;
        public bool rebirth;

        public Interior(int number, int posiX, int posiY, bool set, bool rebirth)
        {
            this.number = number;
            this.posiX = posiX;
            this.posiY = posiY;
            this.set = set;
            this.rebirth = rebirth;
        }
    }
}
