using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class NoGuard : AddOnBase
  {
    public NoGuard(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.NoGuardName");
      this.Plus = true;
      this.UseHz = 8;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.NoGuardDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 82;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[64] = true;
    }
  }
}
