using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class CustomPain : AddOnBase
  {
    public CustomPain(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.CustomPainName");
      this.Plus = true;
      this.UseHz = 6;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.CustomPainDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 76;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[45] = true;
    }
  }
}
