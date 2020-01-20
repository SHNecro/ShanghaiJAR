using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class StyleReUse : AddOnBase
  {
    public StyleReUse(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.StyleReUseName");
      this.Plus = false;
      this.UseHz = 5;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.StyleReUseDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 32;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[29] = true;
    }
  }
}
