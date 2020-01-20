using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class BlueBuster : AddOnBase
  {
    public BlueBuster(AddOnBase.ProgramColor c)
      : base(c)
    {
      this.ID = 4;
      this.name = ShanghaiEXE.Translate("AddOn.BlueBusterName");
      this.UseHz = 0;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.BlueBusterDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[38] = true;
    }
  }
}
