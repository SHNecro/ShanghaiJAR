using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class UsedCure : AddOnBase
  {
    public UsedCure(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.UsedCureName");
      this.Plus = false;
      this.UseHz = 4;
      this.UseCore = 3;
      var information = ShanghaiEXE.Translate("AddOn.UsedCureDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 38;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[31] = true;
    }
  }
}
