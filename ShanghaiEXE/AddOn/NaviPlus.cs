using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class NaviPlus : AddOnBase
  {
    public NaviPlus(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.NaviPlusName");
      this.Plus = false;
      this.UseHz = 4;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.NaviPlusDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 36;
    }

    public override void Running(SaveData save)
    {
      ++save.NaviFolder;
    }
  }
}
