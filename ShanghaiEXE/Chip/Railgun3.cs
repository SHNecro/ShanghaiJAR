﻿using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class Railgun3 : Railgun1
  {
    private bool open;
    private const int shotend = 10;

    public Railgun3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 114;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.Railgun3Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 200;
      this.subpower = 0;
      this.regsize = 44;
      this.reality = 3;
      this._break = false;
      this.crack = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.I;
      this.code[1] = ChipFolder.CODE.M;
      this.code[2] = ChipFolder.CODE.R;
      this.code[3] = ChipFolder.CODE.Z;
      var information = NSGame.ShanghaiEXE.Translate("Chip.Railgun3Desc");
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
        this._rect = new Rectangle(840, 0, 56, 48);
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

