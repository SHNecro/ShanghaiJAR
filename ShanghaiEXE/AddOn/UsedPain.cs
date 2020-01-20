using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class UsedPain : AddOnBase
  {
    public UsedPain(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.UsedPainName");
      this.Plus = true;
      this.UseHz = 2;
      this.UseCore = 3;
      var information = ShanghaiEXE.Translate("AddOn.UsedPainDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 39;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[65] = true;
    }
  }
}
