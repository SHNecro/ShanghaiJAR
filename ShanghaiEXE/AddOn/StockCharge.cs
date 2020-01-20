using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class StockCharge : AddOnBase
  {
    public StockCharge(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.StockChargeName");
      this.Plus = false;
      this.UseHz = 2;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.StockChargeDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 16;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[50] = true;
    }
  }
}
