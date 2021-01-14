using NSGame;
using System;

namespace NSAddOn
{
    [Serializable]
    internal class Sacrifice : AddOnBase
    {
        public Sacrifice(AddOnBase.ProgramColor color) : base(color)
        {
            this.Init();
        }

        private void Init()
        {
            this.ID = 96;
            this.name = ShanghaiEXE.Translate("AddOn.SacrificeName");
            this.Plus = false;
            this.UseHz = 3;
            this.UseCore = 2;
            var information = ShanghaiEXE.Translate("AddOn.SacrificeDesc");
            this.infomasion[0] = information[0];
            this.infomasion[1] = information[1];
            this.infomasion[2] = information[2];
        }

        public override void Running(SaveData save)
        {
            save.addonSkill[73] = true;
        }
    }
}
