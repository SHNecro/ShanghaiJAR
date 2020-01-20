using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class HPPlus500 : AddOnBase
  {
    public HPPlus500(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.HPPlus500Name");
      this.Plus = false;
      this.UseHz = 10;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.HPPlus500Desc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 11;
    }

    public override void Running(SaveData save)
    {
      save.HPplus += 500;
    }
  }
}
