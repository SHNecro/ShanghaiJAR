using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class CFallKnife : AddOnBase
  {
    public CFallKnife(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.CFallKnifeName");
      this.Plus = false;
      this.UseHz = 2;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.CFallKnifeDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 89;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[16] = true;
    }
  }
}
