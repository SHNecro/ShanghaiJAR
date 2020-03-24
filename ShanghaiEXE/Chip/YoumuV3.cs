﻿using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class YoumuV3 : YoumuV1
  {
    private const int interval = 20;
    private const int speed = 2;
    private int[] motionList;
    private int nowmotion;
    private bool end;
    private Point target;
    private int command;
    private const int s = 5;

    public YoumuV3(MyAudio s)
      : base(s)
    {
      this.navi = true;
      this.number = 244;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.YoumuV3Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 120;
      this.subpower = 0;
      this.regsize = 66;
      this.reality = 5;
      this.swordtype = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.Y;
      this.code[1] = ChipFolder.CODE.K;
      this.code[2] = ChipFolder.CODE.Y;
      this.code[3] = ChipFolder.CODE.K;
      var information = NSGame.ShanghaiEXE.Translate("Chip.YoumuV3Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
      this.motionList = new int[4]{ 0, 1, 2, 3 };
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
        this._rect = new Rectangle(616, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic22", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

