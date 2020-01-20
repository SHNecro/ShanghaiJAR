using NSGame;
using System.Collections.Generic;

namespace NSShanghaiEXE.Common
{
    public class VariableArray : List<int>
    {
        public VariableArray()
        : base(new int[200]) { }

        public new int this[int i]
        {
            get => this.OverwrittenVariables(i) ?? base[i];
            set => base[i] = value;
        }

        private int? OverwrittenVariables(int i)
        {
            if (i == 14 && (ShanghaiEXE.Config.DisableBGMOverride ?? false))
            {
                return 0;
            }

            return null;
        }
    }
}
