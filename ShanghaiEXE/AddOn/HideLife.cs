using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class HideLife : AddOnBase
  {
    public HideLife(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.HideLifeName");
      this.Plus = true;
      this.UseHz = 0;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.HideLifeDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 72;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[57] = true;
    }
  }
}
