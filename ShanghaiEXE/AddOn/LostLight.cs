using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class LostLight : AddOnBase
  {
    public LostLight(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.LostLightName");
      this.Plus = true;
      this.UseHz = 6;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.LostLightDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 73;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[42] = true;
    }
  }
}
