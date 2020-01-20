using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class ChipSizePlus : AddOnBase
  {
    public ChipSizePlus(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.ChipSizePlusName");
      this.Plus = false;
      this.UseHz = 14;
      this.UseCore = 8;
      var information = ShanghaiEXE.Translate("AddOn.ChipSizePlusDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 35;
    }

    public override void Running(SaveData save)
    {
      ++save.plusFolder;
    }
  }
}
