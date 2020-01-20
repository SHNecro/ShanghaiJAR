using NSGame;

namespace NSMap.Character.Terms
{
    internal class FlagNot : None
    {
        private readonly int flagNumber;

        public FlagNot(int number)
        {
            this.flagNumber = number;
        }

        public override bool YesNo(SaveData save)
        {
            return !save.FlagList[this.flagNumber];
        }
    }
}
