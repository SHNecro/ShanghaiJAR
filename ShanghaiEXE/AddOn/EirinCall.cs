using NSGame;
using System;

namespace NSAddOn
{
    [Serializable]
    internal class EirinCall : AddOnBase
    {
        public EirinCall(AddOnBase.ProgramColor color)
          : base(color)
        {
            this.Init();
        }

        private void Init()
        {
            this.name = ShanghaiEXE.Translate("AddOn.EirinCallName");
            this.Plus = false;
            this.UseHz = 0;
            this.UseCore = 0;
            var information = ShanghaiEXE.Translate("AddOn.EirinCallDesc");
            this.infomasion[0] = information[0];
            this.infomasion[1] = information[1];
            this.infomasion[2] = information[2];
            this.ID = 57;
        }

        public override void Running(SaveData save)
        {
            save.message = 4;
        }
    }
}
