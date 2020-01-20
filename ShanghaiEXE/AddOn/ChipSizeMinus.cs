using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class ChipSizeMinus : AddOnBase
  {
    public ChipSizeMinus(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.ChipSizeMinusName");
      this.Plus = true;
      this.UseHz = 4;
      this.UseCore = 4;
      var information = ShanghaiEXE.Translate("AddOn.ChipSizeMinusDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 34;
    }

    public override void Running(SaveData save)
    {
      --save.plusFolder;
    }
  }
}
