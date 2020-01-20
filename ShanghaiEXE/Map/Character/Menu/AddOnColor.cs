using NSAddOn;

namespace NSMap.Character.Menu
{
    internal class AddOnColor
    {
        public int number;
        public AddOnBase.ProgramColor color;

        public AddOnColor(int n, AddOnBase.ProgramColor c)
        {
            this.number = n;
            this.color = c;
        }
    }
}
