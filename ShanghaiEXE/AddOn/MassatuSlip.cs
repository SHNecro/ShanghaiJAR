using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class MassatuSlip : AddOnBase
  {
    public MassatuSlip(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.MassatuSlipName");
      this.Plus = true;
      this.UseHz = 4;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.MassatuSlipDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 74;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[41] = true;
    }
  }
}
