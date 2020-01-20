using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class BustorCharge : AddOnBase
  {
    public BustorCharge()
      : base(AddOnBase.ProgramColor.glay)
    {
      this.Init();
    }

    public BustorCharge(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.ID = 2;
      this.name = ShanghaiEXE.Translate("AddOn.BustorChargeName");
      this.UseHz = 1;
      var information = ShanghaiEXE.Translate("AddOn.BustorChargeDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
    }

    public override void Running(SaveData save)
    {
      ++save.busterspec[2];
    }
  }
}
