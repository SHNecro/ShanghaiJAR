using NSGame;
using System;

namespace NSAddOn
{
    [Serializable]
    internal class Mammon : AddOnBase
    {
        public Mammon(AddOnBase.ProgramColor color) : base(color)
        {
            this.Init();
        }

        private void Init()
        {
            this.ID = 97;
            this.name = ShanghaiEXE.Translate("AddOn.MammonName");
            this.Plus = true;
            this.UseHz = 4;
            this.UseCore = 3;
            var information = ShanghaiEXE.Translate("AddOn.MammonDesc");
            this.infomasion[0] = information[0];
            this.infomasion[1] = information[1];
            this.infomasion[2] = information[2];
        }

        public override void Running(SaveData save)
        {
            save.addonSkill[74] = true;
        }
    }
}
