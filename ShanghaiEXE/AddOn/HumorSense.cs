using NSGame;
using System;

namespace NSAddOn
{
    [Serializable]
    internal class HumorSense : AddOnBase
    {
        public HumorSense(AddOnBase.ProgramColor color)
          : base(color)
        {
            this.Init();
        }

        private void Init()
        {
            this.name = ShanghaiEXE.Translate("AddOn.HumorSenseName");
            this.Plus = false;
            this.UseHz = 0;
            this.UseCore = 0;
            var information = ShanghaiEXE.Translate("AddOn.HumorSenseDesc");
            this.infomasion[0] = information[0];
            this.infomasion[1] = information[1];
            this.infomasion[2] = information[2];
            this.ID = 54;
        }

        public override void Running(SaveData save)
        {
            save.message = 1;
        }
    }
}
