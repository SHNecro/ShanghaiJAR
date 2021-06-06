using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class HakutakuManV2 : HakutakuManV1
  {
    private List<Point> target = new List<Point>();
    private int targetNow = -1;
    private const int interval = 20;
    private const int speed = 2;
    private new Point posi;
    private new bool beast;
    private new bool end;

    public HakutakuManV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 207;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.HakutakuManV2Name");
      this.element = ChipBase.ELEMENT.leaf;
      this.power = 90;
      this.subpower = 0;
      this.regsize = 45;
      this.reality = 4;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.H;
      this.code[1] = ChipFolder.CODE.K;
      this.code[2] = ChipFolder.CODE.H;
      this.code[3] = ChipFolder.CODE.K;
      var information = NSGame.ShanghaiEXE.Translate("Chip.HakutakuManV2Desc");
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
        this._rect = new Rectangle(728, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic15", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

