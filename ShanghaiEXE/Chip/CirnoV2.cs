﻿using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class CirnoV2 : CirnoV1
  {
    private const int start = 44;
    private const int speed = 2;

    public CirnoV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 219;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.CirnoV2Name");
      this.element = ChipBase.ELEMENT.aqua;
      this.power = 120;
      this.subpower = 30;
      this.regsize = 39;
      this.reality = 4;
      this._break = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.Q;
      this.code[2] = ChipFolder.CODE.C;
      this.code[3] = ChipFolder.CODE.Q;
      var information = NSGame.ShanghaiEXE.Translate("Chip.CirnoV2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void GraphicsRender(
      IRenderer dg,
      Vector2 p,
      int c,
      bool printgraphics,
      bool printstatus)
    {
      if (printgraphics)
      {
        this._rect = new Rectangle(224, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

