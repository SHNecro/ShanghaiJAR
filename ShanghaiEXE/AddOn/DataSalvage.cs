using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class DataSalvage : AddOnBase
  {
    public DataSalvage(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.DataSalvageName");
      this.Plus = false;
      this.UseHz = 0;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.DataSalvageDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 13;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[2] = true;
    }
  }
}
