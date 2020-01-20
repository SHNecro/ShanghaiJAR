using NSGame;

namespace NSMap.Character.Terms
{
    internal class Havechip : None
    {
        private readonly int chipNumber;
        private readonly byte codeNumber;

        public Havechip(int number, byte code)
        {
            this.chipNumber = number;
            this.codeNumber = code;
        }

        public override bool YesNo(SaveData save)
        {
            return save.ListCheck(this.chipNumber, codeNumber - 1);
        }
    }
}
