using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class RShield : AddOnBase
  {
    public RShield(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.RShieldName");
      this.Plus = false;
      this.UseHz = 4;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.RShieldDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 61;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[21] = true;
    }
  }
}
