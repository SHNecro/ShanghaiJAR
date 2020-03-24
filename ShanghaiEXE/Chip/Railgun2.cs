﻿using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class Railgun2 : Railgun1
  {
    private bool open;
    private const int shotend = 10;

    public Railgun2(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 113;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.Railgun2Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 140;
      this.subpower = 0;
      this.regsize = 34;
      this.reality = 2;
      this._break = false;
      this.crack = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.A;
      this.code[1] = ChipFolder.CODE.C;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.V;
      var information = NSGame.ShanghaiEXE.Translate("Chip.Railgun2Desc");
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
        this._rect = new Rectangle(784, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic3", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }

    public override void IconRender(
      IRenderer dg,
      Vector2 p,
      bool select,
      bool custom,
      int c,
      bool noicon)
    {
      if (!noicon)
      {
        this._rect = this.IconRect(select);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }
  }
}

