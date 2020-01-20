using System;

namespace NSGame
{
    [Serializable]
    public class Style
    {
        public int style;
        public int element;

        public string name
        {
            get
            {
                string str = "";
                switch (this.style)
                {
                    case 0:
                        return ShanghaiEXE.Translate("Style.StyleNormal");
                    case 1:
                        str = ShanghaiEXE.Translate("Style.StyleFght");
                        break;
                    case 2:
                        str = ShanghaiEXE.Translate("Style.StyleNinj");
                        break;
                    case 3:
                        str = ShanghaiEXE.Translate("Style.StyleDoc");
                        break;
                    case 4:
                        str = ShanghaiEXE.Translate("Style.StyleGaia");
                        break;
                    case 5:
                        str = ShanghaiEXE.Translate("Style.StyleWing");
                        break;
                    case 6:
                        str = ShanghaiEXE.Translate("Style.StyleWtch");
                        break;
                }
                switch (this.element)
                {
                    case 0:
                        return ShanghaiEXE.Translate("Style.ElemNormal");
                    case 1:
                        str = ShanghaiEXE.Translate("Style.ElemHeat") + str;
                        break;
                    case 2:
                        str = ShanghaiEXE.Translate("Style.ElemAqua") + str;
                        break;
                    case 3:
                        str = ShanghaiEXE.Translate("Style.ElemElec") + str;
                        break;
                    case 4:
                        str = ShanghaiEXE.Translate("Style.ElemLeaf") + str;
                        break;
                    case 5:
                        str = ShanghaiEXE.Translate("Style.ElemPois") + str;
                        break;
                    case 6:
                        str = ShanghaiEXE.Translate("Style.ElemErth") + str;
                        break;
                }
                return str;
            }
        }

        public string fileName
        {
            get
            {
                string str = "";
                bool flag = false;
                switch (this.style)
                {
                    case 0:
                        str = "shanghai";
                        flag = true;
                        break;
                    case 1:
                        str = "Fighter";
                        break;
                    case 2:
                        str = "Shinobi";
                        break;
                    case 3:
                        str = "Doctor";
                        break;
                    case 4:
                        str = "Gaia";
                        break;
                    case 5:
                        str = "Wing";
                        break;
                    case 6:
                        str = "Witch";
                        break;
                }
                if (flag)
                    return "shanghai";
                switch (this.element)
                {
                    case 1:
                        str += "Heat";
                        break;
                    case 2:
                        str += "Aqua";
                        break;
                    case 3:
                        str += "Eleki";
                        break;
                    case 4:
                        str += "Leaf";
                        break;
                    case 5:
                        str += "Poison";
                        break;
                    case 6:
                        str += "Earth";
                        break;
                }
                return str;
            }
        }
    }
}
