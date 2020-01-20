using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class HeavyFoot : AddOnBase
  {
    public HeavyFoot(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.HeavyFootName");
      this.Plus = true;
      this.UseHz = 8;
      this.UseCore = 3;
      var information = ShanghaiEXE.Translate("AddOn.HeavyFootDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 77;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[43] = true;
    }
  }
}
