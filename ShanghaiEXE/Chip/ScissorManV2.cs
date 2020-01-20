﻿using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
  internal class ScissorManV2 : ScissorManV1
  {
    public ScissorManV2(MyAudio s)
      : base(s)
    {
      this.navi = true;
      this.number = 234;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ScissorManV2Name");
      this.element = ChipBase.ELEMENT.leaf;
      this.power = 230;
      this.subpower = 0;
      this.regsize = 43;
      this.reality = 4;
      this._break = true;
      this.obje = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.S;
      this.code[1] = ChipFolder.CODE.B;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.B;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ScissorManV2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
      this.colory = true;
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
        this._rect = new Rectangle(560, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic19", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

