using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class HardObject : AddOnBase
  {
    public HardObject(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.HardObjectName");
      this.Plus = false;
      this.UseHz = 2;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.HardObjectDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 44;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[67] = true;
    }
  }
}
