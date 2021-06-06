using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class RanV2 : RanV1
  {
    private const int speed = 2;

    public RanV2(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 240;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.RanV2Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 20;
      this.subpower = 0;
      this.regsize = 47;
      this.reality = 4;
      this.hit = 6;
      this.targetY = new int[27];
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.R;
      this.code[1] = ChipFolder.CODE.Y;
      this.code[2] = ChipFolder.CODE.R;
      this.code[3] = ChipFolder.CODE.Y;
      var information = NSGame.ShanghaiEXE.Translate("Chip.RanV2Desc");
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
        this._rect = new Rectangle(56, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic22", this._rect, true, p, Color.White);
      }
      base.GraphicsRender(dg, p, c, false, printstatus);
    }
  }
}

