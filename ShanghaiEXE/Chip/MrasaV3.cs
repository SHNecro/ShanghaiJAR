﻿using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class MrasaV3 : MrasaV1
  {
    private const int start = 44;
    private const int speed = 2;

    public MrasaV3(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 232;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MrasaV3Name");
      this.element = ChipBase.ELEMENT.aqua;
      this.power = 200;
      this.subpower = 0;
      this.regsize = 63;
      this.reality = 5;
      this._break = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.M;
      this.code[1] = ChipFolder.CODE.C;
      this.code[2] = ChipFolder.CODE.M;
      this.code[3] = ChipFolder.CODE.C;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MrasaV3Desc");
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
        this._rect = new Rectangle(448, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic19", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

