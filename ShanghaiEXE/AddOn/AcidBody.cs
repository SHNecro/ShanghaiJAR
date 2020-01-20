using NSGame;
using System;

namespace NSAddOn
{
    [Serializable]
    internal class AcidBody : AddOnBase
    {
        public AcidBody(AddOnBase.ProgramColor color)
          : base(color)
        {
            this.Init();
        }

        private void Init()
        {
            this.name = ShanghaiEXE.Translate("AddOn.AcidBodyName");
            this.Plus = true;
            this.UseHz = 7;
            this.UseCore = 0;
            var information = ShanghaiEXE.Translate("AddOn.AcidBodyDesc");
            this.infomasion[0] = information[0];
            this.infomasion[1] = information[1];
            this.infomasion[2] = information[2];
            this.ID = 71;
        }

        public override void Running(SaveData save)
        {
            save.addonSkill[44] = true;
        }
    }
}
