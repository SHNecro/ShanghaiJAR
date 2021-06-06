using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ChenV2 : ChenV1
  {
    public ChenV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 237;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ChenV2Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 50;
      this.subpower = 0;
      this.regsize = 41;
      this.reality = 4;
      this._break = true;
      this.crack = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.Y;
      this.code[2] = ChipFolder.CODE.C;
      this.code[3] = ChipFolder.CODE.Y;
      this.animePoint.X = -1;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ChenV2Desc");
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
        this._rect = new Rectangle(224, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic22", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

