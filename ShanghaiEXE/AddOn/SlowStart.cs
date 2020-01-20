using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class SlowStart : AddOnBase
  {
    public SlowStart(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.SlowStartName");
      this.Plus = true;
      this.UseHz = 5;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.SlowStartDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 68;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[66] = true;
    }
  }
}
