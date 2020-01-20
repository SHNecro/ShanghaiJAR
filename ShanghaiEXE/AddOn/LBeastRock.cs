using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class LBeastRock : AddOnBase
  {
    public LBeastRock(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.LBeastRockName");
      this.Plus = false;
      this.UseHz = 3;
      this.UseCore = 3;
      var information = ShanghaiEXE.Translate("AddOn.LBeastRockDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 67;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[27] = true;
    }
  }
}
