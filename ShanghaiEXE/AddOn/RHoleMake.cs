using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class RHoleMake : AddOnBase
  {
    public RHoleMake(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.RHoleMakeName");
      this.Plus = false;
      this.UseHz = 2;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.RHoleMakeDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 63;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[23] = true;
    }
  }
}
