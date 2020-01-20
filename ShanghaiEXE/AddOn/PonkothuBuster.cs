using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class PonkothuBuster : AddOnBase
  {
    public PonkothuBuster(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.PonkothuBusterName");
      this.Plus = true;
      this.UseHz = 4;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.PonkothuBusterDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 80;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[54] = true;
    }
  }
}
