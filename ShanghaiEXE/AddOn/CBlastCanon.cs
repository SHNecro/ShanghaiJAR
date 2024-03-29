﻿using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class CBlastCanon : AddOnBase
  {
    public CBlastCanon(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.CBlastCanonName");
      this.Plus = false;
      this.UseHz = 4;
      this.UseCore = 2;
      var information = ShanghaiEXE.Translate("AddOn.CBlastCanonDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 90;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[17] = true;
    }
  }
}
