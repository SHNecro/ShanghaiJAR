﻿using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class Grimoire : AddOnBase
  {
    public Grimoire(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.GrimoireName");
      this.Plus = false;
      this.UseHz = 0;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.GrimoireDesc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 40;
    }

    public override void Running(SaveData save)
    {
    }
  }
}
