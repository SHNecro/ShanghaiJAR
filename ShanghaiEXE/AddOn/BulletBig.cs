﻿using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class BulletBig : AddOnBase
  {
    public BulletBig(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.BulletBigName");
      this.Plus = false;
      this.UseHz = 0;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.BulletBigDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 59;
    }

    public override void Running(SaveData save)
    {
      save.addonSkill[37] = true;
    }
  }
}
