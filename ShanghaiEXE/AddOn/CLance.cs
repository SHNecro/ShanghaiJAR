using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class CLance : AddOnBase
  {
    public CLance(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.CLanceName");
      this.Plus = false;
      this.UseHz = 1;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.CLanceDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 91;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[18] = true;
    }
  }
}
