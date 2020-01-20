using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class Haisui : AddOnBase
  {
    public Haisui(AddOnBase.ProgramColor c)
      : base(c)
    {
      this.name = ShanghaiEXE.Translate("AddOn.HaisuiName");
      this.UseHz = 9;
      this.UseCore = 6;
      this.Plus = true;
      var information = ShanghaiEXE.Translate("AddOn.HaisuiDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 33;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[58] = true;
    }
  }
}
