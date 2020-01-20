using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class DarkPlus : AddOnBase
  {
    public DarkPlus(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.DarkPlusName");
      this.Plus = false;
      this.UseHz = 6;
      this.UseCore = 3;
      var information = ShanghaiEXE.Translate("AddOn.DarkPlusDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 37;
    }

    public override void Running(SaveData save)
    {
      ++save.darkFolder;
    }
  }
}
