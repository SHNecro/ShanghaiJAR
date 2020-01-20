using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class CDustBomb : AddOnBase
  {
    public CDustBomb(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.CDustBombName");
      this.Plus = false;
      this.UseHz = 2;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.CDustBombDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 87;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[14] = true;
    }
  }
}
