using NSGame;
using System.Windows.Forms;

namespace NSMap.Character.Terms
{
    internal class Variable : None
    {
        private readonly int varNumber;
        private readonly bool value_OR_variable;
        private readonly int mark;
        private readonly int value;

        public Variable(int number, bool vOv, int ma, int var)
        {
            this.varNumber = number;
            this.value_OR_variable = vOv;
            this.value = var;
            this.mark = ma;
        }

        public override bool YesNo(SaveData save)
        {
            bool flag = false;
            int num1;
            try
            {
                num1 = this.value_OR_variable ? save.ValList[this.value] : this.value;
            }
            catch
            {
                int num2 = (int)MessageBox.Show("The variable is out of bounds!");
                num1 = this.value;
            }
            if (this.mark == 0)
                flag = save.ValList[this.varNumber] == num1;
            else if (this.mark == 4)
                flag = save.ValList[this.varNumber] > num1;
            else if (this.mark == 3)
                flag = save.ValList[this.varNumber] < num1;
            else if (this.mark == 2)
                flag = save.ValList[this.varNumber] >= num1;
            else if (this.mark == 1)
                flag = save.ValList[this.varNumber] <= num1;
            else if (this.mark == 5)
            {
                flag = save.ValList[this.varNumber] != num1;
            }
            else
            {
                int num3 = (int)MessageBox.Show("Illegal sign!");
            }
            return flag;
        }
    }
}
