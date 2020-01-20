using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class HPDown100 : AddOnBase
  {
    public HPDown100()
      : base(AddOnBase.ProgramColor.dark)
    {
      this.Init();
    }

    public HPDown100(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.HPDown100Name");
      this.Plus = true;
      this.UseHz = 3;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.HPDown100Desc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 70;
    }

    public override void Running(SaveData save)
    {
      save.HPplus -= 100;
    }
  }
}
