using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class OwataManBody : AddOnBase
  {
    public OwataManBody(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.OwataManBodyName");
      this.Plus = true;
      this.UseHz = 20;
      this.UseCore = 5;
      var information = ShanghaiEXE.Translate("AddOn.OwataManBodyDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 81;
    }

    public override void Running(SaveData save)
    {
      save.HPplus -= 99999999;
    }
  }
}
