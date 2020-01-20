using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class RPanelRepair : AddOnBase
  {
    public RPanelRepair(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.RPanelRepairName");
      this.Plus = false;
      this.UseHz = 0;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.RPanelRepairDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 62;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[22] = true;
    }
  }
}
