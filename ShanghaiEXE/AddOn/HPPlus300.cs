﻿using NSGame;
using System;

namespace NSAddOn
{
  [Serializable]
  internal class HPPlus300 : AddOnBase
  {
    public HPPlus300(AddOnBase.ProgramColor color)
      : base(color)
    {
      this.Init();
    }

    private void Init()
    {
      this.name = ShanghaiEXE.Translate("AddOn.HPPlus300Name");
      this.Plus = false;
      this.UseHz = 7;
      this.UseCore = 0;
      var information = ShanghaiEXE.Translate("AddOn.HPPlus300Desc");
      this.infomasion[0] = information[0];
      this.infomasion[1] = information[1];
      this.infomasion[2] = information[2];
      this.ID = 14;
    }

    public override void Running(SaveData save)
    {
      save.HPplus += 300;
    }
  }
}
