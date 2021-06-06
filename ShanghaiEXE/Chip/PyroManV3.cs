using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class PyroManV3 : PyroManV1
  {
    private const int speed = 2;

    public PyroManV3(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 229;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PyroManV3Name");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 90;
      this.subpower = 0;
      this.regsize = 86;
      this.reality = 5;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.P;
      this.code[1] = ChipFolder.CODE.F;
      this.code[2] = ChipFolder.CODE.P;
      this.code[3] = ChipFolder.CODE.F;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PyroManV3Desc");
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
        this._rect = new Rectangle(112, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic19", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

