using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DigDrill2 : DigDrill1
  {
    private const int start = 5;
    private const int speed = 3;

    public DigDrill2(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.number = 75;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DigDrill2Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 60;
      this.subpower = 0;
      this.regsize = 33;
      this.reality = 2;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.G;
      this.code[1] = ChipFolder.CODE.I;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.U;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DigDrill2Desc");
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
        this._rect = new Rectangle(560, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

