using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class CRepair : AddOnBase
  {
    public CRepair(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.CRepairName");
      this.Plus = false;
      this.UseHz = 4;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.CRepairDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 92;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[20] = true;
    }
  }
}
