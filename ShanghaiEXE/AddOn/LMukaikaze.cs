using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class LMukaikaze : AddOnBase
  {
    public LMukaikaze(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.LMukaikazeName");
      this.Plus = false;
      this.UseHz = 4;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.LMukaikazeDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 65;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[25] = true;
    }
  }
}
