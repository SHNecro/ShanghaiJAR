using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class AssaultBuster : AddOnBase
  {
    public AssaultBuster(AddOnBase.ProgramColor c)
      : base(c)
    {
      this.ID = 3;
      this.name = ShanghaiEXE.Translate("AddOn.AssaultBusterName");
      this.UseHz = 2;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.AssaultBusterDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[0] = true;
    }
  }
}
