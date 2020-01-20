using NSGame;

namespace NSMap.Character.Terms
{
    internal class Flag : None
    {
        private readonly int flagNumber;

        public Flag(int number)
        {
            this.flagNumber = number;
        }

        public override bool YesNo(SaveData save)
        {
            return save.FlagList[this.flagNumber];
        }
    }
}
