using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class HPPlus50 : AddOnBase
  {
    public HPPlus50(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.HPPlus50Name");
      this.Plus = false;
      this.UseHz = 2;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.HPPlus50Desc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 8;
    }

    public override void Running(SaveData save)
    {
      save.HPplus += 50;
    }
  }
}
