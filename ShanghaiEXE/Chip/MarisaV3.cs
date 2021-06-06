using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MarisaV3 : MarisaV1
  {
    private const int speed = 2;

    public MarisaV3(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 193;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MarisaV3Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 180;
      this.subpower = 0;
      this.regsize = 50;
      this.reality = 5;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.M;
      this.code[1] = ChipFolder.CODE.K;
      this.code[2] = ChipFolder.CODE.M;
      this.code[3] = ChipFolder.CODE.K;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MarisaV3Desc");
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
        dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

