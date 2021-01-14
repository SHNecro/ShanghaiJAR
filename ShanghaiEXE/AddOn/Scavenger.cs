using NSGame;
using System;

namespace NSAddOn
{
    [Serializable]
    internal class Scavenger : AddOnBase
    {
        public Scavenger(AddOnBase.ProgramColor color) : base(color)
        {
            this.Init();
        }

        private void Init()
        {
            this.ID = 95;
            this.name = ShanghaiEXE.Translate("AddOn.ScavengerName");
            this.Plus = false;
            this.UseHz = 1;
            this.UseCore = 1;
            var information = ShanghaiEXE.Translate("AddOn.ScavengerDesc");
            this.infomasion[0] = information[0];
            this.infomasion[1] = information[1];
            this.infomasion[2] = information[2];
        }

        public override void Running(SaveData save)
        {
            save.addonSkill[72] = true;
        }
    }
}
