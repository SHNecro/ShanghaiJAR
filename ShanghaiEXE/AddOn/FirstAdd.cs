using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class FirstAdd : AddOnBase
  {
    public FirstAdd(AddOnBase.ProgramColor c)
      : base(c)
    {
      this.name = ShanghaiEXE.Translate("AddOn.FirstAddName");
      this.UseHz = 4;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.FirstAddDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 18;
    }

    public override void Running(SaveData save)
    {
      ++save.custom;
    }
  }
}
