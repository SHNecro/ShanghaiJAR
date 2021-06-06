using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ElekiFang3 : ElekiFang1
  {
    private const int speed = 4;
    private const int shotend = 16;

    public ElekiFang3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.number = 129;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ElekiFang3Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 140;
      this.subpower = 0;
      this.regsize = 34;
      this.reality = 3;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.E;
      this.code[1] = ChipFolder.CODE.G;
      this.code[2] = ChipFolder.CODE.H;
      this.code[3] = ChipFolder.CODE.M;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ElekiFang3Desc");
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
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

