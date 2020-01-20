using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class CAuraSword : AddOnBase
  {
    public CAuraSword(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.CAuraSwordName");
      this.Plus = false;
      this.UseHz = 3;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.CAuraSwordDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 86;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[13] = true;
    }
  }
}
