using System;
using System.Collections.Generic;

namespace NSGame
{
    [Serializable]
    public class KeyItem
    {
        public List<string> info = new List<string>();
        public string name;

        protected void AddTXT(string t)
        {
            this.info.Add(t);
        }
    }
}
