using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class LostCustom : AddOnBase
  {
    public LostCustom(AddOnBase.ProgramColor c)
      : base(c)
    {
      this.name = ShanghaiEXE.Translate("AddOn.LostCustomName");
      this.UseHz = 8;
      this.UseCore = 2;
      this.Plus = true;
      var information = ShanghaiEXE.Translate("AddOn.LostCustomDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 83;
    }

    public override void Running(SaveData save)
    {
      --save.custom;
    }
  }
}
