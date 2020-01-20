using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class BustorRapid : AddOnBase
  {
    public BustorRapid()
      : base(AddOnBase.ProgramColor.glay)
    {
      this.Init();
    }

    public BustorRapid(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.ID = 1;
      this.name = ShanghaiEXE.Translate("AddOn.BustorRapidName");
      this.UseHz = 1;
      var information = ShanghaiEXE.Translate("AddOn.BustorRapidDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
    }

    public override void Running(SaveData save)
    {
      ++save.busterspec[1];
    }
  }
}
