using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class BlackMind : AddOnBase
  {
    public BlackMind(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.BlackMindName");
      this.Plus = true;
      this.UseHz = 6;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.BlackMindDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 84;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[55] = true;
    }
  }
}
