using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class FudouMyoou : AddOnBase
  {
    public FudouMyoou(AddOnBase.ProgramColor c)
      : base(c)
    {
      this.name = ShanghaiEXE.Translate("AddOn.FudouMyoouName");
      this.UseHz = 12;
      this.UseCore = 3;
      this.Plus = true;
      var information = ShanghaiEXE.Translate("AddOn.FudouMyoouDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 94;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[63] = true;
    }
  }
}
