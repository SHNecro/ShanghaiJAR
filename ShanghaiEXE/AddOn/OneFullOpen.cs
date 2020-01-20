using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class OneFullOpen : AddOnBase
  {
    public OneFullOpen(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.OneFullOpenName");
      this.Plus = false;
      this.UseHz = 8;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.OneFullOpenDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 20;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[6] = true;
    }
  }
}
