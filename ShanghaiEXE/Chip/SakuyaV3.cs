﻿using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
  internal class SakuyaV3 : SakuyaV1
  {
    private const int interval = 20;
    private const int speed = 2;

    public SakuyaV3(MyAudio s)
      : base(s)
    {
      this.navi = true;
      this.number = 196;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SakuyaV3Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 50;
      this.subpower = 0;
      this.regsize = 61;
      this.reality = 5;
      this.swordtype = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.S;
      this.code[1] = ChipFolder.CODE.I;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.I;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SakuyaV3Desc");
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
        this._rect = new Rectangle(448, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

