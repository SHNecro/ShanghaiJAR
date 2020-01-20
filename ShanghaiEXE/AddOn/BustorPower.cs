using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class BustorPower : AddOnBase
  {
    public BustorPower()
      : base(AddOnBase.ProgramColor.glay)
    {
      this.Init();
    }

    public BustorPower(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.BustorPowerName");
      this.UseHz = 2;
      var information = ShanghaiEXE.Translate("AddOn.BustorPowerDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 0;
    }

    public override void Running(SaveData save)
    {
      ++save.busterspec[0];
    }
  }
}
