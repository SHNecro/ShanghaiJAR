using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class BuraiStyle : AddOnBase
  {
    public BuraiStyle(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.BuraiStyleName");
      this.Plus = true;
      this.UseHz = 3;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.BuraiStyleDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 47;
    }

    public override void Running(SaveData save)
    {
      save.NaviFolder -= 999;
    }
  }
}
