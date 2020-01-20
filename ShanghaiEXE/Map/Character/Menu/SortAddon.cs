using NSAddOn;

namespace NSMap.Character.Menu
{
    internal class SortAddon
    {
        public AddOnBase addon;
        public int number;
        public bool set;
        public bool nowset;

        public SortAddon(AddOnBase a, int n, bool s, bool nows)
        {
            this.addon = a;
            this.number = n;
            this.set = s;
            this.nowset = nows;
        }
    }
}
