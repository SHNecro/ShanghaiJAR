using NSChip;
using System;

namespace NSGame
{
    [Serializable]
    public class ChipS
    {
        public int number;
        public int code;

        public ChipS(int n, int c)
        {
            this.number = n;
            this.code = c;
        }

        public float sortNumber
        {
            get
            {
                return this.SetChip().chip.sortNumber;
            }
        }

        public string Name
        {
            get
            {
                return this.SetChip().chip.name;
            }
        }

        public int Element
        {
            get
            {
                return (int)this.SetChip().chip.element;
            }
        }

        public int Power
        {
            get
            {
                return this.SetChip().chip.power;
            }
        }

        public int Regsize
        {
            get
            {
                return this.SetChip().chip.regsize;
            }
        }

        public byte Reality
        {
            get
            {
                return this.SetChip().chip.reality;
            }
        }

        public int Code
        {
            get
            {
                ChipFolder chipFolder = this.SetChip();
                return (int)chipFolder.chip.code[chipFolder.codeNo];
            }
        }

        public int Many(SaveData save)
        {
            return save.Havechip[this.number, this.code];
        }

        private ChipFolder SetChip()
        {
            ChipFolder chipFolder = new ChipFolder(null);
            chipFolder.SettingChip(this.number);
            chipFolder.codeNo = this.code;
            return chipFolder;
        }
    }
}
