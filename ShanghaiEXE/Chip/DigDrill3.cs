using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DigDrill3 : DigDrill1
  {
    private const int start = 5;
    private const int speed = 3;

    public DigDrill3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.infight = true;
      this.number = 76;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DigDrill3Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 70;
      this.subpower = 0;
      this.regsize = 47;
      this.reality = 3;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.A;
      this.code[1] = ChipFolder.CODE.F;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.P;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DigDrill3Desc");
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
        this._rect = new Rectangle(616, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

