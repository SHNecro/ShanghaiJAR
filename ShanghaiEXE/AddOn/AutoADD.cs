using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class AutoADD : AddOnBase
  {
    public AutoADD(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.AutoADDName");
      this.Plus = false;
      this.UseHz = 10;
      this.UseCore = 3;
      var information = ShanghaiEXE.Translate("AddOn.AutoADDDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 21;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[68] = true;
    }
  }
}
