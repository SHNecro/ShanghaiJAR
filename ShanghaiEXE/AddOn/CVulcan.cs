using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class CVulcan : AddOnBase
  {
    public CVulcan(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.CVulcanName");
      this.Plus = false;
      this.UseHz = 4;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.CVulcanDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 88;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[15] = true;
    }
  }
}
