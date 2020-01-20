using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class Meltingth : AddOnBase
  {
    public Meltingth(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.MeltingthName");
      this.Plus = true;
      this.UseHz = 10;
      this.UseCore = 3;
      var information = ShanghaiEXE.Translate("AddOn.MeltingthDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 75;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[40] = true;
    }
  }
}
