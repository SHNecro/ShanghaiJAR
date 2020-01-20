using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class EriaGuard : AddOnBase
  {
    public EriaGuard(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.EriaGuardName");
      this.Plus = false;
      this.UseHz = 4;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.EriaGuardDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 29;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[46] = true;
    }
  }
}
