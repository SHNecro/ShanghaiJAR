using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class LjiOtama2 : LjiOtama1
  {
    private const int start = 1;
    private const int speed = 2;

    public LjiOtama2(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 50;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.LjiOtama2Name");
      this.element = ChipBase.ELEMENT.aqua;
      this.power = 130;
      this.subpower = 0;
      this.regsize = 27;
      this.reality = 2;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.E;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.M;
      var information = NSGame.ShanghaiEXE.Translate("Chip.LjiOtama2Desc");
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
        this._rect = new Rectangle(392, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

