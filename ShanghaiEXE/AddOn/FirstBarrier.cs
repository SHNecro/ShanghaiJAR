using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class FirstBarrier : AddOnBase
  {
    public FirstBarrier(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.FirstBarrierName");
      this.Plus = false;
      this.UseHz = 3;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.FirstBarrierDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 19;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[48] = true;
    }
  }
}
