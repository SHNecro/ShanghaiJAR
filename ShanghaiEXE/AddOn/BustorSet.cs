using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class BustorSet : AddOnBase
  {
    public BustorSet(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.BustorSetName");
      this.UseHz = 9;
      this.UseCore = 1;
      var information = ShanghaiEXE.Translate("AddOn.BustorSetDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 6;
    }

    public override void Running(SaveData save)
    {
      save.busterspec[0] += 3;
      save.busterspec[1] += 3;
      save.busterspec[2] += 3;
    }
  }
}
